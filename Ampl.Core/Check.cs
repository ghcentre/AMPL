using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.System
{
  /// <summary>
  /// Provides a set of <see langword="static" /> parameter checking routines.
  /// </summary>
  public static class Check
  {
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the given value is null.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="argumentValue"/>.</typeparam>
    /// <param name="argumentValue">The value to check for null.</param>
    /// <param name="argumentName">The argument name.</param>
    /// <returns>The method returns the reference passed in <paramref name="argumentValue"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// The reference passed in <paramref name="argumentValue"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <para>The folowing code demonstrates argument null checking:</para>
    /// <code>
    /// private void SomeMethod(SomeObject obj)
    /// {
    ///   Check.NotNull(obj, "obj"); // throws ArgumentNullException if obj == null
    /// }
    /// </code>
    /// <para>The following code demonstrates argument null checking combined with assignment:</para>
    /// <code>
    /// internal class TheClass
    /// {
    ///   private SomeType _theType;
    ///   // ...
    ///   public TheClass(SomeType theType)
    ///   {
    ///     //
    ///     // throws ArgumentNullException if theType == null
    ///     //   else assigns theType to _theType
    ///     //
    ///     _theType = Check.NotNull(theType, "theType");
    ///   }
    /// }
    /// </code>
    /// </example>
    public static T NotNull<T>(T argumentValue, string argumentName = null)
    {
      if(argumentValue == null)
      {
        throw argumentName == null
          ? new ArgumentNullException()
          : new ArgumentNullException(argumentName);
      }
      return argumentValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="argumentValue"></param>
    /// <param name="argumentName"></param>
    /// <returns></returns>
    public static string NotNullOrEmptyString(string argumentValue, string argumentName = null)
    {
      NotNull(argumentValue, argumentName);
      if(argumentValue.Length == 0)
      {
        throw argumentName == null
          ? new ArgumentException(Messages.ValueCannotBeAnEmptyString)
          : new ArgumentException(Messages.ValueCannotBeAnEmptyString, argumentName);
      }
      return argumentValue;
    }
  }
}
