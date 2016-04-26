using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Ampl.Sql
{
  public interface ISqlHelper
  {
    /// <summary>
    /// Initializes a new instance of the <c>DbConnection</c> class.
    /// </summary>
    /// <param name="connectionString">Connection string to be passed
    /// to the new object's <c>ConnectionString</c> property.</param>
    /// <returns>A new instance of the class, the <c>DbConnection</c> descendant.</returns>
    DbConnection CreateConnection(string connectionString);

    /// <summary>
    /// Initializes a new instance of the <c>DbCommand</c> class.
    /// </summary>
    /// <returns>A new instance of the class, the <c>DbCommand</c> descendant.</returns>
    /// <remarks>Any post-initialization must be performed in this method.</remarks>
    DbCommand CreateCommand();

    /// <summary>
    /// Gets parameter prefix character.
    /// </summary>
    /// <value>Parameter prefix character.</value>
    char ParameterPrefixChar { get; }

    /// <summary>
    /// Adds a parameter to command
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="parameterName">Parameter name.</param>
    /// <param name="preferedType">Prefered type or an empty string to auto-detect parameter type.</param>
    /// <param name="parameterValue">Parameter value</param>
    void AddCommandParameter(IDbCommand command,
      string parameterName, string preferedType, object parameterValue);

    /// <summary>
    /// Gets details for the exception.
    /// </summary>
    /// <param name="exception">An exception that occured during command execution.</param>
    /// <returns>The method returns the <c>ExceptionReason</c> object which describes
    /// reason for the exception.</returns>
    ExceptionDetails GetExceptionDetails(DbException exception);

  }
}
