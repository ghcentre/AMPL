using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration.Tests
{
  [TestFixture]
  public class AppConfig_Tests
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

    [Test]
    public void AppConfig_DefaultConfiguration()
    {
      var config = new Ampl.Configuration.AppConfig(new Repository());
      Assert.IsTrue(config.Configuration.GetType() == typeof(Ampl.Configuration.AppConfigDefaultConfiguration));
    }

    [Test]
    public void AppConfig_DefaultConfiguration_ConvertersExist()
    {
      var defaultConfig = new Ampl.Configuration.AppConfigDefaultConfiguration();
      Assert.IsTrue(defaultConfig.GetConverters().Count() > 0);
    }

    [Test]
    public void AppConfig_DefaultConfiguration_Resolver_NonExistentKey()
    {
      var defaultConfig = new Ampl.Configuration.AppConfigDefaultConfiguration();
      Assert.IsNull(defaultConfig.ResolveDefaultKey("This.Is.NonExistent.Key"));
    }
  }
}

