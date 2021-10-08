using System;

namespace Ampl.Core
{
    public static class CompactGuid
    {
        private const int _shortGuidLength = 22;
        private const string _terminator = "==";

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
