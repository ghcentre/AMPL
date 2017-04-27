using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.TypeExtensions
{
  /// <summary>
  /// Summary description for TypeExtensions_Test
  /// </summary>
  [TestClass]
  public class TypeExtensions_Test
  {
    [TestMethod]
    public void ExtractGeneticInterface_NullExtension_ReturnsNull()
    {
      Type someType = null;

      Type result = someType.ExtractGenericInterface(typeof(IEnumerable<>));

      Assert.IsNull(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ExtractGeneticInterface_NullSecondParam_Throws()
    {
      Type someType = typeof(string);

      Type result = someType.ExtractGenericInterface(null);
    }

    [TestMethod]
    public void ExtractGeneticInterface_FirstNotImplements_ReturnsNull()
    {
      Type someType = typeof(int);

      Type result = someType.ExtractGenericInterface(typeof(IEnumerable<>));

      Assert.IsNull(result);
    }

    [TestMethod]
    public void ExtractGeneticInterface_Implementing_ReturnsFirstGeneric()
    {
      Type someType = typeof(string);

      Type result = someType.ExtractGenericInterface(typeof(IEnumerable<>));

      Assert.AreEqual(typeof(IEnumerable<char>), result);
    }

    [TestMethod]
    public void ExtractGeneticInterface_GenericType_ReturnsThisType()
    {
      Type someType = typeof(IEnumerable<string>);

      Type result = someType.ExtractGenericInterface(typeof(IEnumerable<>));

      Assert.AreEqual(typeof(IEnumerable<string>), result);
    }

    [TestMethod]
    public void ExtractGeneticInterface_GenericTypeSecondParamNotGeneric_ReturnsNukk()
    {
      Type someType = typeof(IEnumerable<string>);

      Type result = someType.ExtractGenericInterface(typeof(string));

      Assert.IsNull(result);
    }
  }
}
