using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Ampl.System
{
  /// <summary>
  /// Provides a set of <see langword="static"/> methods for objects
  /// that implement <see cref="IDictionary{TKey, TValue}"/>.
  /// </summary>
  public static class DictionaryExtensions
  {
    /// <summary>
    /// Adds all of the public properties of the object to the dictionary.
    /// </summary>
    /// <typeparam name="T">The type of the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary, of type <b>IDictionary&lt;System.String, System.Object&gt;</b>
    /// to add to.</param>
    /// <param name="anonymousType">The object of an anonymous type whose property names and values are added
    /// to the <paramref name="dictionary"/>.</param>
    /// <returns>The methods adds the property names of the <paramref name="anonymousType"/> object as the keys, and
    /// property values as the values to the dictionary and returns the dictionary.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="dictionary"/> is <see langword="null"/>, <b>or</b>,
    /// the <paramref name="anonymousType"/> is <see langword="null"/>.</exception>
    public static T IncludeObjectProperties<T>(this T dictionary, object anonymousType) where T : IDictionary<string, object>
    {
      Check.NotNull(dictionary, nameof(dictionary));
      Check.NotNull(anonymousType, nameof(anonymousType));
      var dict = anonymousType.GetType()
                              //.GetRuntimeProperties()
                              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                              .ToDictionary(prop => prop.Name,
                                            prop => prop.GetValue(anonymousType, null));
      foreach(var item in dict)
      {
        dictionary[item.Key] = item.Value;
      }
      return dictionary;
    }


    /// <summary>
    /// Gets the element with the specified key from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type or the valye.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key of the element to get.</param>
    /// <returns>The element with the specified key, or the default value of <typeparamref name="TKey"/> if the
    /// <paramref name="key"/> is not found.</returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
      Check.NotNull(dictionary, nameof(dictionary));

      TValue value = default(TValue);
      dictionary.TryGetValue(key, out value);
      return value;
    }
  }
}
