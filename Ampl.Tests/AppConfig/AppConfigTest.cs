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
    class Repository : Ampl.Configuration.IAppConfigRepository
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
    public void DefaultConfiguration_Clones()
    {
      int defaultConverterCount = Ampl.Configuration.AppConfig.DefaultConfiguration.GetConverters().Count();
      Ampl.Configuration.AppConfig.DefaultConfiguration.AddKeyResolver("from", "to");
      Assert.IsTrue("to" == Ampl.Configuration.AppConfig.DefaultConfiguration.GetKeyResolver("from"));

      var cfg = new Ampl.Configuration.AppConfig(new Repository());
      Assert.AreEqual(defaultConverterCount, cfg.Configuration.GetConverters().Count());
      Assert.IsTrue("to" == cfg.Configuration.GetKeyResolver("from"));

      cfg.Configuration.AddKeyResolver("from1", "to1");
      Assert.AreEqual(null, Ampl.Configuration.AppConfig.DefaultConfiguration.GetKeyResolver("from1"));
    }
  }
}
