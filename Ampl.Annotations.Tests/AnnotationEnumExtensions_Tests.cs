using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using Ampl.System;

namespace Ampl.Annotations.Tests
{
  [TestFixture]
  public class AnnotationEnumExtensions_Tests
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

    [Test]
    public void DisplayName_NonEmptyDisplayAttribute_ReturnsExactName()
    {
      var one = TestEnum.One;
      string displayName = one.GetDisplayName();
      Assert.AreEqual("Первый", displayName);
    }

    [Test]
    public void DisplayDescription_NonEmptyDisplayAttribute_ReturnsExactName()
    {
      var one = TestEnum.One;
      string displayName = one.GetDisplayDescription();
      Assert.AreEqual("Первый элемент", displayName);
    }

    [Test]
    public void DisplayName_EmptyDisplayAttribute_ReturnsNull()
    {
      var two = TestEnum.Two;
      string displayName = two.GetDisplayName();
      Assert.IsNull(displayName);
    }

    [Test]
    public void DisplayName_NoDisplayAttribute_ReturnsNull()
    {
      var three = TestEnum.Three;
      string displayName = three.GetDisplayName();
      Assert.IsNull(displayName);
    }

    [Test]
    public void DisplayName_AttributeWithResource_ReturnsActualString()
    {
      var four = TestEnum.Four;
      string displayName = four.GetDisplayName();
      Assert.AreEqual("String value", displayName);
    }

    [Test]
    public void DisplayDescription_AttributeWithResource_ReturnsActualString()
    {
      var four = TestEnum.Four;
      string displayDescription = four.GetDisplayDescription();
      Assert.AreEqual("String2 value", displayDescription);
    }
  }
}
