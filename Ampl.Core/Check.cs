using Ampl.Core.Resources;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a set of <see langword="static" /> parameter checking routines.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Informs FxCop that method parameter on which this attribute is applies is checked for null with custom method.
        /// </summary>
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
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "The default values assigned for optional parameters are always default values."
        )]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// 
        /// </summary>
        /// <param name="argumentValue"></param>
        /// <param name="argumentName"></param>
        /// <returns></returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "The default values assigned for optional parameters are always default values."
        )]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NotNullOrEmptyString(string argumentValue, string argumentName = null)
        {
            NotNull(argumentValue, argumentName);
            if (argumentValue.Length == 0)
            {
                throw argumentName == null
                        ? new ArgumentException(Messages.ValueCannotBeAnEmptyString)
                        : new ArgumentException(Messages.ValueCannotBeAnEmptyString, argumentName);
            }

            return argumentValue;
        }
    }
}
