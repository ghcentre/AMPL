using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.System
{
  /// <summary>
  /// Provides a set of <see langword="static"/> methods extending the <see cref="IEnumerable{T}"/> interface.
  /// </summary>
  public static class EnumerableExtensions
  {
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

    public static bool In<T>(this T source, params T[] checkValues)
    {
      return source.In(checkValues as IEnumerable<T>);
    }

    public static bool In<T>(this T source, IEnumerable<T> checkValues)
    {
      return checkValues.ToEmptyIfNull().Any(x => x.Equals(source));
    }

    public static T[] Yield<T>(this T source)
    {
      return new[] { source };
    }

    public static string JoinWith<T>(this IEnumerable<T> values, string separator)
    {
      return string.Join(separator, values);
    }
  }
}
