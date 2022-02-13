using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Ampl.Annotations.Tests
{
    [TestFixture]
    public class AnnotationEnumExtensions_Tests
    {
        #region Test Enum

        public enum TestEnum
        {
            [Display(Name = "Первый", Description = "Первый элемент")]
            One,

            [Display(ShortName = "2й")]
            Two = 2,

            Three,

            [Display(ResourceType = typeof(Resources), Name = "String1", Description = "String2", ShortName = "String3")]
            Four,
        }

        #endregion

        #region GetDisplayName

        [Test]
        public void DisplayName_NonEmptyDisplayAttribute_ReturnsExactName()
        {
            var one = TestEnum.One;
            string? displayName = one.GetDisplayName();
            Assert.AreEqual("Первый", displayName);
        }

        [Test]
        public void DisplayName_EmptyDisplayAttribute_ReturnsNull()
        {
            var two = TestEnum.Two;
            string? displayName = two.GetDisplayName();
            Assert.IsNull(displayName);
        }

        [Test]
        public void DisplayName_NoDisplayAttribute_ReturnsNull()
        {
            var three = TestEnum.Three;
            string? displayName = three.GetDisplayName();
            Assert.IsNull(displayName);
        }

        [Test]
        public void DisplayName_AttributeWithResource_ReturnsActualString()
        {
            var four = TestEnum.Four;
            string? displayName = four.GetDisplayName();
            Assert.AreEqual("String value", displayName);
        }

        #endregion

        #region GetDisplayShortName

        [Test]
        public void DisplayShortName_NonEmptyDisplayAttribute_ReturnsExactName()
        {
            var one = TestEnum.Two;
            string? displayShortName = one.GetDisplayShortName();
            Assert.AreEqual("2й", displayShortName);
        }

        [Test]
        public void DisplayShortName_AttributeWithResource_ReturnsActualString()
        {
            var four = TestEnum.Four;
            string? displayShortName = four.GetDisplayShortName();
            Assert.AreEqual("String3 value", displayShortName);
        }

        #endregion

        #region GetDisplayDescription

        [Test]
        public void DisplayDescription_NonEmptyDisplayAttribute_ReturnsExactName()
        {
            var one = TestEnum.One;
            string? displayName = one.GetDisplayDescription();
            Assert.AreEqual("Первый элемент", displayName);
        }

        [Test]
        public void DisplayDescription_AttributeWithResource_ReturnsActualString()
        {
            var four = TestEnum.Four;
            string? displayDescription = four.GetDisplayDescription();
            Assert.AreEqual("String2 value", displayDescription);
        }

        #endregion    
    }
}
