using System;
using Ampl.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests.EnumerableExtensions
{
  [TestClass]
  public class InTest
  {
    [TestMethod]
    public void In_Null_Source()
    {
      string arg = null;
      bool result = arg.In("One", "Two", "Three");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void In_Null_Checks()
    {
      string arg = "String";
      bool result = arg.In();
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void In_Enumerable()
    {
      int arg = 456;
      int[] checks = new[] { 1, 2, 3, 4, 5 };
      Assert.IsFalse(arg.In(checks));
    }

    [TestMethod]
    public void In_Enumerable_Null()
    {
      double arg = 123.45;
      double[] checks = null;
      Assert.IsFalse(arg.In(checks));
    }
  }
}
