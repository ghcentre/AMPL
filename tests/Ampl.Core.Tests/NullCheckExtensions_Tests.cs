﻿using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class NullCheckExtensions_Tests
    {
        public class ViewModel
        {
            public Response Response { get; set; }
        }

        public class Response
        {
            public Error Error { get; set; }
        }

        public class Error
        {
            public Exception Exception { get; set; }
        }

        private ViewModel _model;

        [SetUp]
        public void TestInitialize()
        {
            _model = new ViewModel() {
                Response = new Response() {
                    Error = new Error() {
                        Exception = new Exception("Outer exception", new ArgumentException())
                    }
                }
            };
        }


        #region With

        [Test]
        public void With_NullThis_ReturnsNull()
        {
            // arrange
            string arg = null;
            // act
            string result1 = arg.With(x => x + "string");
            string result2 = arg.With<string, string>(null);
            // assert
            Assert.That(result1, Is.Null);
            Assert.That(result2, Is.Null);
        }

        [Test]
        public void With_NullNullable_ReturnsNull()
        {
            // arrange
            int? arg = null;
            // act
            int? result = arg.With(x => x + 1);
            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void With_NullFunc_Throws()
        {
            // arrange
            string arg = "The string";
            // act-assert
            Assert.Throws<ArgumentNullException>(() => arg.With<string, string>(null));
        }

        [Test]
        public void With_Chaining_ReturnsNested()
        {
            // arrange
            var arg = _model;
            // act
            var result = arg.With(x => x.Response).With(x => x.Error).With(x => x.Exception);
            // assert
            Assert.That(result, Is.SameAs(_model.Response.Error.Exception));
        }

        [Test]
        public void With_ChainingNullInsideChain_ReturnsNull()
        {
            // arrange
            var arg = _model;
            arg.Response.Error = null;
            // act
            var result = arg.With(x => x.Response).With(x => x.Error).With(x => x.Exception);
            // assert
            Assert.That(result, Is.Null);
        }

        #endregion

        #region Do (one func)

        [Test]
        public void Do_NullThis_DoesNothing()
        {
            // arrange
            string arg = null;
            string sideEffect = null;
            // act
            var result = arg.Do(x => sideEffect = "side");
            // assert
            Assert.That(sideEffect, Is.Null);
        }

        [Test]
        public void Do_NullNullable_DoesNothing()
        {
            // arrange
            int? arg = null;
            string sideEffect = null;
            // act
            var result = arg.Do(x => sideEffect = "side");
            // assert
            Assert.That(sideEffect, Is.Null);
        }

        [Test]
        public void Do_NullThis_ReturnsNull()
        {
            // arrange
            string arg = null;
            // act
            var result = arg.Do(x => { });
            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Do_NullAction_Throws()
        {
            // arrange
            string arg = "arg";
            // act-assert
            Assert.Throws<ArgumentNullException>(() => arg.Do(null));
        }

        [Test]
        public void Do_NotNull_DoesSideEffects()
        {
            // arrange
            string arg = "arg";
            string sideEffect = null;
            // act
            var result = arg.Do(x => sideEffect = "side");
            // assert
            Assert.That(sideEffect, Is.EqualTo("side"));
        }

        [Test]
        public void Do_NotNull_ReturnsSameReference()
        {
            // arrange
            string arg = "arg";
            // act
            var result = arg.Do(x => { });
            // assert
            Assert.That(result, Is.SameAs(arg));
        }

        #endregion

        #region Do (2 funcs)

        [Test]
        public void Do2_NullThis2ndNotNull_Does2rd()
        {
            // arrange
            string arg = null;
            string sideEffect = null;
            // act
            var result = arg.Do(x => sideEffect = "side1", x => sideEffect = "side2");
            // assert
            Assert.That(sideEffect, Is.EqualTo("side2"));
        }

        [Test]
        public void Do2_ActionsNull_DoesNothingDoesNotThrow()
        {
            // arrange
            string arg1 = null;
            string arg2 = "arg2";
            // act
            var result1 = arg1.Do(null, null);
            var result2 = arg2.Do(null, null);
            // assert
            Assert.That(arg1, Is.Null);
            Assert.That(arg2, Is.EqualTo("arg2"));
        }

        #endregion

        #region Return (value)

        [Test]
        public void Return_NullThis_ReturnsDefault()
        {
            // arrange
            string arg = null;
            int def = -1;
            // act
            int result = arg.Return(x => x.Length, def);
            // assert
            Assert.That(result, Is.EqualTo(def));
        }

        [Test]
        public void ReturnWithFunc_NullThis_ReturnsFuncResult()
        {
            // arrange
            string arg = null;
            Func<int> def = () => 42;
            // act
            int result = arg.Return(x => x.Length, def);
            // assert
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void ReturnWithFunc_NullThisNullFunc_Throws()
        {
            string arg = null;
            Assert.Throws<ArgumentNullException>(() => arg.Return(x => x.Length, null));
        }

        [Test]
        public void Return_NullFunc_Throws()
        {
            string arg = "arg";
            Assert.Throws<ArgumentNullException>(() => arg.Return<string, int>(null, 1));
        }

        [Test]
        public void ReturnWithFunc_NullFunc_Throws()
        {
            string arg = "arg";
            Assert.Throws<ArgumentNullException>(() => arg.Return<string, int>(null, () => 1));
        }

        [Test]
        public void Return_NotNull_ReturnsSameReference()
        {
            // arrange
            string arg = "arg";
            int def = -1;
            // act
            int result = arg.Return(x => x.Length, def);
            // assert
            Assert.That(result, Is.EqualTo(3));
        }

        #endregion

        #region Return (func)

        [Test]
        public void ReturnFunc_NotNull_ReturnsSameReference()
        {
            // arrange
            string arg = "arg";
            // act
            string result = arg.Return(x => x, () => "");
            // assert
            Assert.That(result, Is.SameAs(arg));
        }

        #endregion

        #region If

        [Test]
        public void If_NullThis_ReturnsNull()
        {
            // arrange
            string arg = null;
            // act
            var result = arg.If(null);
            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void If_NotNullThisNullPredicate_Throws()
        {
            // arrange
            string arg = "string";
            // act - assert
            Assert.Throws<ArgumentNullException>(() => arg.If(null));
        }

        [Test]
        public void If_NotNullBothAndPredicateTrue_ReturnsThis()
        {
            // arrange
            string arg = "string";
            // act
            var result = arg.If(x => x.Length > 2);
            // assert
            Assert.That(result, Is.SameAs(arg));
        }

        [Test]
        public void If_NotNullBothAndPredicateFalse_ReturnsNull()
        {
            // arrange
            string arg = "string";
            // act
            var result = arg.If(x => x == "anotherstring");
            // assert
            Assert.That(result, Is.Null);
        }

        #endregion
    }
}
