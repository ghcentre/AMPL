namespace Ampl.Configuration
{
    /// <summary>
    /// The implementation of <see cref="IAppConfigEntity"/>.
    /// </summary>
    internal class InternalAppConfigEntity : IAppConfigEntity
    {
        /// <inheritdoc/>
        public string Key { get; set; }

        /// <inheritdoc/>
        public string Value { get; set; }
    }
}
