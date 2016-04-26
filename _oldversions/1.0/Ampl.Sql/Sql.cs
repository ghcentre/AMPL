using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;
using System.Configuration;
using GHC.Utilities;
using System.Globalization;
using System.Collections;
using System.Data.Common;

namespace Ampl.Sql
{
  /// <summary>
  /// IDbConnection, IDbCommand Helper.
  /// </summary>
  public static class Sql
  {
    #region Connection Creation, Opening and Deletion

    public static IDbConnection CreateConnection(string connectionStringSelector = "default")
    {
      string connectionString = ConfigurationManager.ConnectionStrings[
          ArgValidator.CheckEmptyString(connectionStringSelector, "connectionStringSelector")]
        .ConnectionString;
      if(connectionString.IsEmpty())
      {
        throw new ConfigurationErrorsException(
          ErrorMessages.NoConnectionString.FormatWith(connectionStringSelector));
      }
      return SqlHelper.CreateConnection(connectionString);
    }

    public static IDbConnection CreateOpenConnection(string connectionStringSelector = "default")
    {
      IDbConnection connection = CreateConnection(connectionStringSelector);
      connection.Open();
      return connection;
    }

    #endregion

    #region Parse Command Arguments

    /// <summary>
    /// Parses command arguments ({0}, {1} and so on).
    /// </summary>
    /// <param name="command"><c>OracleCommand</c>.</param>
    /// <param name="commandText">Command text.</param>
    /// <param name="args">Argument array.</param>
    /// <remarks>
    /// <para>The method parses command text containing numbers in curly
    /// braces ({0}, {1}, etc.)</para>
    /// <para>For each parsed argument, it created <c>OracleParameter</c> object
    /// and binds it value to corresponding parameter in <c>args</c> array.</para>
    /// </remarks>
    internal static void ParseCommandArguments(IDbCommand command, string commandText,
      object[] args)
    {

      //
      // calc number of arguments and replace each {NN} with ?paramNN
      // format string is in the following format
      //  {parameterNumber[:datatype]}
      //
      int numberOfArguments = 0;
      //while(commandText.Contains("{" + numberOfArguments.ToString() + "}"))
      //{
      //  commandText = commandText.Replace(
      //    "{" + numberOfArguments.ToString() + "}",
      //    ":P" + numberOfArguments.ToString());
      //  numberOfArguments++;
      //}

      Dictionary<int, string> dicDatatypes = new Dictionary<int, string>();
      while(true)
      {
        string paramBegin = "{" + numberOfArguments.ToString();
        if(!commandText.Contains(paramBegin))
        {
          break;
        }

        string param = commandText.Between(paramBegin, string.Empty, false);
        param = param.Between(string.Empty, "}", false);
        string datatype = param.Between(":", string.Empty, false);
        if(!datatype.IsEmpty())
        {
          dicDatatypes.Add(numberOfArguments, datatype);
        }

        commandText = commandText.Replace(
          (paramBegin + param + "}"),
          (_sqlHelper.ParameterPrefixChar + "P" + numberOfArguments.ToString())
        );
        numberOfArguments++;
      }

      if(args.Length != numberOfArguments)
      {
        throw new FormatException(ErrorMessages.InvalidNumberOfParameters);
      }

      for(int i = 0; i < numberOfArguments; ++i)
      {
        string preferedType = string.Empty;
        if(dicDatatypes.ContainsKey(i))
        {
          preferedType = dicDatatypes[i];
        }

        object argument = (args[i] == null || (args[i] is DBNull))
          ? (object)DBNull.Value
          : (object)args[i];
            
        _sqlHelper.AddCommandParameter(
          command, 
          "P" + i.ToString(),
          preferedType,
          argument);
      }
      command.CommandText = commandText;
    }

    #endregion

    #region IDataRecord fetching

    /// <summary>
    /// Creates enumerable collection from an SQL query.
    /// </summary>
    /// <param name="connection"><c>IdbConnection</c>.</param>
    /// <param name="commandText">Command text.</param>
    /// <param name="args">Arguments.</param>
    /// <returns>The method returns an enumerable collection of
    /// <c>IDataRecord</c> objects.</returns>
    /// <remarks>Bindable parameters in command text must appear as numbers enclosed in
    /// curly braces. Number of arguments in <c>args</c> must match number of parameters
    /// in command text.</remarks>
    /// <example>
    /// <code>
    /// foreach(IDataRecord r in Sql.CreateEnumerableEx(connection,
    ///   "select * from Table where Field = {0}",
    ///   fieldValue))
    /// {
    ///   Console.WriteLine(r["ID"].ToString());
    ///   if((decimal)r["ID"] == 5)
    ///   {
    ///     break; // break from loop is handled correctly.
    ///   }
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<IDataRecord> CreateEnumerableEx(IDbConnection connection,
      string commandText, params object[] args)
    {
      DbCommand command = _sqlHelper.CreateCommand();
      DbConnection dbconnection = (connection as DbConnection);
      if(dbconnection == null)
      {
        throw new InvalidOperationException(ErrorMessages.IConnectionIsNotDbConnection);
      }

      try
      {
        command.Connection = dbconnection;
        ParseCommandArguments(command, commandText, args);
        DbDataReader reader = command.ExecuteReader();

        try
        {
          IEnumerator enumerator = reader.GetEnumerator();
          while(enumerator.MoveNext())
          {
            yield return (IDataRecord)enumerator.Current;
          }
        }
        finally
        {
          reader.Close();
          reader.Dispose();
        }
      }
      finally
      {
        DisposeCommandParameters(command);
        command.Dispose();
      }
      yield break;

    }

    public static IDataRecord GetSingleRow(IDbConnection connection,
      string commandText, params object[] args)
    {
      //return CreateEnumerableEx(connection, commandText, args).FirstOrDefault();
      foreach(IDataRecord r in CreateEnumerableEx(connection, commandText, args))
      {
        return r;
      }
      return null;
    }

    #endregion

    #region Dispose Command Parameters

    /// <summary>
    /// Disposes all parameters and theis values for the <c>OracleCommand</c>.
    /// </summary>
    /// <param name="command"><c>OracleCommand</c>.</param>
    internal static void DisposeCommandParameters(IDbCommand command)
    {
      foreach(IDbDataParameter parameter in command.Parameters)
      {
        if(parameter.Value is IDisposable)
        {
          (parameter.Value as IDisposable).Dispose();
        }
        if(parameter is IDisposable)
        {
          (parameter as IDisposable).Dispose();
        }
      }
    }

    #endregion

    #region Query Execution

    /// <summary>
    /// Executes an SQL query against the database.
    /// </summary>
    /// <param name="connection"><c>IDbConnection</c>.</param>
    /// <param name="commandText">Command text.</param>
    /// <param name="args">Arguments.</param>
    /// <returns>The method returns number of rows affected by the query.</returns>
    /// <remarks>Bindable parameters in command text must appear as numbers enclosed in
    /// curly braces. Number of arguments in <c>args</c> must match number of parameters
    /// in command text.</remarks>
    /// <example>
    /// <code>
    /// int rowsAffected = Sql.ExecNonQueryEx(connection,
    ///   "update Table set IntField = IntField + 1 where ID = {0}",
    ///   idValue);
    /// </code>
    /// </example>
    public static int ExecNonQueryEx(IDbConnection connection,
      string commandText, params object[] args)
    {
      int rowsAffected = 0;
      IDbCommand command = _sqlHelper.CreateCommand();
      try
      {
        command.Connection = connection;
        ParseCommandArguments(command, commandText, args);
        rowsAffected = command.ExecuteNonQuery();
      }
      finally
      {
        DisposeCommandParameters(command);
        command.Dispose();
      }
      return rowsAffected;
    }

    public delegate void ExecNonQueryExExecuted(IDbCommand command);

    public static int ExecNonQueryEx(IDbConnection connection,
      ExecNonQueryExExecuted afterExecute,
      string commandText, params object[] args)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region Exception Handling

    /// <summary>
    /// Gets details for the DB exception.
    /// </summary>
    /// <param name="exception">An exception that occured during command execution.</param>
    /// <returns>The method returns the <c>ExceptionReason</c> object which describes
    /// reason for the exception.</returns>
    public static ExceptionDetails GetExceptionDetails(DbException exception)
    {
      return _sqlHelper.GetExceptionDetails(exception);
    }

    #endregion

    #region SqlHelper Implementation Object

    /// <summary>
    /// Gets a <c>ISqlHelper</c> implementation object.
    /// </summary>
    /// <value>An <c>ISqlHelper</c> implementation object.</value>
    private static ISqlHelper SqlHelper
    {
      get
      {
        lock(_sqlHelperLocker)
        {
          if(_sqlHelper == null)
          {
            _sqlHelper = FindImplementation();
          }
          return _sqlHelper;
        }
      }
    }
    private static ISqlHelper _sqlHelper = null;
    private static object _sqlHelperLocker = new object();

    /// <summary>
    /// Finds an implementation of an <c>ISqlHelper</c> interface.
    /// </summary>
    /// <returns>Reference to an object implementing the <c>ISqlHelper</c> interface.</returns>
    /// <remarks>
    /// <para>The method looks for assemblies named 'Ampl.Sql.*.dll' in a directory
    /// where Ampl.Sql.dll resides.</para>
    /// <para>The first found assembly is loaded and the first type that implements
    /// the <c>ISqlHelper interface is instantiated using <b>parameterless</b>
    /// constructor.</para>
    /// </remarks>
    private static ISqlHelper FindImplementation()
    {
      string location = Assembly.GetExecutingAssembly().Location;
      string path = Path.GetDirectoryName(location);
      string[] files = Directory.GetFiles(path, "Ampl.Sql.*.dll");
      if(files.Length == 0)
      {
        throw new FileNotFoundException(ErrorMessages.NoSqlHelperAssembliesFound);
      }

      Assembly assembly = Assembly.LoadFrom(files[0]);
      foreach(Type type in assembly.GetExportedTypes())
      {
        foreach(Type interfaceType in type.GetInterfaces())
        {
          if(interfaceType == typeof(ISqlHelper))
          {
            ISqlHelper implementation = (ISqlHelper)Activator.CreateInstance(type);
            return implementation;
          }
        }
      }

      //
      // no class implementing ISqlHelper found.
      //
      throw new BadImageFormatException(ErrorMessages.NoSqlHelperFound,
        assembly.Location);
    }

    #endregion

    /// <summary>
    /// Initializes static fields of the class.
    /// </summary>
    static Sql()
    {
    }
  }
}
