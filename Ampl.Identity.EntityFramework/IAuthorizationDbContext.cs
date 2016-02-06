using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Identity.EntityFramework
{
  public interface IAuthorizationDbContext
  {
    IEnumerable<IAccessControlList> GetAccessControlLists();
  }
}
