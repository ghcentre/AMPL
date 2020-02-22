namespace Ampl.Configuration
{
    /// <summary>
    /// Provides methods for reading and writing configuration objects.
    /// </summary>
    public interface IAppConfig
    {
        /// <summary>
        /// Reads the configuration object.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="key">Key.</param>
        /// <param name="useResolvers"><see langword="true"/>to use resolvers, or <see langword="false"/> otherwise.</param>
        /// <returns>The configuration object associated with the <paramref name="key"/>.
        /// If the object does not exist, the method returns <see langword="null"/>.</returns>
        /// <remarks><seealso cref="IAppConfigConfiguration.ResolveDefaultKey(string)"/></remarks>
        T Get<T>(string key, bool useResolvers = true);

        /// <summary>
        /// Writes the configuration object.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="key">Key.</param>
        /// <param name="value">The configuration object.</param>
        void Set<T>(string key, T value);
    }
}