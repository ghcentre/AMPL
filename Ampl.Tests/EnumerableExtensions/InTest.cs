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

    [TestMethod]
    public void In_Args()
    {
      int arg = 456;
      bool result = arg.In(0, 1, 1, 1, 456, 2, -100);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void In_Enumerable2()
    {
      double arg = 345.22;
      double[] checks = new[] { 0, -300, 45, 345.22 };
      bool result = arg.In(checks);
      Assert.IsTrue(result);
    }
  }
}
