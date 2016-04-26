using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests
{
  [TestClass]
  public class EmptyStringTest
  {
    [TestMethod]
    public void EmptyStrings_Same()
    {
      string s1 = "";
      string s2 = "";
      Assert.AreSame(s1, s2);
    }

    [TestMethod]
    public void EmptyStrings_Same2()
    {
      string s1 = string.Empty;
      string s2 = "";
      Assert.AreSame(s1, s2);
    }

    [TestMethod]
    public void EmptyString_Equal()
    {
      string s1 = "";
      string s2 = "";
      Assert.AreEqual(s1, s2);
    }

    [TestMethod]
    public void EmptyString_Equal2()
    {
      string s1 = string.Empty;
      string s2 = "";
      Assert.AreEqual(s1, s2);
    }
  }
}
