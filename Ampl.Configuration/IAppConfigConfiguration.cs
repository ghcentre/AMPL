using System.Collections.Generic;

namespace Ampl.Configuration
{
    public interface IAppConfigConfiguration
  {
    IEnumerable<IAppConfigConverter> GetConverters();

    string ResolveDefaultKey(string key);
  }
}
