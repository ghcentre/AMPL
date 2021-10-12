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

        [Test]
        public void ToCompactString_SequentalGuids_ProducesSequentalStrings()
        {
            var g1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var s1 = g1.ToCompactString();

            var g2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var s2 = g2.ToCompactString();

            var g3 = Guid.Parse("00000000-0000-0000-0001-000000000000");
            var s3 = g3.ToCompactString();

            var actual1 = StringComparer.Ordinal.Compare(s1, s2);
            var actual2 = StringComparer.Ordinal.Compare(s2, s3);
            var actual3 = StringComparer.Ordinal.Compare(s1, s3);

            Assert.That(actual1, Is.LessThan(0));
            Assert.That(actual2, Is.LessThan(0));
            Assert.That(actual3, Is.LessThan(0));
        }
    }
}
