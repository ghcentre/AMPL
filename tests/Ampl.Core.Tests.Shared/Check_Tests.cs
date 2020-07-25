using NUnit.Framework;
using System;

namespace Ampl.Core.Tests.Shared
{
    [TestFixture]
    public class Check_Tests
    {
        #region NotNull

        [Test]
        public void Check_NotNull_NullArg_Throws()
        {
            // arrange
            string arg = null;
            // act-assert
            Assert.Throws<ArgumentNullException>(() => Check.NotNull(arg));
        }

        [Test]
        public void Check_NotNull_NotNullArg_ReturnsSameReference()
        {
            string arg = "Not null string";
            string result = Check.NotNull(arg);
            Assert.That(result, Is.SameAs(arg));
        }

        [Test]
        public void Check_NotNull_NullArgWithParamName_ThrowsWithParamNameInMessage()
        {
            // arrange
            string arg = null;
            try
            {
                // act
                Check.NotNull(arg, "ThisIsTheArg");
            }
            catch (ArgumentNullException e)
            {
                // assert
                Assert.That(e.Message, Does.Contain("ThisIsTheArg"));
            }
        }

        #endregion

        #region NotNullOrEmptyString

        [TestCase((string)null, "ArgumentNullException")]
        [TestCase("", "ArgumentException")]
        public void Check_NotNullOrEmptyString_InvalidArg_ThrowsCorrectException(string argument, string exceptionClassName)
        {
            // arrange
            try
            {
                // act
                var result = Check.NotNullOrEmptyString(argument);
            }
            catch (Exception exception)
            {
                var actualClassName = exception.GetType().Name;

                // assert
                Assert.That(actualClassName, Is.EqualTo(exceptionClassName));
            }
        }

        [Test]
        public void Check_NotNullOrEmptyString_NullArgWithParamName_ThrowsWithParamNameInMessage()
        {
            // arrange
            string arg = null;
            try
            {
                // act
                Check.NotNullOrEmptyString(arg, "ThisIsArgumentName");
            }
            catch (Exception e)
            {
                // assert
                Assert.That(e.Message, Does.Contain("ThisIsArgumentName"));
            }
        }

        [Test]
        public void Check_NotNullOrEmptyString_NotNullArg_ReturnsSameReference()
        {
            string arg = "This is some argument";
            string result = Check.NotNullOrEmptyString(arg, "ThisIsArgument");
            Assert.That(result, Is.SameAs(arg));
        }

        #endregion

        #region NotNullOrWhiteSpaceString

        [TestCase((string)null)]
        [TestCase("")]
        [TestCase("  \r \n\t")]
        public void Check_NotNullOrWhiteSpaceString_NullArgWithParamName_ThrowsWithParamNameInMessage(string arg)
        {
            // arrange
            try
            {
                // act
                Check.NotNullOrWhiteSpaceString(arg, "ThisIsArgumentName");
            }
            catch (Exception e)
            {
                // assert
                Assert.That(e.Message, Does.Contain("ThisIsArgumentName"));
            }
        }

        [TestCase((string)null, "ArgumentNullException")]
        [TestCase("",           "ArgumentException")]
        [TestCase("  \r \n\t",  "ArgumentException")]
        public void Check_NotNullOrWhiteSpaceString_InvalidArg_ThrowsCorrectException(string argument, string exceptionClassName)
        {
            // arrange
            try
            {
                // act
                var result = Check.NotNullOrWhiteSpaceString(argument);
            }
            catch (Exception exception)
            {
                var actualClassName = exception.GetType().Name;

                // assert
                Assert.That(actualClassName, Is.EqualTo(exceptionClassName));
            }
        }

        [Test]
        public void Check_NotNullOrWhiteSpaceString_NotNullArg_ReturnsSameReference()
        {
            string arg = "This is some argument";
            string result = Check.NotNullOrWhiteSpaceString(arg, "ThisIsArgument");
            Assert.That(result, Is.SameAs(arg));
        }

        #endregion
    }
}
