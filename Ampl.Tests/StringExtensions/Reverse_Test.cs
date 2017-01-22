using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.System.Tests
{
  [TestClass]
  public class Reverse_Test
  {
    [TestMethod]
    public void Reverse_Null_ReturnsNull()
    {
      string arg = null;

      string res = arg.Reverse();

      Assert.IsNull(res);
    }

    [TestMethod]
    public void Reverse_Empty_ReturnsEmpty()
    {
      string arg = string.Empty;

      string res = arg.Reverse();

      Assert.AreEqual(string.Empty, res);
    }

    [TestMethod]
    public void Reverse_English_ReturnsReversed()
    {
      string arg = "This is a string";

      string res = arg.Reverse();

      Assert.AreEqual("gnirts a si sihT", res);
    }

    [TestMethod]
    public void Reverse_Russian_ReturnsReversed()
    {
      string arg = "Это просто строка";

      string res = arg.Reverse();

      Assert.AreEqual("акортс отсорп отЭ", res);
    }

    //
    // http://stackoverflow.com/a/15111719
    //
    [TestMethod]
    public void Reverse_French_ReturnsReversed()
    {
      string arg = "Les Mise\u0301rablès";

      string res = arg.Reverse();

      Assert.AreEqual("sèlbare\u0301siM seL", res);
    }
  }
}
