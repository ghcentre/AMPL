using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ampl.Sql
{
  /// <summary>
  /// Represents DB Exception Details.
  /// </summary>
  public class ExceptionDetails
  {
    /// <summary>
    /// Reason for the exception.
    /// </summary>
    public ExceptionReason Reason { get; set; }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    public ExceptionDetails()
    {
      Reason = ExceptionReason.Unknown;
    }
  }
}
