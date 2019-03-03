using System;

namespace Ampl.System
{
  /// <summary>
  /// Provides a set of <see langword="static"/> extension methods to work with <c>enum</c>s.
  /// </summary>
  public static class EnumExtensions
  {
    public static T ParseValue<T>(this T enumeration,
                                  string source,
                                  bool ignoreCase = false)
      where T : struct, IComparable, IFormattable
    {
      return (T)Enum.Parse(typeof(T), source, ignoreCase);
    }

    public static T? ParseAsNullable<T>(this T enumeration,
                                        string source,
                                        bool ignoreCase = false)
      where T : struct, IComparable, IFormattable
    {
      bool success = Enum.TryParse<T>(source, ignoreCase, out T result);
      if(!success)
      {
        return null;
      }
      return result;
    }
  }
}
