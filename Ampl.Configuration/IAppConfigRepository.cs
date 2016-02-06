using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  public interface IAppConfigRepository
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
