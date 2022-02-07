using System;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a set of <see cref="IGuardClause"/> extension methods.
    /// </summary>
    /// <remarks>This is a rework of Ardalis GuardClauses</remarks>
    public static class GuardClauseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guard"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Null<T>(this IGuardClause guard,
                                T input,
                                string parameterName,
                                string message = null)
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
                throw new ArgumentNullException(message, (Exception)null);
            }

            //
            // (empty, empty)
            //
            throw new ArgumentNullException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guard"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string NullOrEmpty(this IGuardClause guard, string input, string parameterName, string message = null)
        {
            string actualMessage = message ?? Messages.ValueCannotBeAnEmptyString;

            Guard.Against.Null(input, parameterName, actualMessage);

            if (input.Length > 0)
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
        /// 
        /// </summary>
        /// <param name="guard"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string NullOrWhiteSpace(this IGuardClause guard, string input, string parameterName, string message = null)
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
}
