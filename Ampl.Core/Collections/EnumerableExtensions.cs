using System.Collections.Generic;
using System.Linq;

namespace Ampl.Core;

/// <summary>
/// Provides a set of <see langword="static"/> methods extending the <see cref="IEnumerable{T}"/> interface.
/// </summary>
public static class EnumerableExtensions
{
    /*
    /// <summary>
    /// Converts the <see langword="null"/> reference to the <see cref="IEnumerable{T}"/> to the empty enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the enumerable to convert.</typeparam>
    /// <param name="source">The reference to convert.</param>
    /// <returns>If the reference passed in <paramref name="source"/> is <see langword="null"/>, the method
    /// returns the empty <see cref="IEnumerable{T}"/>. Otherwise, the method returns the same reference as passed
    /// in <paramref name="source"/>.</returns>
    public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T> source)
    {
      return source ?? Enumerable.Empty<T>();
    }
    */

    /// <summary>
    /// Determines whether an array contains a specified element by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="source">The value to locate in the array.</param>
    /// <param name="checkValues">An array of elements in which to locate the value.</param>
    /// <returns><see langword="true"/> if the source array contains an element that has the specified value;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool In<T>(this T source, params T[] checkValues)
    {
        return source.In(checkValues as IEnumerable<T>);
    }

    /// <summary>
    /// Determines whether a sequence contains a specified element by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="source">The value to locate in the sequence.</param>
    /// <param name="checkValues">A sequence of elements in which to locate the value.</param>
    /// <returns><see langword="true"/> if the source sequence contains an element that has the specified value;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool In<T>(this T source, IEnumerable<T> checkValues)
    {
        return (checkValues ?? Enumerable.Empty<T>()).Contains(source);
    }

    /// <summary>
    /// Returns a new sequence containing exactly one element of <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="source">The source value.</param>
    /// <returns>The sequence conaining exactly on element of source.</returns>
    public static T[] Yield<T>(this T source)
    {
        return new[] { source };
    }

    /// <summary>
    /// Concatenates the members of a collection, using the specified separator between each member.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values">The sequence of the values to concatenate.</param>
    /// <param name="separator">The separator value. If the parameter is <see langword="null"/>, the empty string
    /// is used as the separator.</param>
    /// <returns>A string that consists of the members of values delimited by the separator string.
    /// If values has no members, the method returns an empty string.</returns>
    public static string JoinWith<T>(this IEnumerable<T> values, string separator)
    {
        return string.Join(separator, values);
    }
}
