using System;

namespace Ampl.Configuration
{
    /// <summary>
    /// Provides methods for committing or rolling back transactions aganinst the <see cref="IAppConfigStore"/>.
    /// </summary>
    public interface IAppConfigTransaction : IDisposable
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        void Rollback();
    }
}
