using System;

namespace Ampl.Core
{
    /// <summary>
    /// Represents a globally unique identifier (GUID) whose string representation is BASE64 encoded.
    /// </summary>
    [Obsolete("Use GuidExtensions.ToCompactString() and CompactGuid.Parse()", true)]
    public class ShortGuid
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The initial <see cref="Guid"/> value.</param>
        public ShortGuid(Guid value) => throw new NotSupportedException();


        /// <summary>
        /// Gets the GUID value.
        /// </summary>
        /// <value>The GUID value.</value>
        public Guid Guid => throw new NotSupportedException();


        /// <summary>
        /// Returns a string representation of the value of this instance.
        /// </summary>
        /// <returns>The value of this <see cref="ShortGuid"/> represented as a series of 22 BASE64-encoded characters.</returns>
        public override string ToString() => throw new NotSupportedException();

        /// <summary>
        /// Converts the string representation of a <see cref="ShortGuid"/> to the equivalent <see cref="ShortGuid"/>.
        /// </summary>
        /// <param name="shortGuid">The string to convert</param>
        /// <returns>An object that contains the value that was parsed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="shortGuid"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException"><paramref name="shortGuid"/>is not in a recognized format.</exception>
        public static ShortGuid Parse(string shortGuid) => throw new NotSupportedException();

        /// <summary>
        /// Inializes a new instance of the class with a new <see cref="Guid"/> value.
        /// </summary>
        /// <returns>A new instance of <see cref="ShortGuid"/>.</returns>
        public static ShortGuid NewShortGuid() => throw new NotSupportedException();
    }
}
