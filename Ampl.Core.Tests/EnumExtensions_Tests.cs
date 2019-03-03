using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

namespace Ampl.Core.Tests
{
  [TestFixture]
  public class EnumExtensions_Tests
  {
    public enum TestEnum
    {
      One,
      Two = 2,
      Three,
      Four,
    }

    [Test]
    public void ParseValue_Existing_ReturnsValue()
    {
      var arg = "Two";
      var result = TestEnum.One.ParseValue(arg);
      Assert.That(result, Is.EqualTo(TestEnum.Two));
    }

    [Test]
    public void ParseValue_LowercaseCaseSensitive_Throws()
    {
      // arrange
      var arg = "two";
      // act-assert
      Assert.Throws<ArgumentException>(() => TestEnum.One.ParseValue(arg));
    }

    [Test]
    public void ParseValue_LowercaseIgnoreCase_Returns()
    {
      var arg = "two";
      var result = TestEnum.One.ParseValue(arg, true);
      Assert.That(result, Is.EqualTo(TestEnum.Two));
    }

    [Test]
    public void ParseValue_NonExistent_Throws()
    {
      var arg = "abc";
      Assert.Throws<ArgumentException>(() => TestEnum.One.ParseValue(arg));
    }

    [Test]
    public void ParseValue_AnyMemberSameArg_ReturnsSame()
    {
      // arrange
      var arg = "Three";
      // act
      var result1 = TestEnum.One.ParseValue(arg);
      var result2 = TestEnum.Three.ParseValue(arg);
      // assert
      Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void ParseAsNullable_Existing_ReturnsNotNull()
    {
      // arrange
      var arg = "Two";
      // act
      var result = TestEnum.One.ParseAsNullable(arg);
      // assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.EqualTo(TestEnum.Two));
    }

    [Test]
    public void ParseAsNullable_NonExisting_ReturnsNull()
    {
      string arg = "NonExistentEnumValue";
      var result = TestEnum.Three.ParseAsNullable(arg, true);
      Assert.That(result, Is.Null);
    }

    [Test]
    public void ParseAsNullable_Lowercase_ReturnsNull()
    {
      string arg = "two";
      var result = TestEnum.One.ParseAsNullable(arg);
      Assert.That(result, Is.Null);
    }

    [Test]
    public void ParseAsNullable_LowercaseWithIgnoreCaseArgument_ReturnsNotNull()
    {
      string arg = "two";
      var result = TestEnum.One.ParseAsNullable(arg, true);
      Assert.That(result, Is.EqualTo(TestEnum.Two));
    }
  }
}
