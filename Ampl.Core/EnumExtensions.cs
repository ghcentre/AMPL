using System;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a set of <see langword="static"/> extension methods to work with <c>enum</c>s.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="T">The type of the Enum.</typeparam>
        /// <param name="enumeration">Enumeration</param>
        /// <param name="source">String representation</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case,
        /// or <see langword="false"/> (default) otherwise.</param>
        /// <returns>An object of type <typeparamref name="T"/> whose value is represented by <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/> is either an empty string ("")
        /// or only contains white space, or is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="source"/> value is outside the range of the
        /// underlying type of <typeparamref name="T"/>.</exception>
        [Obsolete("Use Enums.Parse(). This method will be removed in next release.")]
        public static T ParseValue<T>(this T enumeration,
                                      string source,
                                      bool ignoreCase = false)
            where T : struct, Enum
        {
            return Enums.Parse<T>(source, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="T">The type of the Enum.</typeparam>
        /// <param name="enumeration">Enumeration</param>
        /// <param name="source">String representation</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case,
        /// or <see langword="false"/> (default) otherwise.</param>
        /// <returns>If the conversion succeedes, the method returns a <see cref="Nullable{T}"/> whose value
        /// is represented by <paramref name="source"/>.
        /// Otherwise, the method returns a a <see cref="Nullable{T}"/> whose value is <see langword="null"/>.</returns>
        /// <remarks>Unlike the <see cref="ParseValue{T}(T, string, bool)"/> this method does not throw an exception.</remarks>
        [Obsolete("Use Enums.ParseAsNullable(). This method will be removed in next release.")]
        public static T? ParseAsNullable<T>(this T enumeration,
                                            string source,
                                            bool ignoreCase = false)
            where T : struct, Enum
        {
            return Enums.ParseAsNullable<T>(source, ignoreCase);
        }
    }
}
