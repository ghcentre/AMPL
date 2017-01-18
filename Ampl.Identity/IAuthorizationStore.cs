using System.Collections.Generic;

namespace Ampl.Identity
{
  public interface IAuthorizationStore
  {
    IEnumerable<IAccessControlList> GetAccessControlLists();
  }
}
