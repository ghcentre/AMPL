using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ampl.Core;

/// <summary>
/// Represents a string literal with special characters escaped.
/// </summary>
/// <remarks>
/// <para>The following special characters are escaped with the backslash (\) character:</para>
/// <para>\0, \a, \b, \f, \n, \r, \t, \v, \", \\</para>
/// </remarks>
public class StringLiteral
{
    private const char _encloser = '"';
    private const char _escape = '\\';

    private static readonly IDictionary<char, string> _escapes =
        new Dictionary<char, string>()
        {
            { '\0', @"\0" },
            { '\a', @"\a" },
            { '\b', @"\b" },
            { '\f', @"\f" },
            { '\n', @"\n" },
            { '\r', @"\r" },
            { '\t', @"\t" },
            { '\v', @"\v" },
            { '\"', @"\""" },
            { '\\', @"\\" },
        };
    private static readonly IDictionary<char, char> _unescapes =
        _escapes.ToDictionary(x => x.Value[1], x => x.Key);

    /// <summary>
    /// Initializes a new instance of the <see cref="StringLiteral"/> class.
    /// </summary>
    /// <param name="source">The unescaped string.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="source"/> is <see langword="null"/>.</exception>
    public StringLiteral(string source)
    {
        Guard.Against.Null(source, nameof(source));
        Value = source;
    }

    /// <summary>
    /// Gets the original value that was used to initialize this instance.
    /// </summary>
    /// <value>The original string value.</value>
    public string Value { get; }


    /// <summary>
    /// Escapes the original string.
    /// </summary>
    /// <returns>The escaped string enclosed in quotes.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(Value.Length * 2);

        sb.Append(_encloser);

        foreach (char c in Value)
        {
            if (_escapes.TryGetValue(c, out var escape))
            {
                sb.Append(escape);
                continue;
            }

            sb.Append(c);
        }

        sb.Append(_encloser);

        string result = sb.ToString();
        return result;
    }

    /// <summary>
    /// Parses the escaped string enclosed in quotes.
    /// </summary>
    /// <param name="input">The string representing the escaped literal value.</param>
    /// <param name="literal">If the conversion succeeds, contains the resulting <see cref="StringLiteral"/>.
    /// Otherwise, the value is <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the conversion succeeds, or <see langword="false"/> otherwise.</returns>
    /// <remarks>The method tries to parse the string in <paramref name="input"/>.
    /// The string must be enclosed in double quotes.
    /// The rest of the string after ending double quote is ignored.</remarks>
    public static bool TryParse(string? input, out StringLiteral? literal)
    {
        literal = null;

        if (input == null || !input.StartsWith(_encloser) || input.Length <= 2)
        {
            return false;
        }

        int length = input.Length;
        var sb = new StringBuilder(length);
        int i;

        for (i = 1; i < length && input[i] != _encloser; i++)
        {
            if (input[i] != _escape)
            {
                sb.Append(input[i]);
                continue;
            }

            i++;

            if (i >= length - 1)
            {
                return false;
            }

            bool found = _unescapes.TryGetValue(input[i], out char unescaped);
            char c = found ? unescaped : input[i];
            sb.Append(c);
        }

        if (i == length)
        {
            return false;
        }

        string resultString = sb.ToString();
        literal = new StringLiteral(resultString);

        return true;
    }
}
