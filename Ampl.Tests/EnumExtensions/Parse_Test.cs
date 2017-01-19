using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.EnumExtensions
{
  [TestClass]
  public class Parse_Test
  {
    public enum TestEnum
    {
      [Display(Name = "Первый", Description = "Первый элемент")]
      One,

      [Display()]
      Two = 2,

      Three,

      [Display(ResourceType = typeof(Properties.Resources), Name = "String1", Description = "String2")]
      Four,
    }

    [TestMethod]
    public void ParseValue_Existing_Returns()
    {
      string arg = "Two";

      var result = TestEnum.One.ParseValue(arg);

      Assert.AreEqual(TestEnum.Two, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseValue_Lowercase_ThrowsArgumentException()
    {
      string arg = "two";

      var result = TestEnum.One.ParseValue(arg);

      // ASSERT: exception
    }

    [TestMethod]
    public void ParseValue_LowercaseWithIgnoreCaseArgument_Returns()
    {
      string arg = "two";

      var result = TestEnum.One.ParseValue(arg, true);

      Assert.AreEqual(TestEnum.Two, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseValue_NonExistent_ThrowsArgumentException()
    {
      string arg = "NonExistentEnumValue";

      var result = TestEnum.Three.ParseValue(arg, true);

      // ASSERT: exception
    }
  }
}
