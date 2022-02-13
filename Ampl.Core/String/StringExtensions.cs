using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ampl.Core;

/// <summary>
/// Provides a set of <see langword="static"/> methods for <see cref="String">string</see>s.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Retrieves a substring from the string instance between two substrings.
    /// </summary>
    /// <param name="source">The string to retrieve substring from.</param>
    /// <param name="start">The substring in the source string that is used as the start of resulting substring.
    /// If the parameter is <see langword="null"/> or empty string (""), the substring is retrieved from the start
    /// of the source string.</param>
    /// <param name="end">The substring in the source string that is used as the end of resulting substring.
    /// If the parameter is <see langword="null"/> or empty string value, the substring is retrieved until the end
    /// of the source string.</param>
    /// <param name="options">The <see cref="StringBetweenOptions"/> enumerations that specifies various options.
    /// The default value is <see cref="StringBetweenOptions.None"/>.</param>
    /// <param name="comparison">The <see cref="StringComparison"/> enumerations that specifies string comparison options.
    /// The default value is <see cref="StringComparison.CurrentCulture"/>.</param>
    /// <returns>
    /// <para>A string that is equivalent to substring that begins at position of the <paramref name="start"/> substring
    /// and ends at position of the <paramref name="end"/> substring.</para>
    /// <para><em>-or-</em></para>
    /// <para>an empty string ("").</para>
    /// <para><i>See the remarks section.</i></para>
    /// </returns>
    /// <remarks>
    /// <para>This method returns the substring of the string specified in <paramref name="source"/>.
    /// The start and end positions of the substring are the positions of the <paramref name="start"/> and <paramref name="end"/>
    /// substrings in the source string.</para>
    /// <para>If the non-empty <c>start</c>, or non-empty <c>end</c>, or both substrings are <i>not</i> found  in the source string,
    /// the return value depends on the <see cref="StringBetweenOptions.FallbackToSource"/> flag.
    /// If it is set, the original string is returned, otherwise, an empty string ("").</para>
    /// <para>See the <see cref="StringBetweenOptions"/> enumeration for information about other options.</para>
    /// <seealso cref="StringBetweenOptions"/>
    /// <seealso cref="RemoveBetween(string, string, string, StringComparison)"/>
    /// </remarks>
    /// <example>
    /// The following code demonstrates usage of the <b>Between</b> method:
    /// <code>
    /// public void Method1()
    /// {
    ///     string source = "This is a test string.";
    ///     string s1 = source.Between(null, "test"); // returns "This is a"
    ///     string s2 = source.Between("test", null); // returns " string."
    ///     string s3 = source.Between("One", null); // returns an empty string.
    ///     string s4 = source.Between("One", null, StringBetweenOptions.FallbackToSource); // returns a copy of source
    ///     string s5 = source.Between("This", "test", StringBetweenOptions.IncludeStart); // returns "This is a "
    ///     string s6 = source.Between("thIS", "StR", StringBetweenOptions.None, StringComparison.CurrentCultureIgnoreCase); // return " is a test "
    /// }
    /// </code>
    /// </example>
    public static string? Between(this string? source,
                                  string? start,
                                  string? end,
                                  StringBetweenOptions options = StringBetweenOptions.None,
                                  StringComparison comparison = StringComparison.CurrentCulture)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }
        
        start ??= string.Empty;
        end ??= string.Empty;

        static bool IsEmpty(string s) => s.Length == 0;
        static bool Found(int position) => position != -1;
        static bool ShouldIncludeStart(StringBetweenOptions o) =>
            (o & StringBetweenOptions.IncludeStart) == StringBetweenOptions.IncludeStart;
        static bool ShouldIncludeEnd(StringBetweenOptions o) =>
            (o & StringBetweenOptions.IncludeEnd) == StringBetweenOptions.IncludeEnd;
        static bool ShouldFallbackToSource(StringBetweenOptions o) =>
            (o & StringBetweenOptions.FallbackToSource) == StringBetweenOptions.FallbackToSource;

        int startPosition = IsEmpty(start) ? 0 : source.IndexOf(start, comparison);
        if (!Found(startPosition))
        {
            return ShouldFallbackToSource(options) ? source : string.Empty;
        }
        startPosition += start.Length;

        int endPosition = IsEmpty(end) ? source.Length : source.IndexOf(end, startPosition, comparison);
        if (!Found(endPosition))
        {
            return ShouldFallbackToSource(options) ? source : string.Empty;
        }

        startPosition = ShouldIncludeStart(options) ? startPosition - start.Length : startPosition;
        endPosition = ShouldIncludeEnd(options) ? endPosition + end.Length : endPosition;

        int length = endPosition - startPosition;
        
        string substring = source.Substring(startPosition, length);
        return substring;
    }

    /// <summary>
    /// Removes a part of a string between specified <c>start</c> and <c>end</c>.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="start">The start of the string to remove.</param>
    /// <param name="end">The end of the string to remove</param>
    /// <param name="comparison"><see cref="StringComparison"/>.</param>
    /// <returns>
    /// <para>If the string specified in <paramref name="source"/> is <see langword="null"/> or an empty string, the method returns
    /// a reference to the source string.</para>
    /// <para>If the strings specified in <paramref name="start"/> or <paramref name="end"/>
    /// are not found in the <paramref name="source"/> string, the method returns a string that equals to the source string.
    /// Note that return string may be not the same reference as the input string.</para>
    /// <para>If the strings specified <paramref name="start"/> and <paramref name="end"/> are found in the
    /// <paramref name="source"/> string, the method removes all characters starting from the <b>first</b> occurence
    /// of the <c>start</c> to the <b>last</b> occurence of the <c>end</c> and returns the result.</para>
    /// </returns>
    public static string RemoveBetween(this string source,
                                       string start,
                                       string end,
                                       StringComparison comparison = StringComparison.CurrentCulture)
    {
        start ??= string.Empty;
        end ??= string.Empty;

        if (start == string.Empty && end == string.Empty)
        {
            return source;
        }

        while (true)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            int startPos = source.IndexOf(start, comparison);
            if (startPos == -1)
            {
                return source;
            }

            int endPos = string.IsNullOrEmpty(end) ? source.Length : source.IndexOf(end, startPos, comparison);
            if (endPos == -1)
            {
                return source;
            }

            source = source.Remove(startPos, endPos + end.Length - startPos);
        }
    }

    /// <summary>
    /// Removes HTML tags (if any) from the string.
    /// </summary>
    /// <param name="source">The string to remove tags from.</param>
    /// <returns>A copy of the <paramref name="source"/> string with HTML tags removed.</returns>
    public static string RemoveHtmlTags(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        source = source.RemoveBetween("<!--", "-->");
        source = source.Replace("<", " <");
        source = source.RemoveBetween("<", ">");
        source = source.Replace("\n", " ");
        source = source.Replace("\r", string.Empty);

        while (source.Contains("  "))
        {
            source = source.Replace("  ", " ");
        }

        if (source.StartsWith(" "))
        {
            source = source[1..];
        }

        if (source.EndsWith(" "))
        {
            source = source[0..^1];
        }

        return source;
    }

    /// <summary>
    /// Converts <see langword="null" />, empty or whitespace string to <see langword="null" /> leaving other strings as is. 
    /// </summary>
    /// <param name="source">The string to test and convert.</param>
    /// <returns><see langword="null" /> if the string in <paramref name="source"/> is <see langword="null" />,
    /// empty or whitespace string, otherwise, the value of <paramref name="source"/>.</returns>
    public static string? ToNullIfWhiteSpace(this string? source)
    {
        return string.IsNullOrWhiteSpace(source) ? null : source;
    }

    /// <summary>
    /// Converts the string representation of a number to its 32-bit signed integer equivalent.
    /// </summary>
    /// <param name="source">A <see cref="string" /> containing a number to convert.</param>
    /// <param name="fallbackValue">A 32-bit signed <see cref="int"/> containing fallback value.</param>
    /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="source"/>.
    /// If the <paramref name="source"/> does not contain the string representation of a 32-bit signed number,
    /// the method returns <paramref name="fallbackValue"/>.</returns>
    /// <remarks>
    /// <para>The method doesn't throw any exception.</para>
    /// <para>Unlike the <see cref="int.Parse(string)"/> mehtod this method does not allow leading whitespaces
    /// in <paramref name="source"/>.</para>
    /// </remarks>
    public static int ToInt(this string? source, int fallbackValue = default)
    {
        return ToIntInternal(source, out int result) ? result : fallbackValue;
    }

    /// <summary>
    /// Converts the string representation of a number to its nullable 32-bit signed integer equivalent.
    /// </summary>
    /// <param name="source">A <see cref="String" /> containing a number to convert.</param>
    /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="source"/>.
    /// If the <paramref name="source"/> does not contain the string representation of a 32-bit signed number,
    /// the method returns <see langword="null"/>.</returns>
    /// <remarks>
    /// <para>The method doesn't throw any exception.</para>
    /// <para>Unlike the <see cref="Int32.Parse(String)"/> method this method does not allow leading whitespaces
    /// in <paramref name="source"/>.</para>
    /// </remarks>
    public static int? ToNullableInt(this string? source)
    {
        return ToIntInternal(source, out int result) ? (int?)result : null;
    }


    #region ToInt - Internal

    private static bool ToIntInternal(string? source, out int result)
    {
        result = 0;

        if (source == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(source)) // string is empty - return false
        {
            return false;
        }

        bool minus = false;
        int position = 0;

        if (source[0] == '-') // if first char is munus, assume negative value
        {
            if (source.Length == 1) // negative values must have at least one digit
            {
                return false;
            }

            minus = true;
            position++;
        }

        if (source[0] == '+') // if first char is plus, assume positive value
        {
            if (source.Length == 1) // positive values must have at least one digit
            {
                return false;
            }

            minus = false;
            position++;
        }

        int length = source.Length;
        char c;

        try
        {
            for (int i = position; i < length; ++i)
            {
                c = source[i];
                if (c < '0' || c > '9') // invalid character (not a number) - return default value
                {
                    return false;
                }

                //
                // throw an OverflowException if overflow
                //
                result = checked(result * 10);
                result = checked(result + ((int)c - '0'));
            }
        }
        catch (OverflowException) // overflow
        {
            return false;
        }

        if (minus)
        {
            result = -result;
        }

        return true;
    }

    #endregion


    /// <summary>
    /// Fallback culture for ToDecimal() and ToDouble().
    /// </summary>
    private static readonly CultureInfo _fallbackCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Converts the string representation of a number to its decimal equivalent.
    /// </summary>
    /// <param name="source">The string representation of the number to convert.</param>
    /// <param name="fallbackValue">A <see cref="Decimal"/> containing the fallback value.</param>
    /// <param name="useFallbackCulture"><see langword="true"/> (default) to use fallback culture
    /// if the value in <paramref name="source"/> cannot be converted using current culture.</param>
    /// <returns>The equivalent to the number contained in <paramref name="source"/> if <paramref name="source"/> was converted
    /// successfully, otherwise, the value of <paramref name="fallbackValue"/>.</returns>
    /// <remarks>
    /// <para>The method doesn't throw any exception.</para>
    /// </remarks>
    public static decimal ToDecimal(this string? source,
                                    decimal fallbackValue = default,
                                    bool useFallbackCulture = true)
    {
        return ToDecimalInternal(source, out decimal result, useFallbackCulture)
                    ? result
                    : fallbackValue;
    }

    /// <summary>
    /// Converts the string representation of a number to its nullable decimal equivalent.
    /// </summary>
    /// <param name="source">The string representation of the number to convert.</param>
    /// <param name="useFallbackCulture"><see langword="true"/> (default) to use fallback culture
    /// if the value in <paramref name="source"/> cannot be converted using current culture.</param>
    /// <returns>The equivalent to the number contained in <paramref name="source"/> if <paramref name="source"/> was converted
    /// successfully, otherwise, <see langword="null"/>.</returns>
    /// <remarks>
    /// <para>The method doesn't throw any exception.</para>
    /// </remarks>
    public static decimal? ToNullableDecimal(this string? source, bool useFallbackCulture = true)
    {
        return ToDecimalInternal(source, out decimal result, useFallbackCulture)
                    ? (decimal?)result
                    : null;
    }


    #region ToDecimalInternal

    private static bool ToDecimalInternal(string? source, out decimal result, bool useFallbackCulture)
    {
        result = 0.0M;

        if (source == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(source)) // string is empty - return default value
        {
            return false;
        }

        try
        {
            result = decimal.Parse(source, NumberStyles.Any, CultureInfo.CurrentCulture);
        }
        catch
        {
            if (!useFallbackCulture)
            {
                return false;
            }

            try
            {
                result = decimal.Parse(source, NumberStyles.Any, _fallbackCulture);
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    #endregion


    /// <summary>
    /// Converts the specified string representation of a date and time to its <see cref="DateTime"/> equivalent.
    /// </summary>
    /// <param name="source">A string containing a date and time to convert.</param>
    /// <returns>The <see cref="DateTime"/> value equivalent to the date and time contained in <paramref name="source"/>,
    /// if the conversion succeeded, or  <see langword="null"/> if the conversion failed.
    /// </returns>
    /// <remarks>
    /// <para>The method does not throw any exception.</para>
    /// <para>The conversion fails if the <paramref name="source"/> parameter is <see langword="null"/>,
    /// is an empty string (""), or does not contain a valid string
    /// representation of a date and time.</para>
    /// </remarks>
    public static DateTime? ToNullableDateTime(this string? source)
    {
        return DateTime.TryParse(source, out var result) ? (DateTime?)result : null;
    }

    /// <summary>
    /// Returns a copy of the source string with characters reversed.
    /// This method is faster than <see cref="Reverse(string)"/>
    /// but does not process Unicode surrogate pairs correctly.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The copy of the <paramref name="source"/> with characters reversed.</returns>
    /// <remarks>
    /// <para>Unicode surrogate pairs are <b>not</b> processed correctly.</para>
    /// <para>This methos should be used if sure that the string specified in <paramref name="source"/> does not contain
    /// Unicode surrogate pairs.</para>
    /// <para>Alternatively, the caller should normalize Unicode strings.
    /// See the <see cref="string.Normalize()"/> method.</para>
    /// </remarks>
    public static string FastReverse(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        int length = source.Length;
        int halfLength = source.Length / 2;
        char[] chars = source.ToCharArray();

        for (int i = 0; i < halfLength; i++)
        {
            int revIndex = length - i - 1;
            char temp = chars[i];
            chars[i] = chars[revIndex];
            chars[revIndex] = temp;
        }

        string result = new string(chars);
        return result;
    }

    /// <summary>
    /// Returns a copy of the source string with characters reversed.
    /// This method is slower than <see cref="FastReverse(string)"/>
    /// but processes Unicode surrogate pairs correctly.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns>The copy of the <paramref name="source"/> with characters reversed.</returns>
    /// <remarks>Unicode surrogate pairs are processed correctly.</remarks>
    public static string Reverse(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        return GraphemeClusters(source).Reverse().JoinWith(string.Empty);
    }


    //
    // http://stackoverflow.com/a/15111719
    //
    private static IEnumerable<string> GraphemeClusters(string s)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(s);
        while (enumerator.MoveNext())
        {
            yield return (string)enumerator.Current;
        }
    }

    /// <summary>
    /// Returns a common prefix of the specified string with another string.
    /// </summary>
    /// <param name="value">The specified string.</param>
    /// <param name="anotherValue">The anothe string.</param>
    /// <returns>
    /// <para>The string which is both strings specified in
    /// <paramref name="value"/> and <paramref name="anotherValue"/> start with.</para>
    /// <para>If the string specified in the <paramref name="value"/> is <see langword="null"/>,
    /// the method returns <see langword="null"/>.</para>
    /// <para>If the string specified in the <paramref name="anotherValue"/> is <see langword="null"/> or an empty string,
    /// or the strings do not own a common prefix, the method returns an empty string.</para>
    /// </returns>
    /// <remarks>The method ignores Unicode surrogate pairs.
    /// The caller should use normalized Unicode strings.
    /// See the <see cref="string.Normalize()"/> method.</remarks>
    /// <example>
    /// <code>
    /// string filePath = @"C:\Windows\System32\library.dll";
    /// string anotherPath = @"C:s\Windows\System32\anotherlibrary.dll";
    /// string commonPath = filePath.CommonPrefixWith(anotherPath); // returns "C:\Windows\System32\"
    /// </code>
    /// </example>
    public static string? CommonPrefixWith(this string? value, string? anotherValue)
    {
        if (value == null)
        {
            return null;
        }
        if (string.IsNullOrEmpty(anotherValue))
        {
            return string.Empty;
        }

        int min = Math.Min(value.Length, anotherValue.Length);
        var sb = new StringBuilder(min);
        for (int i = 0; i < min && value[i] == anotherValue[i]; i++)
        {
            sb.Append(value[i]);
        }

        return sb.ToString();
    }
}
