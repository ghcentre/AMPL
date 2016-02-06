using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Identity.EntityFramework
{
  public interface IAccessControlList
  {
    int Position { get; set; }

    string Actions { get; set; }

    string Resources { get; set; }

    string Users { get; set; }

    string Roles { get; set; }

    bool Allow { get; set; }
  }
}
