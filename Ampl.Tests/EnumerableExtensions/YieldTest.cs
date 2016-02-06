using System;
using System.Linq;
using Ampl.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests.EnumerableExtensions
{
  [TestClass]
  public class YieldTest
  {
    [TestMethod]
    public void Yield_Null()
    {
      string arg = null;
      var result = arg.Yield();
      Assert.IsNotNull(result);
      Assert.AreEqual(1, result.Count());
      Assert.AreEqual(arg, result.First());
    }

    [TestMethod]
    public void Yield_NotNull()
    {
      string arg = "string";
      var result = arg.Yield();
      Assert.AreEqual(arg, result.First());
    }

  }
}
