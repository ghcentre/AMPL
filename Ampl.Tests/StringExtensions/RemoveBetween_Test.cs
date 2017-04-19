using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.System.Tests
{
  [TestClass]
  public class RemoveBetween_Test
  {
    [TestMethod]
    public void RemoveBetween_Source_Null_returns_null()
    {
      string argument = null;
      string result = argument.RemoveBetween("start", "end");
      Assert.IsNull(result);
    }

    [TestMethod]
    public void RemoveBetween_All_args_Removes_between_start_end()
    {
      string argument = "This is a test string";
      string result1 = argument.RemoveBetween("is", "test");
      string result2 = argument.RemoveBetween("iS", "tESt", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("Th string", result1);
      Assert.AreEqual("Th string", result2);
    }

    [TestMethod]
    public void RemoveBetween_Start_null_Removes_from_start_of_string()
    {
      string argument = "This is a test string";
      string result1 = argument.RemoveBetween(null, "test");
      string result2 = argument.RemoveBetween(null, "Test", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual(" string", result1);
      Assert.AreEqual(" string", result2);
    }

    [TestMethod]
    public void RemoveBetween_End_null_Removes_to_end_of_string()
    {
      string argument = "This is a test string";
      string result1 = argument.RemoveBetween("test", null);
      string result2 = argument.RemoveBetween("tEST", null, StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("This is a ", result1);
      Assert.AreEqual("This is a ", result2);
    }

    [TestMethod]
    public void RemoveBetween_End_before_and_after_start_Removes_from_start_to_second_end()
    {
      string argument = "This string is a test string, English characters only.";
      string result1 = argument.RemoveBetween("is a", "ing");
      string result2 = argument.RemoveBetween("is A", "ing", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("This string , English characters only.", result1);
      Assert.AreEqual("This string , English characters only.", result2);
    }

    [TestMethod]
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

    [TestMethod]
    public void RemoveBetween_Start_end_null_returns_original()
    {
      string argument = "This is a test string.";
      string result = argument.RemoveBetween(null,null);
      Assert.AreEqual(argument, result);
    }

    [TestMethod]
    public void RemoveBetween_Start_end_notnull_removes_all()
    {
      string argument1 = @"This is a <a href=""some"">test</a> string.";
      string argument2 = @"1st Begin Test One end 2nd bEgin teSt twO End 3rd begin";
      string result1 = argument1.RemoveBetween("<", ">");
      string result2 = argument2.RemoveBetween("begin", "end", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual("This is a test string.", result1);
      Assert.AreEqual("1st  2nd  3rd begin", result2);
    }
  }
}
