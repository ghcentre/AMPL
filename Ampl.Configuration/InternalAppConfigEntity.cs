using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  internal class InternalAppConfigEntity : IAppConfigEntity
  {
    public string Key { get; set; }

    public string Value {get; set; }
  }
}
