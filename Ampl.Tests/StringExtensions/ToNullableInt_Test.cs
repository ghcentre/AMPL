using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace GHC.Ampl.System.Tests
{
  [TestClass]
  public class ToNullableInt_Test
  {
    [TestMethod]
    public void ToNullableInt_Source_Null_returns_null()
    {
      string argument = null;
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == null);
    }

    [TestMethod]
    public void ToNullableInt_NumbersOnly()
    {
      string argument = "12345";
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == 12345);
    }

    [TestMethod]
    public void ToNullableInt_WithMinus()
    {
      string argument = "-12345";
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == -12345);
    }

    [TestMethod]
    public void ToNullableInt_WithPlus()
    {
      string argument = "+12345";
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == 12345);
    }

    [TestMethod]
    public void ToNullableInt_WithInvalidChars()
    {
      string argument1 = " 12345";
      int? result1 = argument1.ToNullableInt();
      Assert.IsTrue(result1 == null);
      Assert.IsTrue(result1 != 12345);

      string argument2 = "1.2345";
      int? result2 = argument2.ToNullableInt();
      Assert.IsTrue(result2 == null);

      string argument3 = "1e8";
      int? result3 = argument3.ToNullableInt();
      Assert.IsTrue(result3 == null);

      string argument4 = "156M";
      int? result4 = argument4.ToNullableInt();
      Assert.IsTrue(result4 == null);
    }

    [TestMethod]
    public void ToNullableInt_WithOverflow()
    {
      string argument1 = "12248498309484849932884566562093428839345";
      int? result1 = argument1.ToNullableInt();
      Assert.IsTrue(result1 == null);
    }


  }
}
