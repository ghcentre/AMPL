using System.Collections.Generic;

namespace Ampl.Configuration
{
    public interface IAppConfigStore
  {
    IAppConfigEntity GetAppConfigEntity(string key);

    IEnumerable<IAppConfigEntity> GetAppConfigEntities(string prefix);

    IAppConfigEntity CreateAppConfigEntity();

    void SetAppConfigEntity(IAppConfigEntity entity);

    bool DeleteAppConfigEntity(IAppConfigEntity entity);

    IAppConfigTransaction BeginAppConfigTransaction();

    void SaveAppConfigChanges();
  }
}
