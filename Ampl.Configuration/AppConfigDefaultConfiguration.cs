using Ampl.System;
using System.Collections.Generic;

namespace Ampl.Configuration
{
  public class AppConfigDefaultConfiguration : IAppConfigConfiguration
  {
    private List<IAppConfigConverter> _converters = new List<IAppConfigConverter>();

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

    internal void AddConverter(IAppConfigConverter converter)
    {
      _converters.Add(Check.NotNull(converter, nameof(converter)));
    }

    public IEnumerable<IAppConfigConverter> GetConverters()
    {
      return _converters.ToArray();
    }

    public string ResolveDefaultKey(string key)
    {
      return null; // no key resolvers by default
    }
  }
}
