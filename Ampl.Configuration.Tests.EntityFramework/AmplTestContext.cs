using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

namespace Ampl.Configuration.Tests.EntityFramework
{
  public class AmplTestContext : DbContext, IAppConfigRepository
  {
    public AmplTestContext() : base("AmplTest")
    {
    }

    public virtual DbSet<AppConfigItem> AppConfigItems { get; set; }

    public IAppConfigTransaction BeginAppConfigTransaction()
    {
      return new AppConfigTransaction(this);
    }

    public IAppConfigEntity CreateAppConfigEntity()
    {
      return new AppConfigItem() {
        AnotherField = new DateTime(1972, 12, 7, 17, 25, 0)  // set another field just to test it remains unchanged
      };
    }

    public bool DeleteAppConfigEntity(IAppConfigEntity entity)
    {
      Check.NotNull(entity, nameof(entity));
      var databaseEntity = AppConfigItems.SingleOrDefault(x => x.Key == entity.Key);
      if(databaseEntity == null)
      {
        return false;
      }
      AppConfigItems.Remove(databaseEntity);
      return true;
    }

    public IEnumerable<IAppConfigEntity> GetAppConfigEntities(string prefix)
    {
      Check.NotNull(prefix, nameof(prefix));
      return AppConfigItems.Where(x => x.Key.StartsWith(prefix));
    }

    public IAppConfigEntity GetAppConfigEntity(string key)
    {
      Check.NotNull(key, nameof(key));
      return AppConfigItems.Where(x => x.Key == key).SingleOrDefault();
    }

    public void SaveAppConfigChanges()
    {
      SaveChanges();
    }

    public void SetAppConfigEntity(IAppConfigEntity entity)
    {
      Check.NotNull(entity, nameof(entity));
      var databaseEntity = AppConfigItems.Where(x => x.Key == entity.Key).SingleOrDefault();
      if(databaseEntity == null)
      {
        databaseEntity = (AppConfigItem)CreateAppConfigEntity();
        databaseEntity.Key = entity.Key;
        databaseEntity.Value = entity.Value;
        AppConfigItems.Add(databaseEntity);
      }
      else
      {
        databaseEntity.Value = entity.Value;
      }
    }
  }
}
