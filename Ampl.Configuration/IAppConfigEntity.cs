using System.ComponentModel.DataAnnotations;

namespace Ampl.Configuration
{
    /// <summary>
    /// Represents the confuguration entity which has a key and a value.
    /// </summary>
    public interface IAppConfigEntity
    {
        /// <summary>
        /// Gets or sets the entity key.
        /// </summary>
        /// <value>The entity key.</value>
        /// <remarks>This entity is required.</remarks>
        [Required]
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the entity value.
        /// </summary>
        /// <value>The entity value.</value>
        string Value { get; set; }
    }
}
