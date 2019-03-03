using Ampl.Configuration.Tests.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration.Tests.EF
{
  public class AppConfigConfiguration : IAppConfigConfiguration
  {
    private readonly AmplTestContext _db;

    public AppConfigConfiguration(AmplTestContext db)
    {
      _db = db;
    }

    public IEnumerable<IAppConfigConverter> GetConverters()
    {
      return new AppConfigDefaultConfiguration().GetConverters();
    }

    public string ResolveDefaultKey(string key)
    {
      return _db.AppConfigKeyResolvers.Find(key)?.ToKey;
    }
  }
}
