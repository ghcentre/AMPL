using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.NullCheckExtensions
{
  [TestClass]
  public class DoTest
  {
    [TestMethod]
    public void Do_ObjectNotNull()
    {
      string s = "This is a string";
      int i = 0;
      s.Do(x => i = 1);
      Assert.AreEqual(1, i);
    }

    [TestMethod]
    public void Do_ObjectNull()
    {
      string s = null;
      int i = 0;
      s.Do(x => i = 1);
      Assert.AreEqual(0, i);
    }

    [TestMethod]
    public void Do_NullableHasValue()
    {
      int? nullableInt = 5;
      int i = 0;
      nullableInt.Do(x => i = 1);
      Assert.AreEqual(1, i);
    }

    [TestMethod]
    public void Do_NullableHas_No_Value()
    {
      int? nullableInt = null;
      int i = 0;
      nullableInt.Do(x => i = 1);
      Assert.AreEqual(0, i);
    }
  }
}
