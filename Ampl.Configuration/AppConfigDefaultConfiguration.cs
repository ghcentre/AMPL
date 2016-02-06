using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

namespace Ampl.Configuration
{
  class AppConfigDefaultConfiguration : IAppConfigConfiguration
  {
    private List<IAppConfigConverter> _converters = new List<IAppConfigConverter>();
    private Dictionary<string, string> _resolvers = new Dictionary<string, string>();

    public AppConfigDefaultConfiguration()
    {
      AddConverter(new StringConverter());
      AddConverter(new IntConverter());
      AddConverter(new NullableIntConverter());
      AddConverter(new BoolConverter());
      AddConverter(new NullableBoolConverter());
      AddConverter(new DecimalConverter());
      AddConverter(new NullableDecimalConverter());
    }

    public void AddConverter(IAppConfigConverter converter)
    {
      _converters.Add(Check.NotNull(converter, nameof(converter)));
    }

    public IEnumerable<IAppConfigConverter> GetConverters()
    {
      return _converters.ToArray();
    }

    public void AddValueResolver(string keyResolveFrom, string keyResolveTo)
    {
      _resolvers[keyResolveFrom] = keyResolveTo;
    }

    public string GetValueResolver(string key)
    {
      return _resolvers.GetValueOrDefault(key);
    }
  }
}
