using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;
using System.Threading;
using System.Globalization;

namespace GHC.Ampl.System.Tests
{
  [TestClass]
  public class ToNullableDateTime_Test
  {
    [TestMethod]
    public void ToNullableDateTime_Source_Null_returns_null()
    {
      string argument = null;
      DateTime? result1 = argument.ToNullableDateTime();
      Assert.IsTrue(result1 == null);
    }

    [TestMethod]
    public void ToNullableDateTime_Source_Empty_returns_null()
    {
      Assert.IsTrue("".ToNullableDateTime() == null);
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDate()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "11.12.2015";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(2015, 12, 11));
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDateIso()
    {
      string argument = "1972-12-07";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(1972, 12, 7));
    }

    [TestMethod]
    public void ToNullableDecimal_ValidTime()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result.Value.TimeOfDay == new TimeSpan(17, 25, 1));
    }

    [TestMethod]
    public void ToNullableDecimal_ValidTimeIso()
    {
      string argument = "17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result.Value.TimeOfDay == new TimeSpan(17, 25, 1));
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDateTime()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "7.12.1972 17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(1972, 12, 7, 17, 25, 1));
    }

    [TestMethod]
    public void ToNullableDecimal_ValidDateTimeIso()
    {
      string argument = "1972-12-07T17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(1972, 12, 7, 17, 25, 1));
    }

    [TestMethod]
    public void ToNullableDecimal_Invalid_string_returns_null()
    {
      string argument = "invalid";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == null);
    }

    [TestMethod]
    public void ToNullableDecimal_AnotherCulture_date_returns_null()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12/24/1972"; // tries to parse as day=12, month=24 and fails
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == null);

      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
      DateTime? result2 = argument.ToNullableDateTime();
      Assert.IsTrue(result2 == new DateTime(1972, 12, 24));
    }


  }
}
