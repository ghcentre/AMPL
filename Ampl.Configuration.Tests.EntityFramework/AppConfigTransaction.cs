using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

namespace Ampl.Configuration.Tests.EntityFramework
{
  internal class AppConfigTransaction : IAppConfigTransaction
  {
    private DbContextTransaction _transaction;

    public AppConfigTransaction(AmplTestContext context)
    {
      Check.NotNull(context, nameof(context));
      if(context.Database.CurrentTransaction != null)
      {
        return;
      }
      _transaction = context.Database.BeginTransaction();
    }

    public void Commit()
    {
      _transaction?.Commit();
    }

    public void Rollback()
    {
      _transaction?.Rollback();
    }

    #region IDisposable Support

    private bool _alreadyDisposed = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if(!_alreadyDisposed)
      {
        if(disposing)
        {
          _transaction?.Dispose();
        }

        // free unmanaged resources (unmanaged objects) and override a finalizer below.
        _transaction = null;

        _alreadyDisposed = true;
      }
    }

    // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~AppConfigTransaction() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }

    #endregion
  }
}
