using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class EnumerableExtensions_Tests
    {
        #region In

        [Test]
        public void In_NullThis_ReturnsFalse()
        {
            string? arg = null;
            bool result = arg.In("One", "Two", "Three");
            Assert.That(result, Is.False);
        }

        [Test]
        public void In_EmptyArgList_ReturnsFalse()
        {
            string arg = "String";
            bool result = arg.In();
            Assert.That(result, Is.False);
        }

        [Test]
        public void In_NotFound_ReturnsFalse()
        {
            int arg = 456;
            int[] checks = new[] { 1, 2, 3, 4, 5 };
            var result = arg.In(checks);
            Assert.That(result, Is.False);
        }

        [Test]
        public void In_NullEnumerable_ReturnsFalse()
        {
            double arg = 123.45;
            double[]? checks = null;
            var result = arg.In(checks!);
            Assert.That(result, Is.False);
        }

        [Test]
        public void In_ArgsFound_ReturnsTrue()
        {
            int arg = 456;
            bool result = arg.In(0, 1, 1, 1, 456, 2, -100);
            Assert.That(result, Is.True);
        }

        [Test]
        public void In_EnumerableFound_ReturnsTrue()
        {
            double arg = 345.22;
            double[] checks = new[] { 0, -300, 45, 345.22 };
            bool result = arg.In(checks);
            Assert.That(result, Is.True);
        }

        #endregion

        #region JoinWith

        [Test]
        public void JoinWith_Strings_ResultEqualsStringJoin()
        {
            string[] arg = new[] { "One", "Two", "Three" };
            string result = arg.JoinWith(", ");
            string matchResult = string.Join(", ", (IEnumerable<string>)arg);
            Assert.AreEqual(result, matchResult);
        }

        [Test]
        public void JoinWith_Objects_ResultEqualsStringJoin()
        {
            object[] arg = new[] { "The string", 2, DateTime.Now, (object)7.0 };
            string result = arg.JoinWith(", ");
            string matchResult = string.Join(", ", arg);
            Assert.AreEqual(result, matchResult);
        }

        #endregion

        #region Traverse

        [Test]
        public void Traverse_NullThis_ReturnsEmptySequence()
        {
            object? arg = null;
            var result = arg.Traverse(x => x);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Traverse_NullNext_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => "hello".Traverse(null!).ToList());
        }

        [Test]
        public void Traverse_InnerProps_ReturnsExpected()
        {
            var exception = new InvalidOperationException(
                "Could not find specified directory.",
                new System.IO.DirectoryNotFoundException(
                    "Directory not found.",
                    new UnauthorizedAccessException("Access denied")));

            var result = exception.Traverse<Exception>(x => x?.InnerException).Select(x => x.Message).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo(exception.Message));
            Assert.That(result[1], Is.EqualTo(exception.InnerException?.Message));
            Assert.That(result[2], Is.EqualTo(exception.InnerException?.InnerException?.Message));
        }

        #endregion
    }
}
