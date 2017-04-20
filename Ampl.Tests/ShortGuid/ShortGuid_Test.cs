using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.ShortGuid_Tests
{

  [TestClass]
  public class ShortGuid_Test
  {
    [TestMethod]
    public void ShortGuid_ToString_Length22()
    {
      var guid = Guid.NewGuid();
      var shortGuid = new ShortGuid(guid);

      string result = shortGuid.ToString();

      Assert.AreEqual(22, result.Length);
    }

    [TestMethod]
    public void ShortGuidDefault_ToString_GeneratesAAAs()
    {
      var guid = default(Guid);
      var shortGuid = new ShortGuid(guid);

      string result = shortGuid.ToString();

      Assert.AreEqual("AAAAAAAAAAAAAAAAAAAAAA", result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Parse_null_throws()
    {
      string argument = null;

      ShortGuid result = ShortGuid.Parse(argument);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void Parse_LengthNot22_throws()
    {
      string argument = "string";

      ShortGuid result = ShortGuid.Parse(argument);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void Parse_InvalidChars_Throws()
    {
      string argument = "!@#$%67890123456789012";

      ShortGuid result = ShortGuid.Parse(argument);
    }

    [TestMethod]
    public void ToString_Parse_Equals()
    {
      var guid = Guid.NewGuid();
      string shortGuidString = new ShortGuid(guid).ToString();

      var shortGuid = ShortGuid.Parse(shortGuidString);

      Assert.AreEqual(guid, shortGuid.Guid);
    }

  }
}
