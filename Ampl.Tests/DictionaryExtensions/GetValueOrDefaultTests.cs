using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.DictionaryExtensions
{

  [TestClass]
  public class GetValueOrDefaultTests
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Null_dictionary_throws()
    {
      Dictionary<string, string> dictionary = null;
      string key = "TestKey";
      var result = dictionary.GetValueOrDefault(key);
    }

    private Dictionary<int, string> _errorMessages = new Dictionary<int, string>() {
      { 0, "The operation completed successfully" },
      { 1, "Incorrect function" },
      { 2, "File not found" },
      { 3, "Path not found" },
      { 4, "Cannot open file" },
      { 5, "Access denied" }
    };

    [TestMethod]
    public void Key_exists()
    {
      int key = 3;
      var result = _errorMessages.GetValueOrDefault(key);
      Assert.AreEqual("Path not found", result);
    }

    [TestMethod]
    public void Key_not_exists()
    {
      int key = 1000;
      var result = _errorMessages.GetValueOrDefault(key);
      Assert.IsNull(result);
    }

    [TestMethod]
    public void Key_not_exists_value_types()
    {
      var dictionary = new Dictionary<string, int>() {
        ["one"] = 1,
        ["two"] = 2,
        ["three"] = 3
      };
      string key = "four";
      int result = dictionary.GetValueOrDefault(key);
      Assert.AreEqual(default(int), result);
    }
  }
}
