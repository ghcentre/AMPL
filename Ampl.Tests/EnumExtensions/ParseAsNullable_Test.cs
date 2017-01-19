using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.EnumExtensions
{
  [TestClass]
  public class ParseAsNullable_Test
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
    public void ParseAsNullable_Existing_ReturnsNotNull()
    {
      string arg = "Two";

      var result = TestEnum.One.ParseAsNullable(arg);

      Assert.IsTrue(result.HasValue);
      Assert.AreEqual(TestEnum.Two, result.Value);
    }

    [TestMethod]
    public void ParseAsNullable_NonExistent_ReturnsNull()
    {
      string arg = "NonExistentEnumValue";

      var result = TestEnum.Three.ParseAsNullable(arg, true);

      Assert.IsFalse(result.HasValue);
    }

    [TestMethod]
    public void ParseAsNullable_Lowercase_ReturnsNull()
    {
      string arg = "two";

      var result = TestEnum.One.ParseAsNullable(arg);

      Assert.IsFalse(result.HasValue);
    }

    [TestMethod]
    public void ParseAsNullable_LowercaseWithIgnoreCaseArgument_Returns()
    {
      string arg = "two";

      var result = TestEnum.One.ParseAsNullable(arg, true);

      Assert.AreEqual(TestEnum.Two, result.Value);
    }
  }
}
