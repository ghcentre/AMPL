using System.Collections.Generic;

namespace Ampl.Configuration
{
    /// <summary>
    /// Represents a store for <see cref="IAppConfig"/>.
    /// </summary>
    public interface IAppConfigStore
    {
        /// <summary>
        /// Gets the <see cref="IAppConfigEntity"/> associated with the key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>The associated entity or <see langword="null"/> if there is no associated entity.</returns>
        IAppConfigEntity GetAppConfigEntity(string key);

        /// <summary>
        /// Gets the sequence of the <see cref="IAppConfigEntity"/> associated with the key prefix.
        /// </summary>
        /// <param name="prefix">The key prefix.</param>
        /// <returns>The sequence of the associated entities.</returns>
        IEnumerable<IAppConfigEntity> GetAppConfigEntities(string prefix);

        /// <summary>
        /// Initializes a new instance of the <see cref="IAppConfigEntity"/>.
        /// </summary>
        /// <returns>The new instance of the <see cref="IAppConfigEntity"/>.</returns>
        IAppConfigEntity CreateAppConfigEntity();

        /// <summary>
        /// Sets (writes) the entity.
        /// </summary>
        /// <param name="entity">The entity to set.</param>
        void SetAppConfigEntity(IAppConfigEntity entity);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns><see langword="true"/> if the entity existed in the store,
        /// or <see langword="false"/> otherwise.</returns>
        bool DeleteAppConfigEntity(IAppConfigEntity entity);

        /// <summary>
        /// Begins a transaction against the config store.
        /// </summary>
        /// <returns>The <see cref="IAppConfigTransaction"/>.</returns>
        IAppConfigTransaction BeginAppConfigTransaction();

        /// <summary>
        /// Applies changes made to the config store.
        /// </summary>
        void SaveAppConfigChanges();
    }
}
