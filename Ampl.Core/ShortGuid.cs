using Ampl.System.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.System
{
  public class ShortGuid
  {
    private readonly Guid _guid;

    public ShortGuid(Guid value)
    {
      _guid = value;
    }

    public Guid Guid => _guid;

    public override string ToString()
    {
      byte[] bytes = _guid.ToByteArray();
      //
      // "+" => "-"
      // "/" => "_"
      // (RFC 3548, par. 4).
      //
      // remove trailing "==" as base-64 encoded GUID always ends with "=="
      //
      string result = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", string.Empty);
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
  }
}
