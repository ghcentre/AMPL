using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  public interface IAppConfigConfiguration
  {
    void AddConverter(IAppConfigConverter converter);

    IEnumerable<IAppConfigConverter> GetConverters();

    void AddValueResolver(string keyResolveFrom, string keyResolveTo);

    string GetValueResolver(string key);
  }
}
