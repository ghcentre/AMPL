using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Ampl.Core
{
    /// <summary>
    /// <para>Specifies various options for the <see cref="StringExtensions.Between"/> method.</para>
    /// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination
    /// of its member values.</para>
    /// </summary>
    /// <seealso cref="StringExtensions.Between"/>
    /// <remarks>
    /// <para>The <see cref="StringExtensions.Between"/> default behavior is:</para>
    /// <list type="bullet">
    ///   <item>If at least one substring specified by the <b>start</b> and <b>end</b> parameters is not found in the
    ///   <b>source</b> string, the method return the empty string.</item>
    ///   <item>The substrings specified by the <b>start</b> and <b>end</b> parameters are not included in the result substring.</item>
    /// </list>
    /// </remarks>
    [Flags]
    public enum StringBetweenOptions
    {
        /// <summary>
        /// The <see cref="StringExtensions.Between"/> method uses it's default behavior.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The <see cref="StringExtensions.Between"/> method returns the copy of the <b>source</b> string if the
        /// source string does not contain all of the substrings specified in <b>start</b> and <b>end</b> parameters.
        /// </summary>
        FallbackToSource = 0x01,

        /// <summary>
        /// The return value of the <see cref="StringExtensions.Between"/> method includes the value of the <b>start</b> substring.
        /// </summary>
        IncludeStart = 0x02,

        /// <summary>
        /// The return value of the <see cref="StringExtensions.Between"/> method includes the value of the <b>end</b> substring.
        /// </summary>
        IncludeEnd = 0x04,

        /// <summary>
        /// The return value of the <see cref="StringExtensions.Between"/> method includes both the values
        /// of the <b>start</b> and <b>end</b> substrings.
        /// </summary>
        IncludeStartEnd = 0x06,
    }

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
        /// If the parameter is <see langword="null"/> or empty string value,
        /// the substring is retrieved from the start of the source string.</param>
        /// <param name="end">The substring in the source string that is used as the end of resulting substring.
        /// If the parameter is <see langword="null"/> or empty string value,
        /// the substring is retrieved until the end of the source string.</param>
        /// <param name="options">The <see cref="StringBetweenOptions"/> enumerations that specifies various options.
        /// The default value is <see cref="StringBetweenOptions.None"/>.</param>
        /// <param name="comparison">The <see cref="StringComparison"/> enumerations that specifies string comparison
        /// options. The default value is <see cref="StringComparison.CurrentCulture"/>.</param>
        /// <returns>
        /// <para>A string that is equivalent to substring that begins at position of the <b>start</b> substring and
        /// ends at position of the <b>end</b> substring.</para>
        /// <para><em>-or-</em></para>
        /// <para>the empty string.</para>
        /// <para><i>See the remarks section.</i></para>
        /// </returns>
        /// <remarks>
        /// <para>This method returns the substring of the string specified in <b>source</b>. The start and end positions
        /// of the substring are the positions of the <b>start</b> and <b>end</b> substrings in the source string.</para>
        /// <para>If <i>one</i> of the <b>start</b> and <b>end</b> substrings is <i>not found</i> in the source string,
        /// the return value depends on the <see cref="StringBetweenOptions.FallbackToSource"/> flag:
        /// <list type="table">
        ///   <listheader>
        ///     <term>FallbackToSource</term>
        ///     <description>Return Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>Not Set</term>
        ///     <description>An empty string.</description>
        ///   </item>
        ///   <item>
        ///     <term>Set</term>
        ///     <description>A copy of the string specified in <b>source</b>.</description>
        ///   </item>
        /// </list>
        /// </para>
        /// <para>
        /// See the <see cref="StringBetweenOptions"/> enumeration for information about other options.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following code demonstrates usage of the <b>Between</b> method:
        /// <code>
        /// public void Method1()
        /// {
        ///   string source = "This is a test string.";
        ///   string s1 = source.Between(null, "test"); // returns "This is a"
        ///   string s2 = source.Between("test", null); // returns " string."
        ///   string s3 = source.Between("One", null); // returns an empty string.
        ///   string s4 = source.Between("One", null, StringBetweenOptions.FallbackToSource); // returns a copy of source
        ///   string s5 = source.Between("This", "test", StringBetweenOptions.IncludeStart); // returns "This is a "
        ///   string s6 = source.Between("thIS", "StR", StringBetweenOptions.None, StringComparison.CurrentCultureIgnoreCase; // return "is a test "
        /// }
        /// </code>
        /// </example>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "The default values assigned for optional parameters are always default values."
        )]
        public static string Between(this string source,
                                     string start,
                                     string end,
                                     StringBetweenOptions options = StringBetweenOptions.None,
                                     StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (source == null)
            {
                return null;
            }

            start = start ?? string.Empty;
            end = end ?? string.Empty;

            if (start.Length == 0 && end.Length == 0)
            {
                return source;
            }

            int startPosition = 0;

            if (start.Length > 0)
            {
                startPosition = source.IndexOf(start, comparison);
                if (startPosition == -1)
                {
                    if ((options & StringBetweenOptions.FallbackToSource) != StringBetweenOptions.FallbackToSource)
                    {
                        return string.Empty;
                    }

                    startPosition = 0;
                }
                else
                {
                    if ((options & StringBetweenOptions.IncludeStart) != StringBetweenOptions.IncludeStart)
                    {
                        startPosition += start.Length;
                    }
                }
            }

            int endPosition = source.Length;

            if (end.Length > 0)
            {
                endPosition = source.IndexOf(end, startPosition, comparison);
                if (endPosition == -1)
                {
                    if ((options & StringBetweenOptions.FallbackToSource) != StringBetweenOptions.FallbackToSource)
                    {
                        return string.Empty;
                    }

                    endPosition = source.Length;
                }
                else
                {
                    if ((options & StringBetweenOptions.IncludeEnd) == StringBetweenOptions.IncludeEnd)
                    {
                        endPosition += end.Length;
                    }
                }
            }

            int copyLength = endPosition - startPosition;
            string substring = source.Substring(startPosition, copyLength);

            return substring;
        }

        /// <summary>
        /// TODO: RemoveBetween
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string RemoveBetween(this string source,
                                           string start,
                                           string end,
                                           StringComparison comparison = StringComparison.CurrentCulture)
        {
            start = start ?? string.Empty;
            end = end ?? string.Empty;
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
                source = source.Substring(1);
            }

            if (source.EndsWith(" "))
            {
                source = source.Substring(0, source.Length - 1);
            }

            return source;
        }

        /// <summary>
        /// Converts <see langword="null" />, empty or whitespace string to <see langword="null" /> leaving other strings as is. 
        /// </summary>
        /// <param name="source">The string to test and convert.</param>
        /// <returns><see langword="null" /> if the string in <paramref name="source"/> is <see langword="null" />,
        /// empty or whitespace string, otherwise, the value of <paramref name="source"/>.</returns>
        public static string ToNullIfWhiteSpace(this string source)
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
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1720:IdentifiersShouldNotContainTypeNames",
            MessageId = "int",
            Justification = "The type name is important in the identifier."
        )]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "The default values assigned for optional parameters are always default values."
        )]
        public static int ToInt(this string source, int fallbackValue = 0)
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
        /// <para>Unlike the <see cref="Int32.Parse(String)"/> mehtod this method does not allow leading whitespaces
        /// in <paramref name="source"/>.</para>
        /// </remarks>
        public static int? ToNullableInt(this string source)
        {
            return ToIntInternal(source, out int result) ? (int?)result : null;
        }


        #region ToInt - Internal

        private static bool ToIntInternal(string source, out int result)
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
        private static readonly CultureInfo _fallbackCulture = new CultureInfo("en-US");

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
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1720:IdentifiersShouldNotContainTypeNames",
            MessageId = "decimal",
            Justification = "The type name is important in the identifier."
        )]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "The default values assigned for optional parameters are always default values."
        )]
        public static decimal ToDecimal(this string source,
                                        decimal fallbackValue = 0.0M,
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
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "The default values assigned for optional parameters are always default values."
        )]
        public static decimal? ToNullableDecimal(this string source, bool useFallbackCulture = true)
        {
            return ToDecimalInternal(source, out decimal result, useFallbackCulture)
                        ? (decimal?)result
                        : null;
        }


        #region ToDecimalInternal

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Catch block catches conversion exceptions only."
        )]
        private static bool ToDecimalInternal(string source, out decimal result, bool useFallbackCulture)
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
        public static DateTime? ToNullableDateTime(this string source)
        {
            return DateTime.TryParse(source, out var result) ? (DateTime?)result : null;
        }


        /// <summary>
        /// Returns a copy of the source string with characters reversed.
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
    }
}
