using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  public interface IAppConfigConfiguration
  {
    IEnumerable<IAppConfigConverter> GetConverters();

    string ResolveDefaultKey(string key);
  }
}
