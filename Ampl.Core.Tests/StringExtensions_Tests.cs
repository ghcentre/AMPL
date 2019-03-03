using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;
using System.Threading;
using System.Globalization;

namespace Ampl.Core.Tests
{
  [TestFixture]
  class StringExtensions_Tests
  {
    #region Between

    [Test]
    public void Between_Source_Null_returns_null()
    {
      string argument = null;
      string result = argument.Between("start", "end");
      Assert.IsNull(result);
    }

    [Test]
    public void Between_Start_Null_equals_empty()
    {
      string argument = "This is a test string";
      string result1 = argument.Between(null, "st");
      string result2 = argument.Between("", "st");
      Assert.AreEqual(result1, result2);
    }

    [Test]
    public void Between_End_Null_equals_empty()
    {
      string argument = "This is a test string";
      string result1 = argument.Between("is", null);
      string result2 = argument.Between("is", "");
      Assert.AreEqual(result1, result2);
    }

    [Test]
    public void Between_Start_and_End_both_empty_returns_source()
    {
      string argument = "This is a test string";
      string result = argument.Between(null, null);
      Assert.AreEqual(result, argument);
    }

    [Test]
    public void Between_Start_Found_End_Null()
    {
      string argument = "This is a test string";
      string result = argument.Between("This", null);
      Assert.AreEqual(result, " is a test string");

      string result2 = argument.Between("is", null);
      Assert.AreEqual(result2, " is a test string");
    }

    [Test]
    public void Between_Start_NotFound_NotKeep_Returns_Empty()
    {
      string argument = "This is a test string";
      string result = argument.Between("NotExist", null);
      Assert.AreEqual(result, string.Empty);
    }

    [Test]
    public void Between_Start_NotFound_Keep_Returns_Same()
    {
      string argument = "This is a test string";
      string result = argument.Between("NotExist", null, StringBetweenOptions.FallbackToSource);
      Assert.AreEqual(result, argument);
    }

    [Test]
    public void Between_Start_Null_End_Found()
    {
      string argument = "This is a test string";
      string result = argument.Between(null, "string");
      Assert.AreEqual(result, "This is a test ");

      string result2 = argument.Between(null, "st");
      Assert.AreEqual(result2, "This is a te");
    }

    [Test]
    public void Between_End_NotFound_NotKeep_Returns_Empty()
    {
      string argument = "This is a test string";
      string result = argument.Between(null, "NotExist");
      Assert.AreEqual(result, string.Empty);
    }

    [Test]
    public void Between_End_NotFound_Keep_Returns_Same()
    {
      string argument = "This is a test string";
      string result = argument.Between(null, "NotExist", StringBetweenOptions.FallbackToSource);
      Assert.AreEqual(result, argument);
    }

    [Test]
    public void Between_Start_End_Found()
    {
      string argument = "This is a test string";
      string result = argument.Between("is", "st");
      Assert.AreEqual(result, " is a te");
    }

    [Test]
    public void Between_Start_NotFound_End_Found()
    {
      string argument = "This is a test string";
      string result = argument.Between("Hello", "st", StringBetweenOptions.FallbackToSource);
      Assert.AreEqual(result, "This is a te");
    }

    [Test]
    public void Between_Start_NotFound_End_NotFound()
    {
      string argument = "This is a test string";
      string result = argument.Between("1", "2", StringBetweenOptions.FallbackToSource);
      Assert.AreEqual(result, argument);
    }

    [Test]
    public void Between_IgnoreCase()
    {
      string argument = "This is a test string";
      string result = argument.Between("THIS", "InG", StringBetweenOptions.FallbackToSource, StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual(result, " is a test str");
    }

    [Test]
    public void Between_IncludeStart()
    {
      string argument = "This is a test string.";
      string result = argument.Between("is", null, StringBetweenOptions.IncludeStart);
      Assert.AreEqual("is is a test string.", result);

      string result2 = argument.Between("is", "st", StringBetweenOptions.IncludeStart);
      Assert.AreEqual("is is a te", result2);
    }

    [Test]
    public void Between_IncludeEnd()
    {
      string argument = "This is a test string.";
      string result = argument.Between(null, "test", StringBetweenOptions.IncludeEnd);
      Assert.AreEqual("This is a test", result);

      string result2 = argument.Between("is", "st", StringBetweenOptions.IncludeEnd);
      Assert.AreEqual(" is a test", result2);
    }

    #endregion

    #region RemoveBetween

    [Test]
    public void RemoveBetween_Source_Null_returns_null()
    {
      string argument = null;
      string result = argument.RemoveBetween("start", "end");
      Assert.IsNull(result);
    }

    [Test]
    public void RemoveBetween_All_args_Removes_between_start_end()
    {
      string argument = "This is a test string";
      string result1 = argument.RemoveBetween("is", "test");
      string result2 = argument.RemoveBetween("iS", "tESt", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("Th string", result1);
      Assert.AreEqual("Th string", result2);
    }

    [Test]
    public void RemoveBetween_Start_null_Removes_from_start_of_string()
    {
      string argument = "This is a test string";
      string result1 = argument.RemoveBetween(null, "test");
      string result2 = argument.RemoveBetween(null, "Test", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual(" string", result1);
      Assert.AreEqual(" string", result2);
    }

    [Test]
    public void RemoveBetween_End_null_Removes_to_end_of_string()
    {
      string argument = "This is a test string";
      string result1 = argument.RemoveBetween("test", null);
      string result2 = argument.RemoveBetween("tEST", null, StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("This is a ", result1);
      Assert.AreEqual("This is a ", result2);
    }

    [Test]
    public void RemoveBetween_End_before_and_after_start_Removes_from_start_to_second_end()
    {
      string argument = "This string is a test string, English characters only.";
      string result1 = argument.RemoveBetween("is a", "ing");
      string result2 = argument.RemoveBetween("is A", "ing", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("This string , English characters only.", result1);
      Assert.AreEqual("This string , English characters only.", result2);
    }

    [Test]
    public void RemoveBetween_Start_or_end_not_found_returns_original()
    {
      string argument = "This is a test string.";
      string result1 = argument.RemoveBetween("123", "ing");
      string result2 = argument.RemoveBetween("is", "123");
      string result3 = argument.RemoveBetween("123", "456");
      Assert.AreEqual(argument, result1);
      Assert.AreEqual(argument, result2);
      Assert.AreEqual(argument, result3);
    }

    [Test]
    public void RemoveBetween_Start_end_null_returns_original()
    {
      string argument = "This is a test string.";
      string result = argument.RemoveBetween(null, null);
      Assert.AreEqual(argument, result);
    }

    [Test]
    public void RemoveBetween_Start_end_notnull_removes_all()
    {
      string argument1 = @"This is a <a href=""some"">test</a> string.";
      string argument2 = @"1st Begin Test One end 2nd bEgin teSt twO End 3rd begin";
      string result1 = argument1.RemoveBetween("<", ">");
      string result2 = argument2.RemoveBetween("begin", "end", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("This is a test string.", result1);
      Assert.AreEqual("1st  2nd  3rd begin", result2);
    }

    #endregion

    #region RemoveHtmlTags

    [Test]
    public void RemoveHtmlTags_Source_Null_returns_null()
    {
      string argument = null;
      string result = argument.RemoveHtmlTags();
      Assert.IsNull(result);
    }

    [Test]
    public void RemoveHtmlTags_Source_Empty_returns_empty()
    {
      string argument = string.Empty;
      string result = argument.RemoveHtmlTags();
      Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void RemoveHtmlTags_Removes()
    {
      string argument = @"
<html>
<head>
<title>Title</title>
</head>
<body>
<!-- comment -->
<ul>
  <li><a href=""one"">one text</a></li><li><a href=""two"">two text</a></li>
  <li><a href=""one"">three text</a></li><li><a href=""two"">four text</a></li>
</ul>
<script type=""text/javascript"">
  alert('hello');
</script>
</body>
</html>";
      string result = argument.RemoveHtmlTags();
      string expected = @"Title one text two text three text four text alert('hello');";
      Assert.AreEqual(expected, result);
    }

    #endregion

    #region ToNullIfWhiteSpace

    [Test]
    public void ToNullIfWhiteSpace_Null_ReturnsNull()
    {
      string arg = null;
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == null);
    }

    [Test]
    public void ToNullIfWhiteSpace_Empty_ReturnsNull()
    {
      string arg = string.Empty;
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == null);
    }

    [Test]
    public void ToNullIfWhiteSpace_Spaces_ReturnsNull()
    {
      string arg = " \t\r\n";
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == null);
    }

    [Test]
    public void ToNullIfWhiteSpace_Nonspace_ReturnsSame()
    {
      string arg = "\tThis\r\nis a test\t string";
      string result = arg.ToNullIfWhiteSpace();
      Assert.That(result, Is.SameAs(arg));
    }

    #endregion

    #region ToInt

    [Test]
    public void ToInt_Source_Null_returns_default()
    {
      string argument = null;
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == 0);

      int result2 = argument.ToInt(-1);
      Assert.IsTrue(result2 == -1);
    }

    [Test]
    public void ToInt_NumbersOnly()
    {
      string argument = "12345";
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == 12345);
    }

    [Test]
    public void ToInt_WithMinus()
    {
      string argument = "-12345";
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == -12345);
    }

    [Test]
    public void ToInt_WithPlus()
    {
      string argument = "+12345";
      int result1 = argument.ToInt();
      Assert.IsTrue(result1 == 12345);
    }

    [Test]
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

    [Test]
    public void ToInt_WithOverflow()
    {
      string argument1 = "12248498309484849932884566562093428839345";
      int result1 = argument1.ToInt();
      Assert.IsTrue(result1 == 0);
    }

    #endregion

    #region ToNullableInt

    [Test]
    public void ToNullableInt_Source_Null_returns_null()
    {
      string argument = null;
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == null);
    }

    [Test]
    public void ToNullableInt_NumbersOnly()
    {
      string argument = "12345";
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == 12345);
    }

    [Test]
    public void ToNullableInt_WithMinus()
    {
      string argument = "-12345";
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == -12345);
    }

    [Test]
    public void ToNullableInt_WithPlus()
    {
      string argument = "+12345";
      int? result1 = argument.ToNullableInt();
      Assert.IsTrue(result1 == 12345);
    }

    [Test]
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

    [Test]
    public void ToNullableInt_WithOverflow()
    {
      string argument1 = "12248498309484849932884566562093428839345";
      int? result1 = argument1.ToNullableInt();
      Assert.IsTrue(result1 == null);
    }

    #endregion

    #region ToDecimal

    [Test]
    public void ToDecimal_Source_Null_returns_default()
    {
      string argument = null;
      decimal result1 = argument.ToDecimal();
      Assert.IsTrue(result1 == 0);

      decimal result2 = argument.ToDecimal(-1);
      Assert.IsTrue(result2 == -1);
    }

    [Test]
    public void ToDecimal_ValidInteger()
    {
      string argument = "12345";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 12345);
    }

    [Test]
    public void ToDecimal_ValidDecimalCurrentCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345,67";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [Test]
    public void ToDecimal_ValidDecimalFallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [Test]
    public void ToDecimal_ValidDecimal_NO_FallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal result = argument.ToDecimal(1, false);
      Assert.IsTrue(result == 1);
    }

    [Test]
    public void ToDecimal_Invalid()
    {
      string argument = "invalid";
      decimal result = argument.ToDecimal();
      Assert.IsTrue(result == 0);
    }

    #endregion

    #region ToNullableDecimal

    [Test]
    public void ToNullableDecimal_Source_Null_returns_null()
    {
      string argument = null;
      decimal? result1 = argument.ToNullableDecimal();
      Assert.IsTrue(result1 == null);
    }

    [Test]
    public void ToNullableDecimal_ValidInteger()
    {
      string argument = "12345";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == 12345);
    }

    [Test]
    public void ToNullableDecimal_ValidDecimalCurrentCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345,67";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [Test]
    public void ToNullableDecimal_ValidDecimalFallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == 12345.67M);
    }

    [Test]
    public void ToNullableDecimal_ValidDecimal_NO_FallbackCulture()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "12345.67";
      decimal? result = argument.ToNullableDecimal(false);
      Assert.IsTrue(result == null);
    }

    [Test]
    public void ToNullableDecimal_Invalid()
    {
      string argument = "invalid";
      decimal? result = argument.ToNullableDecimal();
      Assert.IsTrue(result == null);
    }

    #endregion

    #region ToNullableDateTime

    [Test]
    public void ToNullableDateTime_Source_Null_returns_null()
    {
      string argument = null;
      DateTime? result1 = argument.ToNullableDateTime();
      Assert.IsTrue(result1 == null);
    }

    [Test]
    public void ToNullableDateTime_Source_Empty_returns_null()
    {
      Assert.IsTrue("".ToNullableDateTime() == null);
    }

    [Test]
    public void ToNullableDecimal_ValidDate()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "11.12.2015";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(2015, 12, 11));
    }

    [Test]
    public void ToNullableDecimal_ValidDateIso()
    {
      string argument = "1972-12-07";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(1972, 12, 7));
    }

    [Test]
    public void ToNullableDecimal_ValidTime()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result.Value.TimeOfDay == new TimeSpan(17, 25, 1));
    }

    [Test]
    public void ToNullableDecimal_ValidTimeIso()
    {
      string argument = "17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result.Value.TimeOfDay == new TimeSpan(17, 25, 1));
    }

    [Test]
    public void ToNullableDecimal_ValidDateTime()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
      string argument = "7.12.1972 17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(1972, 12, 7, 17, 25, 1));
    }

    [Test]
    public void ToNullableDecimal_ValidDateTimeIso()
    {
      string argument = "1972-12-07T17:25:01";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == new DateTime(1972, 12, 7, 17, 25, 1));
    }

    [Test]
    public void ToNullableDecimal_Invalid_string_returns_null()
    {
      string argument = "invalid";
      DateTime? result = argument.ToNullableDateTime();
      Assert.IsTrue(result == null);
    }

    [Test]
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


    #endregion

    #region Reverse

    [Test]
    public void Reverse_Null_ReturnsNull()
    {
      string arg = null;
      string res = arg.Reverse();
      Assert.IsNull(res);
    }

    [Test]
    public void Reverse_Empty_ReturnsEmpty()
    {
      string arg = string.Empty;
      string res = arg.Reverse();
      Assert.AreEqual(string.Empty, res);
    }

    [Test]
    public void Reverse_English_ReturnsReversed()
    {
      string arg = "This is a string";
      string res = arg.Reverse();
      Assert.AreEqual("gnirts a si sihT", res);
    }

    [Test]
    public void Reverse_Russian_ReturnsReversed()
    {
      string arg = "Это просто строка";
      string res = arg.Reverse();
      Assert.AreEqual("акортс отсорп отЭ", res);
    }

    //
    // http://stackoverflow.com/a/15111719
    //
    [Test]
    public void Reverse_French_ReturnsReversed()
    {
      string arg = "Les Mise\u0301rablès";
      string res = arg.Reverse();
      Assert.AreEqual("sèlbare\u0301siM seL", res);
    }

    #endregion
  }
}
