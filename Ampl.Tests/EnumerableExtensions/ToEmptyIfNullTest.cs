using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;
using System.Linq;

namespace Ampl.Tests.EnumerableExtensions
{
  [TestClass]
  public class ToEmptyIfNullTest
  {
    [TestMethod]
    public void ToNullIfEmpty_NotNull()
    {
      IEnumerable<string> argument = new[] { "one", "two", "three" };
      var result = argument.ToEmptyIfNull();
      Assert.AreEqual(argument, result);
    }

    [TestMethod]
    public void ToNullIfEmpty_Null()
    {
      IEnumerable<string> argument = null;
      var result = argument.ToEmptyIfNull();
      Assert.AreNotEqual(null, result);
      Assert.AreEqual(0, result.ToList().Count);
    }
  }
}
