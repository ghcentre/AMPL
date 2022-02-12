using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class Enums_Tests
    {
        public enum TestEnum
        {
            One,
            Two = 2,
            Three,
            Four,
        }


        #region Parse

        [Test]
        public void Parse_Existing_ReturnsValue()
        {
            var arg = "Two";
            var result = Enums.Parse<TestEnum>(arg);
            Assert.That(result, Is.EqualTo(TestEnum.Two));
        }

        [Test]
        public void Parse_LowercaseCaseSensitive_Throws()
        {
            var arg = "two";
            Assert.Throws<ArgumentException>(() => Enums.Parse<TestEnum>(arg));
        }

        [Test]
        public void Parse_LowercaseIgnoreCase_Returns()
        {
            var arg = "two";
            var result = Enums.Parse<TestEnum>(arg, true);
            Assert.That(result, Is.EqualTo(TestEnum.Two));
        }

        [Test]
        public void Parse_NonExistent_Throws()
        {
            var arg = "abc";
            Assert.Throws<ArgumentException>(() => Enums.Parse<TestEnum>(arg));
        }

        #endregion


        #region ParseAsNullable

        [Test]
        public void ParseAsNullable_Existing_ReturnsNotNull()
        {
            var arg = "Two";

            var result = Enums.ParseAsNullable<TestEnum>(arg);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(TestEnum.Two));
        }

        [Test]
        public void ParseAsNullable_NonExisting_ReturnsNull()
        {
            string arg = "NonExistentEnumValue";
            var result = Enums.ParseAsNullable<TestEnum>(arg, true);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ParseAsNullable_Lowercase_ReturnsNull()
        {
            string arg = "two";
            var result = Enums.ParseAsNullable<TestEnum>(arg);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ParseAsNullable_LowercaseWithIgnoreCaseArgument_ReturnsNotNull()
        {
            string arg = "two";
            var result = Enums.ParseAsNullable<TestEnum>(arg, true);
            Assert.That(result, Is.EqualTo(TestEnum.Two));
        }

        #endregion
    }
}
