using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  public interface IAppConfigConfiguration : ICloneable
  {
    void AddConverter(IAppConfigConverter converter);

    IEnumerable<IAppConfigConverter> GetConverters();

    void AddKeyResolver(string from, string to);

    string GetKeyResolver(string key);
  }
}
