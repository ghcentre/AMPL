using Ampl.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ampl.Core;

/// <summary>
/// Provides a set of <see langword="static"/> methods extending the <see cref="IEnumerable{T}"/> interface.
/// </summary>
public static class EnumerableExtensions
{
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
    [Obsolete("Use new[] { source }")]
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

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> based on traversing the "linked list" represented by the head <paramref name="node"/>
    /// and the <paramref name="next"/> delegate. The sequence terminates when a <see langword="null"/> node is reached.
    /// </summary>
    /// <typeparam name="T">The type of node in the list.</typeparam>
    /// <param name="node">The head node of the list</param>
    /// <param name="next">A delegate that, given a node in the sequence, generates the next node in the sequence.</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    /// <exception cref="ArgumentNullException">The <paramref name="next"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> Traverse<T>(this T? node, Func<T?, T?> next)
    {
        Guard.Against.Null(next, nameof(next));

        for (var current = node; current != null; current = next(current))
        {
            yield return current;
        }
    }
}
