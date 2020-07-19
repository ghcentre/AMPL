using System;
using System.Globalization;

namespace Ampl.Configuration
{
    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="string"/>.
    /// </summary>
    public class StringConverter : IAppConfigConverter
    {
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(string));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) => entityValue;
        
        /// <inheritdoc/>
        public string WriteEntity(object objectValue) => (string)objectValue;
    }

    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="int"/>.
    /// </summary>
    public class IntConverter : IAppConfigConverter
    {
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(int));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) => int.Parse(entityValue);
        
        /// <inheritdoc/>
        public string WriteEntity(object objectValue) => ((int)objectValue).ToString();
    }

    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="Nullable{Int32}"/>.
    /// </summary>
    public class NullableIntConverter : IAppConfigConverter
    {
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(int?));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) =>
            string.IsNullOrEmpty(entityValue)
                ? null
                : (int?)int.Parse(entityValue);

        /// <inheritdoc/>
        public string WriteEntity(object objectValue) => ((int?)objectValue).ToString();
    }

    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="bool"/>.
    /// </summary>
    public class BoolConverter : IAppConfigConverter
    {
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(bool));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) => bool.Parse(entityValue.ToLowerInvariant());
        
        /// <inheritdoc/>
        public string WriteEntity(object objectValue) => ((bool)objectValue).ToString();
    }

    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="Nullable{Boolean}"/>.
    /// </summary>
    public class NullableBoolConverter : IAppConfigConverter
    {
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(bool?));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) => string.IsNullOrEmpty(entityValue) ? null : (bool?)bool.Parse(entityValue);
        
        /// <inheritdoc/>
        public string WriteEntity(object objectValue) => ((bool?)objectValue).ToString();
    }

    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="decimal"/>.
    /// </summary>
    public class DecimalConverter : IAppConfigConverter
    {
        private readonly CultureInfo _convertCulture = new CultureInfo("en-US");
        
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(decimal));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) => decimal.Parse(entityValue, _convertCulture);
        
        /// <inheritdoc/>
        public string WriteEntity(object objectValue) => ((decimal)objectValue).ToString(_convertCulture);
    }

    /// <summary>
    /// The implementation of the <see cref="IAppConfigConverter"/> interface
    /// that handles objects of type <see cref="Nullable{Decimal}"/>.
    /// </summary>
    public class NullableDecimalConverter : IAppConfigConverter
    {
        private readonly CultureInfo _convertCulture = new CultureInfo("en-US");
        
        /// <inheritdoc/>
        public bool CanConvert(Type type) => type.Equals(typeof(decimal?));
        
        /// <inheritdoc/>
        public object ReadEntity(string entityValue) =>
            string.IsNullOrEmpty(entityValue)
                ? null
                : (decimal?)decimal.Parse(entityValue, _convertCulture);
        
        /// <inheritdoc/>
        public string WriteEntity(object objectValue) =>
            ((decimal?)objectValue).HasValue
                ? ((decimal?)objectValue).Value.ToString(_convertCulture)
                : null;
    }
}
