using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.EnumExtensions
{
  [TestClass]
  public class EnumExtensions_Test
  {
    public enum TestEnum
    {
      [Display(Name = "Первый", Description = "Первый элемент")]
      One,

      [Display()]
      Two = 2,
      Three
    }

    [TestMethod]
    public void DisplayName_NonEmptyDisplayAttribute_ReturnsExactName()
    {
      var one = TestEnum.One;

      string displayName = one.GetDisplayName();

      Assert.AreEqual("Первый", displayName);
    }

    [TestMethod]
    public void DisplayDescription_NonEmptyDisplayAttribute_ReturnsExactName()
    {
      var one = TestEnum.One;

      string displayName = one.GetDisplayDescription();

      Assert.AreEqual("Первый элемент", displayName);
    }

    [TestMethod]
    public void DisplayName_EmptyDisplayAttribute_ReturnsNull()
    {
      var two = TestEnum.Two;

      string displayName = two.GetDisplayName();

      Assert.IsNull(displayName);
    }

    [TestMethod]
    public void DisplayName_NoDisplayAttribute_ReturnsNull()
    {
      var three = TestEnum.Three;

      string displayName = three.GetDisplayName();

      Assert.IsNull(displayName);
    }
  }
}
