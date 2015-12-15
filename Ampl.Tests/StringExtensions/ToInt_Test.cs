using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace GHC.Ampl.System.Tests
{
  [TestClass]
  public class ToInt_Test
  {
    [TestMethod]
    public void ToInt_Source_Null_returns_default()
    {
      string argument = null;
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == 0);

      int result2 = argument.ToInt(-1);
      Assert.IsTrue(result2 == -1);
    }

    [TestMethod]
    public void ToInt_NumbersOnly()
    {
      string argument = "12345";
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == 12345);
    }

    [TestMethod]
    public void ToInt_WithMinus()
    {
      string argument = "-12345";
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == -12345);
    }

    [TestMethod]
    public void ToInt_WithPlus()
    {
      string argument = "+12345";
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == 12345);
    }

    [TestMethod]
    public void ToInt_WithInvalidChars()
    {
      string argument1 = " 12345";
      int result1 = argument1.ToInt();
      Assert.IsTrue(result1 == 0);

      string argument2 = "1.2345";
      int result2 = argument2.ToInt();
      Assert.IsTrue(result2 == 0);

      string argument3 = "1e8";
      int result3 = argument3.ToInt();
      Assert.IsTrue(result3 == 0);

      string argument4 = "156M";
      int result4 = argument4.ToInt();
      Assert.IsTrue(result4 == 0);
    }

    [TestMethod]
    public void ToInt_WithOverflow()
    {
      string argument1 = "12248498309484849932884566562093428839345";
      int result1 = argument1.ToInt();
      Assert.IsTrue(result1 == 0);
    }


  }
}
