using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;
using System.Reflection;

namespace Ampl.Configuration
{
  public class AppConfig
  {
    private IAppConfigRepository _repository;

    public AppConfig(IAppConfigRepository repository)
    {
      _repository = Check.NotNull(repository, nameof(repository));
    }

    public static IAppConfigConfiguration DefaultConfiguration { get; set; } = new AppConfigDefaultConfiguration();

    public IAppConfigConfiguration Configuration { get; set; } = (IAppConfigConfiguration)DefaultConfiguration.Clone();

    private IAppConfigEntity GetEntityUsingResolvers(string key, bool useResolvers)
    {
      IAppConfigEntity entity = _repository.GetAppConfigEntity(key);
      if(useResolvers)
      {
        while(entity?.Value == null)
        {
          key = Configuration.GetKeyResolver(key);
          if(key == null)
          {
            break;
          }
          entity = _repository.GetAppConfigEntity(key);
        }
      }
      return entity;
    }

    private IEnumerable<IAppConfigEntity> GetEntitiesUsingResolvers(string key, string keyOperator, bool useResolvers)
    {
      if(keyOperator != "[" && keyOperator != ".")
      {
        throw new InvalidOperationException();
      }

      var entities = _repository.GetAppConfigEntities(key + keyOperator).ToList();
      if(useResolvers)
      {
        while(entities.Count == 0)
        {
          key = Configuration.GetKeyResolver(key);
          if(key == null)
          {
            break;
          }
          entities = _repository.GetAppConfigEntities(key + keyOperator).ToList();
        }
      }
      return entities;
    }

    private Type GetGenericCollectionInterface(Type type)
    {
      return new[] { type }.Concat(type.GetInterfaces())
        .FirstOrDefault(x =>
          x.IsGenericType &&
          x.GetGenericTypeDefinition() == typeof(ICollection<>) &&
          x.GetGenericArguments().Length == 1);
    }

    private static int GetCollectionKeyIndex(string collectionKey)
    {
      string mid = collectionKey.Between("[", null, StringBetweenOptions.None, StringComparison.OrdinalIgnoreCase);
      string digit = mid.Between(null, "]", StringBetweenOptions.None, StringComparison.OrdinalIgnoreCase);
      return digit.ToInt();
    }

    private List<IAppConfigEntity> GetEntities(Type targetType, IAppConfigConverter converter, string key, bool useResolvers)
    {
      var list = new List<IAppConfigEntity>();

      //
      // primitive type for which a converter exists
      //
      if(converter != null)
      {
        var entity = GetEntityUsingResolvers(key, useResolvers);
        if(entity != null)
        {
          list.Add(entity);
        }
      }

      //
      // type which interits from ICollection<>
      //
      else if(GetGenericCollectionInterface(targetType) != null)
      {
        list.AddRange(GetEntitiesUsingResolvers(key, "[", useResolvers).ToEmptyIfNull());
      }

      //
      // all other types are treated as object type
      //
      else
      {
        list.AddRange(GetEntitiesUsingResolvers(key, ".", useResolvers).ToEmptyIfNull());
      }

      return list;
    }

    private object InternalGet(Type targetType, string key, bool useResolvers = true)
    {
      var converter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(targetType));
      var entities = GetEntities(targetType, converter, key, useResolvers);

      //
      // primitive types and these types which are handled with type converters
      //
      if(converter != null)
      {
        if(entities.Count == 1)
        {
          return converter.ReadEntity(entities[0].Value);
        }
        return null;
      }

      //
      // types that interit from ICollection<>
      //
      var genericCollectionType = GetGenericCollectionInterface(targetType);
      if(genericCollectionType != null)
      {
        var argumentType = genericCollectionType.GetGenericArguments()[0];

        var typeConverter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(argumentType));
        if(typeConverter == null)
        {
          throw new InvalidOperationException("Only collections of primitive types are supported.");
        }

        var addMethodInfo = genericCollectionType.GetMethod("Add");
        object collection = Activator.CreateInstance(targetType);

        if(entities.Count == 0)
        {
          return collection;
        }

        entities.Sort((x, y) => GetCollectionKeyIndex(x.Key) - GetCollectionKeyIndex(y.Key));
        foreach(var entity in entities)
        {
          object value = typeConverter.ReadEntity(entity.Value);
          addMethodInfo.Invoke(collection, new[] { value });
        }
        return collection;
      }

      //
      // object types
      //
      if(entities.Count== 0)
      {
        return null;
      }

      object result = Activator.CreateInstance(targetType);
      var propertyInfos = targetType
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(x => x.CanRead && x.CanWrite);

      foreach(var propertyInfo in propertyInfos)
      {
        string propertyKey = key + "." + propertyInfo.Name;
        object propertyValue = InternalGet(propertyInfo.PropertyType, propertyKey, useResolvers);
        propertyInfo.SetValue(result, propertyValue);
      }
      return result;
    }

    public T Get<T>(string key, bool useResolvers = true)
    {
      Check.NotNull(key, nameof(key));
      var targetType = typeof(T);
      var result = InternalGet(targetType, key, useResolvers);
      if(result == null && targetType.IsValueType)
      {
        return (T)Activator.CreateInstance(targetType);
      }
      return (T)result;
    }

    private void SetEntity(string key, string value)
    {
      var entity = _repository.CreateAppConfigEntity();
      entity.Key = key;
      entity.Value = value;
      _repository.SetAppConfigEntity(entity);
    }

    private void SetNullEntity(string key)
    {
      foreach(var oldEntity in GetEntitiesUsingResolvers(key, ".", false))
      {
        _repository.DeleteAppConfigEntity(oldEntity);
      }

      foreach(var oldEntity in GetEntitiesUsingResolvers(key, "[", false))
      {
        _repository.DeleteAppConfigEntity(oldEntity);
      }

      _repository.SaveAppConfigChanges();
      SetEntity(key, null);
    }

    private void InternalSet(Type sourceType, string key, object value)
    {
      var converter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(sourceType));

      //
      // primitive types and these types which are handled with type converters
      //
      if(converter != null)
      {
        SetEntity(key, converter.WriteEntity(value));
        return;
      }

      //
      // types that interit from ICollection<>
      //
      var genericCollectionType = GetGenericCollectionInterface(sourceType);
      if(genericCollectionType != null)
      {
        var argumentType = genericCollectionType.GetGenericArguments()[0];

        var typeConverter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(argumentType));
        if(typeConverter == null)
        {
          throw new InvalidOperationException("Only collections of primitive types are supported.");
        }

        //
        // remove old entities
        //
        foreach(var oldEntity in GetEntitiesUsingResolvers(key, "[", false))
        {
          _repository.DeleteAppConfigEntity(oldEntity);
        }
        _repository.SaveAppConfigChanges();

        if(value != null)
        {
          int index = 0;
          foreach(object collectionItem in (value as IEnumerable)) // ICollection<> inhetits from IEnumerable
          {
            SetEntity($"{key}[{index}]", typeConverter.WriteEntity(collectionItem));
            index++;
          }
        }
        return;
      }

      //
      // object types
      //
      var propertyInfos = sourceType
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(x => x.CanRead && x.CanWrite);

      //
      // remove old entities
      //
      foreach(var oldEntity in GetEntitiesUsingResolvers(key, ".", false))
      {
        _repository.DeleteAppConfigEntity(oldEntity);
      }
      _repository.SaveAppConfigChanges();

      if(value != null)
      {
        foreach(var propertyInfo in propertyInfos)
        {
          string propertyKey = key + "." + propertyInfo.Name;
          object propertyValue = propertyInfo.GetValue(value);
          InternalSet(propertyInfo.PropertyType, propertyKey, propertyValue);
        }
      }
    }

    public void Set<T>(string key, T value)
    {
      Check.NotNull(key, nameof(key));
      var sourceType = typeof(T);

      using(var transaction = _repository.BeginAppConfigTransaction())
      {
        try
        {
          InternalSet(sourceType, key, value);
          _repository.SaveAppConfigChanges();
          transaction.Commit();
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }
  }
}
