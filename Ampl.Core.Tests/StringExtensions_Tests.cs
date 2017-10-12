using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

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
  }
}
