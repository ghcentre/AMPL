using Ampl.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ampl.Configuration;

/// <summary>
/// Provides the default implementation of the <see cref="IAppConfig"/> interface.
/// </summary>
public class AppConfig : IAppConfig
{
    private readonly IAppConfigStore _store;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppConfig"/> class.
    /// </summary>
    /// <param name="store">The config store.</param>
    /// <param name="configuration">The configuration.
    /// If the value is <see langword="null"/>, the default value /// provided by <see cref="ConfigurationFactory"/>
    /// is used.</param>
    public AppConfig(IAppConfigStore store, IAppConfigConfiguration configuration = null)
    {
        _store = Check.NotNull(store, nameof(store));

        if (configuration != null)
        {
            Configuration = configuration;
        }
        else
        {
            Configuration = ConfigurationFactory();
        }
    }

    /// <summary>
    /// Gets or sets the configuration factory.
    /// </summary>
    /// <value>The configuration factory used to provide an object implementing <see cref="IAppConfigConfiguration"/>
    /// if the <see cref="AppConfig.AppConfig(IAppConfigStore, IAppConfigConfiguration)"/> second parameter
    /// is <see langword="null"/>.</value>
    public static Func<IAppConfigConfiguration> ConfigurationFactory { get; set; } =
        () => new AppConfigDefaultConfiguration();

    /// <summary>
    /// The configuration.
    /// </summary>
    public IAppConfigConfiguration Configuration { get; set; }

    private IAppConfigEntity GetEntityUsingResolvers(string key, bool useResolvers)
    {
        var entity = _store.GetAppConfigEntity(key);

        if (useResolvers)
        {
            while (entity?.Value == null)
            {
                key = Configuration.ResolveDefaultKey(key);
                if (key == null)
                {
                    break;
                }

                entity = _store.GetAppConfigEntity(key);
            }
        }

        return entity;
    }

    private IEnumerable<IAppConfigEntity> GetEntitiesUsingResolvers(string key, string keyOperator, bool useResolvers)
    {
        if (keyOperator != "[" && keyOperator != ".")
        {
            throw new InvalidOperationException();
        }

        var entities = _store.GetAppConfigEntities(key + keyOperator).ToList();
        if (!useResolvers)
        {
            return entities;
        }

        string originalKey = key;
        while (entities.Count == 0)
        {
            key = Configuration.ResolveDefaultKey(key);
            if (key == null)
            {
                break;
            }
            entities = _store.GetAppConfigEntities(key + keyOperator).ToList();
        }

        var newEntities = entities
          .Select(x => new InternalAppConfigEntity()
          {
              Key = x.Key.Replace(key + keyOperator, originalKey + keyOperator),
              Value = x.Value
          })
          .ToList();

        return newEntities;
    }

    private Type GetGenericCollectionInterface(Type type)
    {
        //return type.Yield().Concat(type.GetInterfaces())
        //  .FirstOrDefault(x =>
        //    x.IsGenericType &&
        //    x.GetGenericTypeDefinition() == typeof(ICollection<>) &&
        //    x.GetGenericArguments().Length == 1);

        return type.Yield()
            .Concat(type.GetTypeInfo().ImplementedInterfaces)
            .FirstOrDefault(
                x => x.IsConstructedGenericType &&
                     x.GetGenericTypeDefinition() == typeof(ICollection<>) &&
                     x.GenericTypeArguments.Length == 1
            );
    }

    private Type GetGenericDictionaryInterface(Type type)
    {
        //return type.Yield().Concat(type.GetInterfaces())
        //  .FirstOrDefault(x =>
        //    x.IsGenericType &&
        //    x.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
        //    x.GetGenericArguments().Length == 2 &&
        //    (x.GetGenericArguments()[0] == typeof(string) || x.GetGenericArguments()[0] == typeof(object))
        //  );

        return type.Yield()
            .Concat(type.GetTypeInfo().ImplementedInterfaces)
            .FirstOrDefault(
                x => x.IsConstructedGenericType &&
                     x.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
                     x.GenericTypeArguments.Length == 2 &&
                     x.GenericTypeArguments[0].In(typeof(string), typeof(object))
            );
    }

    private static int CompareCollectionKeyIndexes(string key1, string key2)
    {
        int intKey1 = GetCollectionKeyIndex(key1);
        int intKey2 = GetCollectionKeyIndex(key2);

        if (intKey1 == intKey2)
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

        string lastPart = collectionKey.Between(prefix,
                                                null,
                                                StringBetweenOptions.None,
                                                StringComparison.OrdinalIgnoreCase);
        if (lastPart.ToNullIfWhiteSpace() == null)
        {
            throw new InvalidOperationException("Invalid collection indexer.");
        }

        string indexPart = lastPart.Between(null,
                                            "]",
                                            StringBetweenOptions.IncludeEnd,
                                            StringComparison.OrdinalIgnoreCase);

        return prefix + indexPart;
    }

    private static string ExtractDictionaryKey(string key)
    {
        key = key.Reverse().JoinWith("");
        key = key.Between("]",
                          "[",
                          StringBetweenOptions.None,
                          StringComparison.OrdinalIgnoreCase);

        if (key.ToNullIfWhiteSpace() == null)
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
        if (converter != null)
        {
            var entity = GetEntityUsingResolvers(key, useResolvers);
            if (entity != null)
            {
                list.Add(entity);
            }
        }

        //
        // type which interits from ICollection<>
        //
        else if (GetGenericCollectionInterface(targetType) != null)
        {
            list.AddRange(
                GetEntitiesUsingResolvers(key, "[", useResolvers) ?? Enumerable.Empty<IAppConfigEntity>()
            );
        }

        //
        // all other types are treated as object type
        //
        else
        {
            list.AddRange(
                GetEntitiesUsingResolvers(key, ".", useResolvers) ?? Enumerable.Empty<IAppConfigEntity>()
            );
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
        if (converter != null)
        {
            if (entities.Count == 1) // primitive types must exactly fit in one entity
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

        if (genericDictionaryType != null)
        {
            var argumentType = genericDictionaryType.GetTypeInfo().GenericTypeArguments[1];
            var addMethodInfo = genericDictionaryType.GetTypeInfo().GetDeclaredMethod("Add");
            object dictionary = Activator.CreateInstance(targetType);

            if (entities.Count == 0)
            {
                return dictionary;
            }

            var keys = entities.Select(x => GetCollectionObjectName(key, x.Key)).Distinct().ToList();
            keys.Sort((x, y) => CompareCollectionKeyIndexes(x, y));

            foreach (var k in keys)
            {
                object value = InternalGet(argumentType, k, useResolvers);
                string valueKey = ExtractDictionaryKey(k);
                addMethodInfo.Invoke(dictionary, new[] { valueKey, value });
            }

            return dictionary;
        }
        else if (genericCollectionType != null)
        {
            var argumentType = genericCollectionType.GenericTypeArguments[0];
            var addMethodInfo = genericCollectionType.GetTypeInfo().GetDeclaredMethod("Add");
            object collection = Activator.CreateInstance(targetType);

            if (entities.Count == 0)
            {
                return collection;
            }

            var keys = entities.Select(x => GetCollectionObjectName(key, x.Key)).Distinct().ToList();
            keys.Sort((x, y) => CompareCollectionKeyIndexes(x, y));

            foreach (var k in keys)
            {
                object value = InternalGet(argumentType, k, useResolvers);
                addMethodInfo.Invoke(collection, new[] { value });
            }

            return collection;
        }

        //
        // object types
        //
        if (entities.Count == 0)
        {
            return null;
        }

        object result = Activator.CreateInstance(targetType);
        var propertyInfos = targetType.GetTypeInfo()
            .DeclaredProperties
            .Where(x => x.CanRead && x.CanWrite && !x.GetMethod.IsStatic);

        foreach (var propertyInfo in propertyInfos)
        {
            string propertyKey = key + "." + propertyInfo.Name;
            object propertyValue = InternalGet(propertyInfo.PropertyType, propertyKey, useResolvers);
            propertyInfo.SetValue(result, propertyValue);
        }

        return result;
    }

    /// <inheritdoc/>
    public T Get<T>(string key, bool useResolvers = true)
    {
        Check.NotNull(key, nameof(key));

        var targetType = typeof(T);
        var result = InternalGet(targetType, key, useResolvers);

        if (result == null && targetType.GetTypeInfo().IsValueType)
        {
            return (T)Activator.CreateInstance(targetType);
        }

        return (T)result;
    }

    private void SetEntity(string key, string value)
    {
        var entity = _store.CreateAppConfigEntity();
        entity.Key = key;
        entity.Value = value;

        _store.SetAppConfigEntity(entity);
    }

    private void InternalSet(Type sourceType, string key, object value)
    {
        var converter = Configuration.GetConverters().FirstOrDefault(x => x.CanConvert(sourceType));

        //
        // primitive types and types handled with type converters
        //
        if (converter != null)
        {
            SetEntity(key, converter.WriteEntity(value));
            return;
        }

        //
        // types that inherit from ICollection<> or IDictionary<,>
        //
        var genericDictionaryType = GetGenericDictionaryInterface(sourceType);
        var genericCollectionType = GetGenericCollectionInterface(sourceType);

        if (genericDictionaryType != null)
        {
            //var argumentType = genericDictionaryType.GetGenericArguments()[1];
            var argumentType = genericDictionaryType.GenericTypeArguments[1];

            //
            // remove old entities
            //
            foreach (var oldEntity in GetEntitiesUsingResolvers(key, "[", false))
            {
                _store.DeleteAppConfigEntity(oldEntity);
            }
            _store.SaveAppConfigChanges();


            if (value != null)
            {
                var keysPropInfo = genericDictionaryType.GetTypeInfo().GetDeclaredProperty("Keys");
                var keysPropGetMethod = keysPropInfo.GetMethod;
                object keys = keysPropGetMethod.Invoke(value, new object[] { });

                var thisPropInfo = genericDictionaryType.GetTypeInfo().GetDeclaredProperty("Item");
                var itemPropGetMethod = thisPropInfo.GetMethod;

                foreach (object k in (IEnumerable)keys)
                {
                    object dictionaryItem = itemPropGetMethod.Invoke(value, new object[] { k });
                    InternalSet(argumentType, $"{key}[{k}]", dictionaryItem);
                }
            }

            return;
        }
        else if (genericCollectionType != null)
        {
            var argumentType = genericCollectionType.GenericTypeArguments[0];

            foreach (var oldEntity in GetEntitiesUsingResolvers(key, "[", false))
            {
                _store.DeleteAppConfigEntity(oldEntity);
            }
            _store.SaveAppConfigChanges();

            if (value != null)
            {
                int index = 0;
                foreach (object collectionItem in (value as IEnumerable)) // ICollection<> inhetits from IEnumerable
                {
                    InternalSet(argumentType, $"{key}[{index}]", collectionItem);
                    index++;
                }
            }

            return;
        }

        var propertyInfos = sourceType
            .GetTypeInfo()
            .DeclaredProperties
            .Where(x => x.CanRead && x.CanWrite && !x.GetMethod.IsStatic);

        //
        // remove old entities
        //
        foreach (var oldEntity in GetEntitiesUsingResolvers(key, ".", false))
        {
            _store.DeleteAppConfigEntity(oldEntity);
        }
        _store.SaveAppConfigChanges();

        if (value != null)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                string propertyKey = key + "." + propertyInfo.Name;
                object propertyValue = propertyInfo.GetValue(value);
                InternalSet(propertyInfo.PropertyType, propertyKey, propertyValue);
            }
        }
    }

    /// <inheritdoc/>
    public void Set<T>(string key, T value)
    {
        Check.NotNull(key, nameof(key));
        var sourceType = typeof(T);

        using (var transaction = _store.BeginAppConfigTransaction())
        {
            try
            {
                InternalSet(sourceType, key, value);
                _store.SaveAppConfigChanges();
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
