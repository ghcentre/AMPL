using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ampl.Sql
{
  /// <summary>
  /// Describes a reason for an exception
  /// </summary>
  public enum ExceptionReason
  {
    /// <summary>
    /// The reason for exception is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Unique/primary key constraint violated.
    /// </summary>
    UniqueKeyViolated = 1,
  }
}
