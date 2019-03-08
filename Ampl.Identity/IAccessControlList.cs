namespace Ampl.Identity
{
    /// <summary>
    /// Represents an entry in the Access Control List.
    /// </summary>
    public interface IAccessControlList
    {
        /// <summary>
        /// Gets or sets the Position of this entry in the list.
        /// </summary>
        /// <value>Position of this entry in the list.</value>
        int Position { get; set; }

        /// <summary>
        /// Gets or sets the Actions this entry controls.
        /// </summary>
        /// <value>Actions this entry controls.</value>
        string Actions { get; set; }

        /// <summary>
        /// Gets or sets the Resources this entry controls.
        /// </summary>
        /// <value>Resources this entry controls.</value>
        string Resources { get; set; }

        /// <summary>
        /// Gets or sets the Users this entry controls.
        /// </summary>
        /// <value>Users this entry controls.</value>
        string Users { get; set; }

        /// <summary>
        /// Gets or sets the Roles this entry controls.
        /// </summary>
        /// <value>Roles this entry controls.</value>
        string Roles { get; set; }

        /// <summary>
        /// Gets or sets the value indicating to allow or deny the user and/or role to the combination of actions and resources.
        /// </summary>
        /// <value>A <see cref="bool"/> value.</value>
        bool Allow { get; set; }
    }
}
