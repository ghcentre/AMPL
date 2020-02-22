using Ampl.Core;
using System.Collections.Generic;

namespace Ampl.Configuration
{
    /// <summary>
    /// Provides the default implementation for the <see cref="IAppConfigConfiguration"/> interface.
    /// </summary>
    public class AppConfigDefaultConfiguration : IAppConfigConfiguration
    {
        private List<IAppConfigConverter> _converters = new List<IAppConfigConverter>();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppConfigDefaultConfiguration()
        {
            AddConverter(new StringConverter());
            AddConverter(new IntConverter());
            AddConverter(new NullableIntConverter());
            AddConverter(new BoolConverter());
            AddConverter(new NullableBoolConverter());
            AddConverter(new DecimalConverter());
            AddConverter(new NullableDecimalConverter());
        }

        /// <summary>
        /// Adds the converter to the list of converters.
        /// </summary>
        /// <param name="converter">The converter.</param>
        private void AddConverter(IAppConfigConverter converter)
        {
            _converters.Add(Check.NotNull(converter, nameof(converter)));
        }

        /// <inheritdoc/>>
        public IEnumerable<IAppConfigConverter> GetConverters()
        {
            return _converters.ToArray();
        }

        /// <inheritdoc/>
        public string ResolveDefaultKey(string key)
        {
            return null; // no key resolvers by default
        }
    }
}
