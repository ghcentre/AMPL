using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;
using System.Threading;
using System.Globalization;

namespace GHC.Ampl.System.Tests
{
  [TestClass]
  public class ToDecimal_Test
  {
    [TestMethod]
    public void ToDecimal_Source_Null_returns_default()
    {
      string argument = null;
      decimal result1 = argument.ToDecimal();
      Assert.IsTrue(result1 == 0);

      decimal result2 = argument.ToDecimal(-1);
      Assert.IsTrue(result2 == -1);
    }

    [TestMethod]
    public void ToDecimal_ValidInteger()
    {
      string argument = "12345";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 12345);
    }

    [TestMethod]
    public void ToDecimal_ValidDecimalCurrentCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345,67";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [TestMethod]
    public void ToDecimal_ValidDecimalFallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [TestMethod]
    public void ToDecimal_ValidDecimal_NO_FallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal result = argument.ToDecimal(1, false);
      Assert.IsTrue(result == 1);
    }

    [TestMethod]
    public void ToDecimal_Invalid()
    {
      string argument = "invalid";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 0);
    }

  }
}
