using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class ShortGuid_Tests
    {
        [Test]
        public void ShortGuid_ToString_Length22()
        {
            // arrange
            var guid = Guid.NewGuid();
            var shortGuid = new ShortGuid(guid);
            // act
            string result = shortGuid.ToString();
            // assert
            Assert.That(result.Length, Is.EqualTo(22));
        }

        [Test]
        public void ShortGuidDefault_ToString_GeneratesAAAs()
        {
            // arrange
            var guid = default(Guid);
            var shortGuid = new ShortGuid(guid);
            // act
            string result = shortGuid.ToString();
            // assert
            Assert.That(result, Is.EqualTo("AAAAAAAAAAAAAAAAAAAAAA"));
        }

        [Test]
        public void Parse_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ShortGuid.Parse(null));
        }

        [Test]
        public void Parse_LengthNot22_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => ShortGuid.Parse("12345"));
        }

        [Test]
        public void Parse_InvalidChars_Throws()
        {
            Assert.Throws<FormatException>(() => ShortGuid.Parse("!@#$%67890123456789012"));
        }

        [Test]
        public void ToString_Parse_Equals()
        {
            // arrange
            var arg = Guid.NewGuid();
            string shortGuidString = new ShortGuid(arg).ToString();
            // act
            var shortGuid = ShortGuid.Parse(shortGuidString);
            var guid = shortGuid.Guid;
            // assert
            Assert.That(arg, Is.EqualTo(guid));
        }

        [Test]
        public void NewShortGuid_GeneratesValidResult()
        {
            var arg = ShortGuid.NewShortGuid();
            var result = ShortGuid.Parse(arg.ToString());
            Assert.That(arg.Guid, Is.EqualTo(result.Guid));
        }
    }
}
