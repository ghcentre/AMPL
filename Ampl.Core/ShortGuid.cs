using Ampl.Core.Resources;
using System;

namespace Ampl.Core
{
    /// <summary>
    /// Represents a globally unique identifier (GUID) whose string representation is BASE64 encoded.
    /// </summary>
    public class ShortGuid
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The initial <see cref="Guid"/> value.</param>
        public ShortGuid(Guid value)
        {
            Guid = value;
        }


        /// <summary>
        /// Gets the GUID value.
        /// </summary>
        /// <value>The GUID value.</value>
        public Guid Guid { get; }


        /// <summary>
        /// Returns a string representation of the value of this instance.
        /// </summary>
        /// <returns>The value of this <see cref="ShortGuid"/> represented as a series of 22 BASE64-encoded characters.</returns>
        public override string ToString()
        {
            byte[] bytes = Guid.ToByteArray();
            //
            // "+" => "-"
            // "/" => "_"
            // (RFC 3548, par. 4).
            // (RFC 1575, appendix C)
            //
            // remove trailing "==" as base-64 encoded GUID always ends with "=="
            //
            string result = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", string.Empty);

            return result;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="ShortGuid"/> to the equivalent <see cref="ShortGuid"/>.
        /// </summary>
        /// <param name="shortGuid">The string to convert</param>
        /// <returns>An object that contains the value that was parsed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="shortGuid"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException"><paramref name="shortGuid"/>is not in a recognized format.</exception>
        public static ShortGuid Parse(string shortGuid)
        {
            Check.NotNullOrEmptyString(shortGuid, nameof(shortGuid));
            if (shortGuid.Length != 22)
            {
                throw new FormatException(Messages.InputStringWasNotInACorrectFormat);
            }

            string value = shortGuid.Replace("-", "+").Replace("_", "/") + "==";
            byte[] bytes = Convert.FromBase64String(value);
            var guid = new Guid(bytes);

            return new ShortGuid(guid);
        }

        /// <summary>
        /// Inializes a new instance of the class with a new <see cref="Guid"/> value.
        /// </summary>
        /// <returns>A new instance of <see cref="ShortGuid"/>.</returns>
        public static ShortGuid NewShortGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }
    }
}
