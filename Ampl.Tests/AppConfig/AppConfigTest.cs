using System;
using System.Collections.Generic;
using System.Linq;
using Ampl.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests.AppConfig
{
  [TestClass]
  public class AppConfigTest
  {
    class Repository : Ampl.Configuration.IAppConfigStore
    {
      public IAppConfigTransaction BeginAppConfigTransaction()
      {
        throw new NotImplementedException();
      }

      public IAppConfigEntity CreateAppConfigEntity()
      {
        throw new NotImplementedException();
      }

      public bool DeleteAppConfigEntity(IAppConfigEntity entity)
      {
        throw new NotImplementedException();
      }

      public IEnumerable<IAppConfigEntity> GetAppConfigEntities(string prefix)
      {
        throw new NotImplementedException();
      }

      public IAppConfigEntity GetAppConfigEntity(string key)
      {
        throw new NotImplementedException();
      }

      public void SaveAppConfigChanges()
      {
        throw new NotImplementedException();
      }

      public void SetAppConfigEntity(IAppConfigEntity entity)
      {
        throw new NotImplementedException();
      }
    }

    [TestMethod]
    public void AppConfig_DefaultConfiguration()
    {
      var config = new Ampl.Configuration.AppConfig(new Repository());
      Assert.IsTrue(config.Configuration.GetType() == typeof(Ampl.Configuration.AppConfigDefaultConfiguration));
    }

    [TestMethod]
    public void AppConfig_DefaultConfiguration_ConvertersExist()
    {
      var defaultConfig = new Ampl.Configuration.AppConfigDefaultConfiguration();
      Assert.IsTrue(defaultConfig.GetConverters().Count() > 0);
    }

    [TestMethod]
    public void AppConfig_DefaultConfiguration_Resolver_NonExistentKey()
    {
      var defaultConfig = new Ampl.Configuration.AppConfigDefaultConfiguration();
      Assert.IsNull(defaultConfig.ResolveDefaultKey("This.Is.NonExistent.Key"));
    }
  }
}
