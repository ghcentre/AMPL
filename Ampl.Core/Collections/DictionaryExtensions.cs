using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ampl.Core;

/// <summary>
/// Provides a set of <see langword="static"/> methods for objects that implement <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Adds all of the public properties of the object to the dictionary.
    /// </summary>
    /// <typeparam name="T">The type of the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary, of type <c>IDictionary&lt;System.String, System.Object&gt;</c>
    /// to add to.</param>
    /// <param name="anonymousType">The object (maybe of an anonymous type) whose property names and values have to be added
    /// to the <paramref name="dictionary"/>.</param>
    /// <returns>The methods adds the property names of the <paramref name="anonymousType"/> object as the keys, and
    /// property values as the values to the dictionary and returns the dictionary.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="dictionary"/> is <see langword="null"/>, <b>or</b>,
    /// the <paramref name="anonymousType"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code><![CDATA[
    /// var anonymousObject = new { id = 42, title = "Hello there", createdAt = new DateTime(2001, 01, 01) };
    /// var dic = new Dictionary<string, object>().IncludeObjectProperties(anonymousObject);
    /// //
    /// // dic now contains:
    /// // { { "id", 42 }, { "title", "Hello there" }, { "createdAt", [01.01.2001 0:00:00] } }
    /// //
    /// ]]></code>
    /// </example>
    /// <remarks>
    /// <para>This method does <b>not</b> read public properties of the <paramref name="anonymousType"/> object, instead,
    /// it uses a reflection mechanism to read private compiler-generated backing fields.</para>
    /// <para>It's the only way allowed in <b>Mono.iOS</b> due to its restrictions.</para>
    /// <para>The method may stop working if the compiler-generated backing field naming changes
    /// <i>(it's very unkilely, however)</i>.</para>
    /// </remarks>
    public static T IncludeObjectProperties<T>(this T dictionary, object anonymousType) where T : IDictionary<string, object>
    {
        Guard.Against.Null(dictionary, nameof(dictionary));
        Guard.Against.Null(anonymousType, nameof(anonymousType));

        var dict = anonymousType.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .ToDictionary(
                prop => prop.Name.Between("<", ">i__Field"),
                prop => prop.GetValue(anonymousType));

        foreach (var item in dict)
        {
            dictionary[item.Key!] = item.Value;
        }

        return dictionary;
    }
}
