using NUnit.Framework;
using System;
using Ampl.Core;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class CompactGuid_Tests
    {
        [Test]
        public void AnyGuid_ToCompactString_Length22()
        {
            var guid = Guid.NewGuid();
            var stringValue = guid.ToCompactString();

            Assert.That(stringValue.Length, Is.EqualTo(22));
        }

        [Test]
        public void DefaultGuid_ToCompactString_GeneratesAAAs()
        {
            var guid = default(Guid);
            var stringValue = guid.ToCompactString();

            Assert.That(stringValue, Is.EqualTo("AAAAAAAAAAAAAAAAAAAAAA"));
        }

        [Test]
        public void CompactGuidParse_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CompactGuid.Parse(null));
        }

        [Test]
        public void CompactGuidParse_LengthNot22_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => CompactGuid.Parse("12345"));
        }

        [Test]
        public void CompactGuidParse_InvalidChars_Throws()
        {
            Assert.Throws<FormatException>(() => CompactGuid.Parse("!@#$%67890123456789012"));
        }

        [Test]
        public void ToCompactString_CompactGuidParse_Equals()
        {
            var expected = Guid.NewGuid();
            var compactString = expected.ToCompactString();

            var actual = CompactGuid.Parse(compactString);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
