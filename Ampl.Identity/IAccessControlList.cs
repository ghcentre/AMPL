//using System;

namespace Ampl.Identity
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
