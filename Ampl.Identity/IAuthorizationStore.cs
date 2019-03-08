using System.Collections.Generic;

namespace Ampl.Identity
{
    /// <summary>
    /// Represents an Authorization Store.
    /// </summary>
    public interface IAuthorizationStore
    {
        /// <summary>
        /// Gets the ACL entries.
        /// </summary>
        /// <returns>The sequence of <see cref="IAccessControlList"/> entries.</returns>
        IEnumerable<IAccessControlList> GetAccessControlLists();
    }
}
