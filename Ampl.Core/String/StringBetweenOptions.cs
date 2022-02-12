using System;

namespace Ampl.Core;

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

