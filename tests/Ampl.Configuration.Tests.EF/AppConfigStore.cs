using Ampl.Configuration.Tests.EF.Entities;
using Ampl.System;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Ampl.Configuration.Tests.EF
{
    public class AppConfigStore : IAppConfigStore
    {
        private readonly AmplTestContext _context;

        public AppConfigStore(AmplTestContext context)
        {
            _context = Check.NotNull(context, nameof(context));
        }

        public IAppConfigTransaction BeginAppConfigTransaction()
        {
            return new Transaction(_context.Database.CurrentTransaction == null
                                     ? _context.Database.BeginTransaction()
                                     : null);
        }

        public IAppConfigEntity CreateAppConfigEntity()
        {
            return new Entities.AppConfigItem();
        }

        public bool DeleteAppConfigEntity(IAppConfigEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            var databaseEntity = _context.AppConfigItems.SingleOrDefault(x => x.Key == entity.Key);
            if(databaseEntity == null)
            {
                return false;
            }
            _context.AppConfigItems.Remove(databaseEntity);
            return true;
        }

        public IEnumerable<IAppConfigEntity> GetAppConfigEntities(string prefix)
        {
            Check.NotNull(prefix, nameof(prefix));
            return _context.AppConfigItems.Where(x => x.Key.StartsWith(prefix));
        }

        public IAppConfigEntity GetAppConfigEntity(string key)
        {
            Check.NotNull(key, nameof(key));
            return _context.AppConfigItems.SingleOrDefault(x => x.Key == key);
        }

        public void SaveAppConfigChanges()
        {
            _context.SaveChanges();
        }

        public void SetAppConfigEntity(IAppConfigEntity entity)
        {
            Check.NotNull(entity, nameof(entity));
            var databaseEntity = _context.AppConfigItems.SingleOrDefault(x => x.Key == entity.Key);
            if(databaseEntity == null)
            {
                databaseEntity = (AppConfigItem)CreateAppConfigEntity();
                databaseEntity.Key = entity.Key;
                databaseEntity.Value = entity.Value;
                _context.AppConfigItems.Add(databaseEntity);
            }
            else
            {
                databaseEntity.Value = entity.Value;
            }
        }

        public class Transaction : IAppConfigTransaction, IDisposable
        {
            private readonly DbContextTransaction _transaction;

            internal Transaction(DbContextTransaction transaction)
            {
                _transaction = transaction;
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

            private bool _alreadyDisposed = false;

            protected virtual void Dispose(bool disposing)
            {
                if(!_alreadyDisposed)
                {
                    if(disposing)
                    {
                        _transaction?.Dispose();
                    }
                    _alreadyDisposed = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            #endregion
        }
    }
}

