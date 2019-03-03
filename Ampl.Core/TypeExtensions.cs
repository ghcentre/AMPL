using System;
using System.Reflection;

namespace Ampl.System
{
  /// <summary>
  /// Provides a set of <see langword="static"/> methods used to manipulate CLR types.
  /// </summary>
  public static class TypeExtensions
  {
    /// <summary>
    /// Extracts generic interface from the type.
    /// </summary>
    /// <param name="thisType">The type to extract generic interface from.</param>
    /// <param name="interfaceType">The type of interface to extract.</param>
    /// <returns>The method returns the <see cref="Type"/> of the first generic interface the type specified
    /// in <paramref name="thisType"/> implements, or <see langword="null"/> if the type is not implementing
    /// the interface.</returns>                                         
    /// <remarks>The method returns <see langword="null"/> if <paramref name="thisType"/> is null.</remarks>
    /// <exception cref="ArgumentNullException">The <paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    public static Type ExtractGenericInterface(this Type thisType, Type interfaceType)
    {
      if(thisType == null)
      {
        return null;
      }
      Check.NotNull(interfaceType, nameof(interfaceType));

      if(MatchesGenericTypeDefinition(thisType, interfaceType))
      {
        return thisType;
      }

      foreach(var implementedInterfaceType in 
                  thisType.GetTypeInfo().ImplementedInterfaces
                  //thisType.GetInterfaces()
             )
      {
        if(MatchesGenericTypeDefinition(implementedInterfaceType, interfaceType))
        {
          return implementedInterfaceType;
        }
      }

      return null;
    }

    #region Interface Extraction

    private static bool MatchesGenericTypeDefinition(Type checkType, Type genericTypeDefinition)
    {
      var ti = checkType.GetTypeInfo();
      return ti.IsGenericType && checkType.GetGenericTypeDefinition() == genericTypeDefinition;
      //return checkType.IsGenericType && checkType.GetGenericTypeDefinition() == genericTypeDefinition;
    }

    #endregion
  }
}
