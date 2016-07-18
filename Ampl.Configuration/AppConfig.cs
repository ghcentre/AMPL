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

    public AppConfig(IAppConfigRepository repository, IAppConfigConfiguration configuration = null)
    {
      _repository = Check.NotNull(repository, nameof(repository));

      if(configuration != null)
      {
        Configuration = configuration;
      }
      else
      {
        Configuration = ConfigurationFactory();
      }
    }

    public static Func<IAppConfigConfiguration> ConfigurationFactory { get; set; } = () => new AppConfigDefaultConfiguration();

    public IAppConfigConfiguration Configuration { get; set; }

    private IAppConfigEntity GetEntityUsingResolvers(string key, bool useResolvers)
    {
      IAppConfigEntity entity = _repository.GetAppConfigEntity(key);
      if(useResolvers)
      {
        while(entity?.Value == null)
        {
          key = Configuration.ResolveDefaultKey(key);
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
      if(!useResolvers)
      {
        return entities;
      }

      string originalKey = key;
      while(entities.Count == 0)
      {
        key = Configuration.ResolveDefaultKey(key);
        if(key == null)
        {
          break;
        }
        entities = _repository.GetAppConfigEntities(key + keyOperator).ToList();
      }
      var newEntities = entities
        .Select(x => new InternalAppConfigEntity() {
          Key = x.Key.Replace(key + keyOperator, originalKey + keyOperator),
          Value = x.Value
        })
        .ToList();
      return newEntities;
    }

    private Type GetGenericCollectionInterface(Type type)
    {
      return type.Yield().Concat(type.GetInterfaces())
        .FirstOrDefault(x =>
          x.IsGenericType &&
          x.GetGenericTypeDefinition() == typeof(ICollection<>) &&
          x.GetGenericArguments().Length == 1);
    }

    private Type GetGenericDictionaryInterface(Type type)
    {
      return type.Yield().Concat(type.GetInterfaces())
        .FirstOrDefault(x =>
          x.IsGenericType &&
          x.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
          x.GetGenericArguments().Length == 2 &&
          (x.GetGenericArguments()[0] == typeof(string) || x.GetGenericArguments()[0] == typeof(object))
        );
    }

    private static int CompareCollectionKeyIndexes(string key1, string key2)
    {
      int intKey1 = GetCollectionKeyIndex(key1);
      int intKey2 = GetCollectionKeyIndex(key2);
      if(intKey1 == intKey2)
      {
        return key1.CompareTo(key2);
      }
      return intKey1.CompareTo(intKey2);
    }

    private static int GetCollectionKeyIndex(string collectionKey)
    {
      //
      // some.object[1234].other.stuff => 1234
      //
      string mid = collectionKey.Between("[", null, StringBetweenOptions.None, StringComparison.OrdinalIgnoreCase);
      string digit = mid.Between(null, "]", StringBetweenOptions.None, StringComparison.OrdinalIgnoreCase);
      return digit.ToInt();
    }

    private static string GetCollectionObjectName(string collectionObjectPrefix, string collectionKey)
    {
      //
      // prefix: some.object
      // key:    some.object[1234].other.prop[5678].stuff
      // result: some.object[1234]
      //
      // prefix: some.object[1234].other.prop
      // key:    some.object[1234].other.prop[5678].stuff
      // result: some.object[1234].other.prop[5678]
      //
      string prefix = collectionObjectPrefix + "[";

      string lastPart = collectionKey.Between(prefix, null,
        StringBetweenOptions.None,
        StringComparison.OrdinalIgnoreCase);
      if(lastPart.ToNullIfWhiteSpace() == null)
      {
        throw new InvalidOperationException("Invalid collection indexer.");
      }

      string indexPart = lastPart.Between(null, "]",
        StringBetweenOptions.IncludeEnd,
        StringComparison.OrdinalIgnoreCase);

      return prefix + indexPart;
    }

    private static string ExtractDictionaryKey(string key)
    {
      key = key.Reverse().JoinWith("");
      key = key.Between("]", "[", StringBetweenOptions.None, StringComparison.OrdinalIgnoreCase);
      if(key.ToNullIfWhiteSpace() == null)
      {
        throw new InvalidOperationException("Invalid dictionary indexer.");
      }
      return key.Reverse().JoinWith("");
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
      // primitive types and types handled with type converters
      //
      if(converter != null)
      {
        if(entities.Count == 1) // primitive types must exactly fit in one entity
        {
          return converter.ReadEntity(entities[0].Value);
        }
        return null;
      }

      //
      // types that inherit from ICollection<> or IDictionary<,>
      //
      var genericDictionaryType = GetGenericDictionaryInterface(targetType);
      var genericCollectionType = GetGenericCollectionInterface(targetType);

      if(genericDictionaryType != null)
      {
        var argumentType = genericDictionaryType.GetGenericArguments()[1];
        var addMethodInfo = genericDictionaryType.GetMethod("Add");
        object dictionary = Activator.CreateInstance(targetType);

        if(entities.Count == 0)
        {
          return dictionary;
        }

        var keys = entities.Select(x => GetCollectionObjectName(key, x.Key)).Distinct().ToList();
        keys.Sort((x, y) => CompareCollectionKeyIndexes(x, y));
        foreach(var k in keys)
        {
          object value = InternalGet(argumentType, k, useResolvers);
          string valueKey = ExtractDictionaryKey(k);
          addMethodInfo.Invoke(dictionary, new[] { valueKey, value });
        }
        return dictionary;
      }
      else if(genericCollectionType != null)
      {
        var argumentType = genericCollectionType.GetGenericArguments()[0];

        //var typeConverter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(argumentType));
        //if(typeConverter == null)
        //{
        //  throw new InvalidOperationException("Only collections of primitive types are supported.");
        //}

        var addMethodInfo = genericCollectionType.GetMethod("Add");
        object collection = Activator.CreateInstance(targetType);

        if(entities.Count == 0)
        {
          return collection;
        }

        //entities.Sort((x, y) => GetCollectionKeyIndex(x.Key) - GetCollectionKeyIndex(y.Key));
        //foreach(var entity in entities)
        //{
        //  object value = typeConverter.ReadEntity(entity.Value);
        //  addMethodInfo.Invoke(collection, new[] { value });
        //}

        var keys = entities.Select(x => GetCollectionObjectName(key, x.Key)).Distinct().ToList();
        keys.Sort((x, y) => CompareCollectionKeyIndexes(x, y));
        foreach(var k in keys)
        {
          object value = InternalGet(argumentType, k, useResolvers);
          addMethodInfo.Invoke(collection, new[] { value });
        }
        return collection;
      }

      //
      // object types
      //
      if(entities.Count == 0)
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
      // primitive types and types handled with type converters
      //
      if(converter != null)
      {
        SetEntity(key, converter.WriteEntity(value));
        return;
      }

      //
      // types that inherit from ICollection<> or IDictionary<,>
      //
      var genericDictionaryType = GetGenericDictionaryInterface(sourceType);
      var genericCollectionType = GetGenericCollectionInterface(sourceType);

      if(genericDictionaryType != null)
      {
        var argumentType = genericDictionaryType.GetGenericArguments()[1];

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
          var keysPropInfo = genericDictionaryType.GetProperty("Keys");
          var keysPropGetMethod = keysPropInfo.GetGetMethod();
          object keys = keysPropGetMethod.Invoke(value, new object[] { });

          var thisPropInfo = genericDictionaryType.GetProperty("Item");
          var itemPropGetMethod = thisPropInfo.GetGetMethod();

          foreach(object k in (IEnumerable)keys)
          {
            object dictionaryItem = itemPropGetMethod.Invoke(value, new object[] { k });
            InternalSet(argumentType, $"{key}[{k}]", dictionaryItem);
          }
        }
        return;
      }
      else if(genericCollectionType != null)
      {
        var argumentType = genericCollectionType.GetGenericArguments()[0];

        //var typeConverter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(argumentType));
        //if(typeConverter == null)
        //{
        //  throw new InvalidOperationException("Only collections of primitive types are supported.");
        //}

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
          //foreach(object collectionItem in (value as IEnumerable)) // ICollection<> inhetits from IEnumerable
          //{
          //  SetEntity($"{key}[{index}]", typeConverter.WriteEntity(collectionItem));
          //  index++;
          //}
          foreach(object collectionItem in (value as IEnumerable)) // ICollection<> inhetits from IEnumerable
          {
            InternalSet(argumentType, $"{key}[{index}]", collectionItem);
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
