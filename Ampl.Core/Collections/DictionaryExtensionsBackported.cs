using Ampl.Core;
using System;
using System.Collections.Generic;

namespace Ampl.Collections
{
    /// <summary>
    /// Provides a set of <see langword="static"/> methods for objects
    /// that implement <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <remarks>
    /// <para>This class is a temporary workaround until
    /// System.Collections.Generic.CollectionExtensions.GetValueOrDefault{TKey, TValue}
    /// appears in .NET Standard 3.0</para>
    /// </remarks>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the element with the specified key from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type or the valye.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>The element with the specified key, or the default value of <typeparamref name="TKey"/> if the
        /// <paramref name="key"/> is not found.</returns>
        /// <remarks>Will become obsolete after the .NET Standard 3.0 release.</remarks>
        //[Obsolete(
        //    "This method is obsolete. " +
        //    "Use System.Collections.Generic.CollectionExtensions.GetValueOrDefault instead. " +
        //    "If the method is not available on your platform, disable warning using #pragma warning disable CS0618."
        //)]
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }
    }
}
