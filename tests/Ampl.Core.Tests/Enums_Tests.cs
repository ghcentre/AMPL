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

        [Test]
        public void ParseValue_Existing_ReturnsValue()
        {
            var arg = "Two";
            var result = Enums.Parse<TestEnum>(arg);
            Assert.That(result, Is.EqualTo(TestEnum.Two));
        }

        [Test]
        public void ParseValue_LowercaseCaseSensitive_Throws()
        {
            // arrange
            var arg = "two";
            // act-assert
            Assert.Throws<ArgumentException>(() => Enums.Parse<TestEnum>(arg));
        }

        [Test]
        public void ParseValue_LowercaseIgnoreCase_Returns()
        {
            var arg = "two";
            var result =  Enums.Parse<TestEnum>(arg, true);
            Assert.That(result, Is.EqualTo(TestEnum.Two));
        }

        [Test]
        public void ParseValue_NonExistent_Throws()
        {
            var arg = "abc";
            Assert.Throws<ArgumentException>(() => Enums.Parse<TestEnum>(arg));
        }

        [Test]
        public void ParseAsNullable_Existing_ReturnsNotNull()
        {
            // arrange
            var arg = "Two";
            // act
            var result = Enums.ParseAsNullable<TestEnum>(arg);
            // assert
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
    }
}
