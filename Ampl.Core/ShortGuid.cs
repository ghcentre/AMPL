using Ampl.Core.Resources;
using System;

namespace Ampl.Core
{
    public class ShortGuid
    {
        public ShortGuid(Guid value)
        {
            Guid = value;
        }

        public Guid Guid { get; }

        public override string ToString()
        {
            byte[] bytes = Guid.ToByteArray();
            //
            // "+" => "-"
            // "/" => "_"
            // (RFC 3548, par. 4).
            //
            // remove trailing "==" as base-64 encoded GUID always ends with "=="
            //
            string result = Convert.ToBase64String(bytes).Replace("+", "-")
                                                         .Replace("/", "_")
                                                         .Replace("=", string.Empty);
            return result;
        }

        public static ShortGuid Parse(string shortGuid)
        {
            Check.NotNullOrEmptyString(shortGuid, nameof(shortGuid));
            if(shortGuid.Length != 22)
            {
                throw new FormatException(Messages.InputStringWasNotInACorrectFormat);
            }
            string value = shortGuid.Replace("-", "+").Replace("_", "/") + "==";
            byte[] bytes = Convert.FromBase64String(value);
            var guid = new Guid(bytes);
            return new ShortGuid(guid);
        }

        public static ShortGuid NewShortGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }
    }
}
