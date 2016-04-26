using System;
using System.Collections.Generic;
using System.Linq;
using Ampl.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests.EnumerableExtensions
{
  [TestClass]
  public class JoinWithTest
  {
    [TestMethod]
    public void JoinWith_Strings()
    {
      string[] arg = new[] { "One", "Two", "Three" };
      string result = arg.JoinWith(", ");
      string matchResult = string.Join(", ", (IEnumerable<string>)arg);
      Assert.AreEqual(result, matchResult);
    }

    [TestMethod]
    public void JoinWith_Objects()
    {
      object[] arg = new[] { (object)"The string", (object)2, (object)DateTime.Now, (object)7.0 };
      string result = arg.JoinWith(", ");
      string matchResult = string.Join(", ", arg);
      Assert.AreEqual(result, matchResult);
    }

  }
}
