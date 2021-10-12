using System;

namespace Ampl.Core
{
    /// <summary>
    /// Contains a set of <see langword="static"/> methots that converts the <see cref="Guid"/> compact string representation
    /// to the Guid instance.
    /// </summary>
    public static class CompactGuid
    {
        private const int _shortGuidLength = 22;
        private const string _terminator = "==";

        /// <summary>
        /// Converts the compact string representation of a GUID to the equivalent <see cref="Guid"/> structure.
        /// </summary>
        /// <param name="stringValue">The string to convert.</param>
        /// <returns>A structure that contains the value that was parsed.</returns>
        /// <exception cref="ArgumentNullException">Input is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Input is not in a recognized format.</exception>
        public static Guid Parse(string stringValue)
        {
            _ = stringValue ?? throw new ArgumentNullException(nameof(stringValue));

            if (stringValue.Length != _shortGuidLength)
            {
                throw new FormatException(Messages.InputStringWasNotInACorrectFormat);
            }

            string value = stringValue.Replace("-", "+").Replace("_", "/") + _terminator;

            byte[] bytes = Convert.FromBase64String(value);
            var guid = new Guid(bytes);

            return guid;
        }
    }
}
