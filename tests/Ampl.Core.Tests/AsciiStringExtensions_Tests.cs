using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class AsciiStringExtensions_Tests
    {
        #region FoldToAscii

        [Test]
        public void FoldToAscii_Null_Throws()
        {
            var arg = (string)null!;
            Assert.Throws<ArgumentNullException>(() => arg.FoldToAscii());
        }

        [Test]
        public void FoldToAscii_Empty_ReturnsEmpty()
        {
            var arg = string.Empty;
            var result = arg.FoldToAscii();
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [TestCase("hello", "hello")]
        [TestCase("Hello", "Hello")]
        [TestCase("123", "123")]
        [TestCase("Wüsthof Straße", "Wusthof Strasse")]
        [TestCase("ÁÂÃÄÅ Ç ÈÉ àáâãäå èéêë ìíîï òóôõ", "AAAAA C EE aaaaaa eeee iiii oooo")]
        [TestCase("Пятый-подъезд-Ёлкина", "Пятый-подъезд-Ёлкина")]
        public void FoldToAscii_Expected(string arg, string expected)
        {
            var result = arg.FoldToAscii();

            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion
    }
}
