using NUnit.Framework;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class StringLiteral_Tests
    {
        [Test]
        public void ToString_Escapes()
        {
            var arg = "\a\b\f\n\r\t\v\"\\";

            var literal = new StringLiteral(arg);
            var result = literal.ToString();

            Assert.That(result, Is.EqualTo("\"\\a\\b\\f\\n\\r\\t\\v\\\"\\\\\""));
        }

        [Test]
        public void CorrectLiteral_Parses()
        {
            var arg = @"""a\r\nbc""other";

            var result = StringLiteral.TryParse(arg, out var literal);

            Assert.That(result, Is.True);
            Assert.That(literal!.Value, Is.EqualTo("a\r\nbc"));
        }

        [TestCase("abc", Description = "No starting quote")]
        [TestCase("abc\"def", Description = "No starting quote")]
        [TestCase("\"abcdef", Description = "No ending quote")]
        public void IncorrectLiteral_DoesNotParse(string arg)
        {
            var result = StringLiteral.TryParse(arg, out var literal);

            Assert.That(result, Is.False);
            Assert.That(literal, Is.Null);
        }
    }
}
