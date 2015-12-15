using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.StringExtensions
{
  [TestClass]
  public class NullIfWhitespace_Test
  {
    [TestMethod]
    public void Null_returns_null()
    {
      string arg = null;
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == null);
    }

    [TestMethod]
    public void Empty_returns_null()
    {
      string arg = string.Empty;
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == null);
    }

    [TestMethod]
    public void Spaces_returns_null()
    {
      string arg = " \t\r\n";
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == null);
    }

    [TestMethod]
    public void Nonspace_returns_argument()
    {
      string arg = "\tThis\r\nis a test\t string";
      string result = arg.ToNullIfWhiteSpace();
      Assert.IsTrue(result == arg);
    }
  }
}
