using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a set of <see langword="static" /> parameter checking routines.
    /// </summary>
    /// <remarks>
    /// This class is <b>obsolete</b> and generates a error if referenced.
    /// </remarks>
    [Obsolete("Use Guard Clauses", true)]
    public static class Check
    {
        /// <summary>
        /// Informs FxCop that method parameter on which this attribute is applies is checked for null with custom method.
        /// </summary>
        [AttributeUsage(AttributeTargets.All)]
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }


        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given value is null.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="argumentValue"/>.</typeparam>
        /// <param name="argumentValue">The value to check for null.</param>
        /// <param name="argumentName">The argument name.</param>
        /// <returns>The method returns the reference passed in <paramref name="argumentValue"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The reference passed in <paramref name="argumentValue"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <para>The folowing code demonstrates argument null checking:</para>
        /// <code>
        /// private void SomeMethod(SomeObject obj)
        /// {
        ///   Check.NotNull(obj, "obj"); // throws ArgumentNullException if obj == null
        /// }
        /// </code>
        /// <para>The following code demonstrates argument null checking combined with assignment:</para>
        /// <code>
        /// internal class TheClass
        /// {
        ///   private SomeType _theType;
        ///   // ...
        ///   public TheClass(SomeType theType)
        ///   {
        ///     //
        ///     // throws ArgumentNullException if theType == null
        ///     //   else assigns theType to _theType
        ///     //
        ///     _theType = Check.NotNull(theType, "theType");
        ///   }
        /// }
        /// </code>
        /// </example>
        [SuppressMessage(
            "Microsoft.Usage",
            "CA2208:InstantiateArgumentExceptionsCorrectly",
            Justification = "ArgumentNullException parameterless constructor called explicitly to indicate than no parameter name is given."
        )]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("Use Guard.Against.Null() or discard assignment with null coalesce operator and throw expression.")]
        public static T NotNull<T>([ValidatedNotNull] T argumentValue, string argumentName = null)
        {
            if (argumentValue == null)
            {
                throw argumentName == null
                        ? new ArgumentNullException()
                        : new ArgumentNullException(argumentName);
            }

            return argumentValue;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the given value is null or an empty <see cref="System.String"/>.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The optional argument name.</param>
        /// <returns>The method return the value passed to the <paramref name="argumentName"/> parameter if the value is
        /// not <see langword="null"/> or an empty string.
        /// Otherwise, the method throws an <see cref="ArgumentException"/>.</returns>
        /// <exception cref="ArgumentException">
        /// The value passed to the <paramref name="argumentValue"/> is null or an empty string.
        /// </exception>
        [Obsolete("Use Guard.Against.NullOrEmpty().")]
        public static string NotNullOrEmptyString(string argumentValue, string argumentName = null)
        {
            //
            // throw ArgumentNullException (not ArgumentException) if argument is null;
            // throw ArgumentException otherwise
            //
            _ = argumentValue ?? throw new ArgumentNullException(nameof(argumentName));

            if (argumentValue.Length == 0)
            {
                throw argumentName == null
                        ? new ArgumentException(Messages.ValueCannotBeAnEmptyString)
                        : new ArgumentException(Messages.ValueCannotBeAnEmptyString, argumentName);
            }

            return argumentValue;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the given value is null or an empty <see cref="System.String"/>
        /// or consists only of white-space characters.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The optional argument name.</param>
        /// <returns>The method return the value passed to the <paramref name="argumentName"/> parameter if the value is
        /// not <see langword="null"/> or an empty string or a white-space string.
        /// Otherwise, the method throws an <see cref="ArgumentException"/>.</returns>
        /// <exception cref="ArgumentException">
        /// The value passed to the <paramref name="argumentValue"/> is either <see langword="null" />,
        /// or an empty string,
        /// or consists only of white-space characters.
        /// </exception>
        [Obsolete("Use Guard.Against.NullOrWhiteSpace().")]
        public static string NotNullOrWhiteSpaceString(string argumentValue, string argumentName = null)
        {
            //
            // throw ArgumentNullException (not ArgumentException) if argument is null;
            // throw ArgumentException otherwise
            //
            _ = argumentValue ?? throw new ArgumentNullException(nameof(argumentName));

            NotNullOrEmptyString(argumentValue, argumentName);

            for (int i = 0; i < argumentValue.Length; i++)
            {
                if (!char.IsWhiteSpace(argumentValue[i]))
                {
                    return argumentValue;
                }
            }

            throw argumentName == null
                    ? new ArgumentException(Messages.ValueCannotBeAWhiteSpaceString)
                    : new ArgumentException(Messages.ValueCannotBeAWhiteSpaceString, argumentName);
        }
    }
}