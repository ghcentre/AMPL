using System;
using System.Runtime.CompilerServices;

namespace Ampl.Core;

/// <summary>
/// Provides a set of <see cref="IGuardClause"/> extension methods.
/// </summary>
/// <remarks>This is a rework of Ardalis GuardClauses</remarks>
public static class GuardClauseExtensions
{
    /// <summary>
    /// Throw a <see cref="ArgumentNullException"/> if <paramref name="input"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="guard">The guard clause.</param>
    /// <param name="input">The input to check for <see langword="null"/>.</param>
    /// <param name="parameterName">The parameter name.</param>
    /// <param name="message">The message, or <see langword="null"/> to use default message.</param>
    /// <returns>The value specified in <paramref name="input"/> if the input is not <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">The input is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Null<T>(this IGuardClause guard,
                            T? input,
                            string? parameterName,
                            string? message = null)
    {
        if (!(input is null))
        {
            return input;
        }

        //
        // (nonempty, nonempty)
        //
        if (!string.IsNullOrEmpty(parameterName) && !string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(parameterName, message);
        }

        //
        // one of, or both, are empty
        //

        //
        // (nonempty, empty)
        //
        if (!string.IsNullOrEmpty(parameterName))
        {
            throw new ArgumentNullException(parameterName);
        }

        //
        // (empty, nonempty)
        //
        if (!string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(message, (Exception)null!);
        }

        //
        // (empty, empty)
        //
        throw new ArgumentNullException();
    }

    /// <summary>
    /// Throws an exception if the <paramref name="input"/> is <see langword="null"/> or an empty string ("").
    /// </summary>
    /// <param name="guard">The guard clause</param>
    /// <param name="input">The input string to check.</param>
    /// <param name="parameterName">The parameter name.</param>
    /// <param name="message">The message, or <see langword="null"/> to use a default message.</param>
    /// <returns>If the <paramref name="input"/> is not <see langword="null"/> or an empty string, the method returns
    /// the value of the input.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="input"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The <paramref name="input"/> is an empty string.</exception>
    public static string NullOrEmpty(this IGuardClause guard,
                                     string? input,
                                     string? parameterName,
                                     string? message = null)
    {
        string actualMessage = message ?? Messages.ValueCannotBeAnEmptyString;

        Guard.Against.Null(input, parameterName, actualMessage);

        if (input!.Length > 0)
        {
            return input;
        }

        if (string.IsNullOrEmpty(parameterName))
        {
            throw new ArgumentException(actualMessage);
        }

        throw new ArgumentException(actualMessage, parameterName);
    }

    /// <summary>
    /// Throws an exception if the <paramref name="input"/> is <see langword="null"/>, an empty, or a white space string.
    /// </summary>
    /// <param name="guard">The guard clause.</param>
    /// <param name="input">The input string to check.</param>
    /// <param name="parameterName">The parameter name.</param>
    /// <param name="message">The message, or <see langword="null"/> to use a default message.</param>
    /// <returns>If the <paramref name="input"/> is not <see langword="null"/>, an empty, or a white space string,
    /// the method returns a value of the input.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="input"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The <paramref name="input"/> is an empty, or white space string.</exception>
    public static string NullOrWhiteSpace(this IGuardClause guard,
                                          string? input,
                                          string? parameterName,
                                          string? message = null)
    {
        string actualMessage = message ?? Messages.ValueCannotBeAWhiteSpaceString;

        Guard.Against.Null(input, parameterName, actualMessage);

        if (!string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        if (string.IsNullOrEmpty(parameterName))
        {
            throw new ArgumentException(actualMessage);
        }

        throw new ArgumentException(actualMessage, parameterName);
    }
}
