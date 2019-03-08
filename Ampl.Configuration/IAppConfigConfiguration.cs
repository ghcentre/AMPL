using System.Collections.Generic;

namespace Ampl.Configuration
{
    /// <summary>
    /// Represents a configuration for <see cref="IAppConfig"/>.
    /// </summary>
    public interface IAppConfigConfiguration
    {
        /// <summary>
        /// Gets a sequence of converters.
        /// </summary>
        /// <returns>The sequence of converters, each of <see cref="IAppConfigConverter"/> type.</returns>
        IEnumerable<IAppConfigConverter> GetConverters();

        /// <summary>
        /// Resolves a key that acts as a default/fallback key if the <see cref="IAppConfig.Get{T}(string, bool)"/> method
        /// returns <see langword="null"/> for the key.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The default key.</returns>
        string ResolveDefaultKey(string key);
    }
}
