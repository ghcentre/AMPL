using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;
using System.Threading;
using System.Globalization;

namespace GHC.Ampl.System.Tests
{
  [TestClass]
  public class ToNullableDecimal_Test
  {
    [TestMethod]
    public void ToNullableDecimal_Source_Null_returns_null()
    {
      string argument = null;
      decimal? result1 = argument.ToNullableDecimal();
      Assert.IsTrue(result1 == null);
    }

    [TestMethod]
    public void ToNullableDecimal_ValidInteger()
    {
      string argument = "12345";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == 12345);
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDecimalCurrentCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345,67";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDecimalFallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDecimal_NO_FallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal? result = argument.ToNullableDecimal(false);
      Assert.IsTrue(result == null);
    }

    [TestMethod]
    public void ToNullableDecimal_Invalid()
    {
      string argument = "invalid";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == null);
    }

  }
}
