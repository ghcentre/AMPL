using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class GuardClauses_Tests
    {
        #region Null
        
        [Test]
        public void Null_BothParametersNull_ThrowsParamNameNull()
        {
            string arg = null;

            try
            {
                Guard.Against.Null(arg, null, null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.That(exception.ParamName, Is.Null);
            }
        }

        [Test]
        public void Null_ParamNameNotNull_ThrowsParamNameNotNull()
        {
            string arg = null;

            try
            {
                Guard.Against.Null(arg, nameof(arg));
            }
            catch (ArgumentNullException exception)
            {
                Assert.That(exception.ParamName, Is.EqualTo("arg"));
            }
        }

        [Test]
        public void Null_ParamNameNullCustomMessage_ThrowsParamNameNullCustomMessage()
        {
            string arg = null;

            try
            {
                Guard.Against.Null(arg, null, "custom message");
            }
            catch (ArgumentNullException exception)
            {
                Assert.That(exception.ParamName, Is.Null);
                Assert.That(exception.Message, Is.EqualTo("custom message"));
            }
        }

        [Test]
        public void Null_ParamNameNotNullCustomMessage_ThrowsParamNameNotNullCustomMessage()
        {
            string arg = null;

            try
            {
                Guard.Against.Null(arg, nameof(arg), "custom message");
            }
            catch (ArgumentNullException exception)
            {
                Assert.That(exception.ParamName, Is.EqualTo("arg"));
                Assert.That(exception.Message, Does.StartWith("custom message"));
                Assert.That(exception.Message, Does.Contain("'arg'"));
            }
        }

        #endregion

        #region NullOrEmpty

        [TestCase(null, null)]
        [TestCase(null, "message")]
        [TestCase("arg", null)]
        [TestCase("arg", "message")]
        public void NullOrEmpty_Null_ThrowsArgumentNullException(string argName, string message)
        {
            string arg = null;

            Assert.Throws<ArgumentNullException>(() => Guard.Against.NullOrEmpty(arg, argName, message));
        }

        [TestCase(null, null)]
        [TestCase(null, "message")]
        [TestCase("arg", null)]
        [TestCase("arg", "message")]
        public void NullOrEmpty_Empty_ThrowsArgumentException(string argName, string message)
        {
            string arg = "";

            Assert.Throws<ArgumentException>(() => Guard.Against.NullOrEmpty(arg, argName, message));
        }

        #endregion
    }
}
