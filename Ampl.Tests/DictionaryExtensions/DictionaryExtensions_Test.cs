using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ampl.System;

namespace Ampl.System.Tests
{
  [TestClass]
  public class DictionaryExtensions_Test
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IncludeObjectProperties_ThrowsIfNull1()
    {
      Dictionary<string, object> dic = null;
      var anonType = new { Name = "Maria", Surname = "Breon", Age = 45, Weight = 80.2 };
      dic.IncludeObjectProperties(anonType);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IncludeObjectProperties_ThrowsIfNull2()
    {
      Dictionary<string, object> dic = new Dictionary<string, object>();
      object anonType = null;
      dic.IncludeObjectProperties(anonType);
    }

    [TestMethod]
    public void IncludeObjectProperties_AddToEmpty()
    {
      var anonType = new { Name = "Maria", Surname = "Breon", Age = 45, Weight = 80.2 };
      var dic = new Dictionary<string, object>().IncludeObjectProperties(anonType);
      Assert.IsTrue((string)dic["Name"] == anonType.Name);
      Assert.IsTrue((double)dic["Weight"] == anonType.Weight);
    }

    [TestMethod]
    public void IncludeObjectProperties_AddToExisting_Replace()
    {
      var dic = new Dictionary<string, object>() { { "Name", "Anna" } };
      Assert.IsTrue((string)dic["Name"] == "Anna");
      Assert.IsFalse(dic.ContainsKey("Weight"));

      var anonType = new { Name = "Maria", Surname = "Breon", Age = 45, Weight = 80.2 };
      dic.IncludeObjectProperties(anonType);
      Assert.IsTrue((string)dic["Name"] == anonType.Name);
      Assert.IsTrue((double)dic["Weight"] == anonType.Weight);
    }
  }
}
