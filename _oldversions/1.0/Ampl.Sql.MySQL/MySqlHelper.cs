using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ampl.Sql
{
  public class MySqlHelper : ISqlHelper
  {
    public DbConnection CreateConnection(string connectionString)
    {
      return new MySqlConnection
        {
          ConnectionString = connectionString
        };
    }

    public DbCommand CreateCommand()
    {
      return new MySqlCommand();
    }

    public char ParameterPrefixChar
    {
      get { return '?'; }
    }

    public void AddCommandParameter(IDbCommand command,
      string parameterName, string preferedType, object parameterValue)
    {
      MySqlCommand cmd = (command as MySqlCommand);
      MySqlDbType dbType = MySqlDbType.VarString;
      if(!string.IsNullOrEmpty(preferedType))
      {
        dbType = (MySqlDbType)Enum.Parse(typeof(MySqlDbType), preferedType);
      }
      else
      {
        if(parameterValue is decimal)
        {
          dbType = MySqlDbType.Decimal;
        }
      }

      cmd.Parameters.Add(parameterName, dbType).Value = parameterValue;
    }

    private ExceptionReason NumberToReason(int number)
    {
      var map = new[]
        {
          new { n = 1062, r = ExceptionReason.UniqueKeyViolated },        
        };

      foreach(var pair in map)
      {
        if(pair.n == number)
        {
          return pair.r;
        }
      }
      return ExceptionReason.Unknown;
    }

    public ExceptionDetails GetExceptionDetails(DbException exception)
    {
      ExceptionDetails result = new ExceptionDetails();
      MySqlException e = (exception as MySqlException);
      if(e == null)
      {
        return result;
      }

      result.Reason = NumberToReason(e.Number);
      return result;
    }
  }
}
