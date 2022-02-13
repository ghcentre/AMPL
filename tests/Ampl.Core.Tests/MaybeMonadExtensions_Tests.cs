using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class MaybeMonadExtensions_Tests
    {
        #region Models
        
        public class ViewModel
        {
            public Response? Response { get; set; }
        }

        public class Response
        {
            public Error? Error { get; set; }
        }

        public class Error
        {
            public Exception? Exception { get; set; }
        }

        private ViewModel _model = default!;

        #endregion

        #region Setup

        [SetUp]
        public void TestInitialize()
        {
            _model = new ViewModel()
            {
                Response = new Response()
                {
                    Error = new Error()
                    {
                        Exception = new Exception("Outer exception", new ArgumentException())
                    }
                }
            };
        }

        #endregion

        #region With

        [Test]
        public void With_NullThis_ReturnsNull()
        {
            string? arg = null;

            string? result1 = arg.With(x => x + "string");
            string? result2 = arg.With<string, string>(null!);

            Assert.That(result1, Is.Null);
            Assert.That(result2, Is.Null);
        }

        [Test]
        public void With_NullNullable_ReturnsNull()
        {
            int? arg = null;
            int? result = arg.With(x => x + 1);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void With_NullFunc_Throws()
        {
            string arg = "The string";
            Assert.Throws<ArgumentNullException>(() => arg.With<string, string>(null!));
        }

        [Test]
        public void With_Chaining_ReturnsNested()
        {
            var arg = _model;
            var result = arg.With(x => x.Response).With(x => x.Error).With(x => x.Exception);
            Assert.That(result, Is.SameAs(_model.Response!.Error!.Exception));
        }

        [Test]
        public void With_ChainingNullInsideChain_ReturnsNull()
        {
            var arg = _model;
            arg.Response!.Error = null;
            
            var result = arg.With(x => x.Response).With(x => x.Error).With(x => x.Exception);
            
            Assert.That(result, Is.Null);
        }

        #endregion

        #region Do (one func)

        [Test]
        public void Do_NullThis_DoesNothing()
        {
            string? arg = null;
            string? sideEffect = null;

            var result = arg.Do(x => sideEffect = "side");

            Assert.That(sideEffect, Is.Null);
        }

        [Test]
        public void Do_NullNullable_DoesNothing()
        {
            int? arg = null;
            string? sideEffect = null;

            var result = arg.Do(x => sideEffect = "side");

            Assert.That(sideEffect, Is.Null);
        }

        [Test]
        public void Do_NullThis_ReturnsNull()
        {
            string? arg = null;
            var result = arg.Do(x => { });
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Do_NullAction_Throws()
        {
            string arg = "arg";
            Assert.Throws<ArgumentNullException>(() => arg.Do(null!));
        }

        [Test]
        public void Do_NotNull_DoesSideEffects()
        {
            string arg = "arg";
            string? sideEffect = null;

            var result = arg.Do(x => sideEffect = "side");

            Assert.That(sideEffect, Is.EqualTo("side"));
        }

        [Test]
        public void Do_NotNull_ReturnsSameReference()
        {
            string arg = "arg";
            var result = arg.Do(x => { });
            Assert.That(result, Is.SameAs(arg));
        }

        #endregion

        #region Do (2 funcs)

        [Test]
        public void Do2_NullThis2ndNotNull_Does2rd()
        {
            string? arg = null;
            string? sideEffect = null;

            var result = arg.Do(x => sideEffect = "side1", x => sideEffect = "side2");

            Assert.That(sideEffect, Is.EqualTo("side2"));
        }

        [Test]
        public void Do2_ActionsNull_DoesNothingDoesNotThrow()
        {
            string? arg1 = null;
            string arg2 = "arg2";

            var result1 = arg1.Do(null, null);
            var result2 = arg2.Do(null, null);

            Assert.That(result1, Is.Null);
            Assert.That(result2, Is.EqualTo("arg2"));
        }

        #endregion

        #region Return (value)

        [Test]
        public void Return_NullThis_ReturnsDefault()
        {
            string? arg = null;
            int def = -1;

            int result = arg.Return(x => x!.Length, def);

            Assert.That(result, Is.EqualTo(def));
        }

        [Test]
        public void ReturnWithFunc_NullThis_ReturnsFuncResult()
        {
            string? arg = null;
            Func<int> def = () => 42;

            int result = arg.Return(x => x!.Length, def);

            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void ReturnWithFunc_NullThisNullFunc_Throws()
        {
            string? arg = null;
            Assert.Throws<ArgumentNullException>(() => arg.Return(x => x!.Length, null!));
        }

        [Test]
        public void Return_NullFunc_Throws()
        {
            string arg = "arg";
            Assert.Throws<ArgumentNullException>(() => arg.Return<string, int>(null!, 1));
        }

        [Test]
        public void ReturnWithFunc_NullFunc_Throws()
        {
            string arg = "arg";
            Assert.Throws<ArgumentNullException>(() => arg.Return<string, int>(null!, () => 1));
        }

        [Test]
        public void Return_NotNull_ReturnsSameReference()
        {
            string arg = "arg";
            int def = -1;

            int result = arg.Return(x => x.Length, def);

            Assert.That(result, Is.EqualTo(3));
        }

        #endregion

        #region Return (func)

        [Test]
        public void ReturnFunc_NotNull_ReturnsSameReference()
        {
            string arg = "arg";
            string result = arg.Return(x => x, () => "");
            Assert.That(result, Is.SameAs(arg));
        }

        #endregion

        #region If

        [Test]
        public void If_NullThis_ReturnsNull()
        {
            string? arg = null;
            var result = arg.If(null!);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void If_NotNullThisNullPredicate_Throws()
        {
            string arg = "string";
            Assert.Throws<ArgumentNullException>(() => arg.If(null!));
        }

        [Test]
        public void If_NotNullBothAndPredicateTrue_ReturnsThis()
        {
            string arg = "string";
            var result = arg.If(x => x.Length > 2);
            Assert.That(result, Is.SameAs(arg));
        }

        [Test]
        public void If_NotNullBothAndPredicateFalse_ReturnsNull()
        {
            string arg = "string";
            var result = arg.If(x => x == "anotherstring");
            Assert.That(result, Is.Null);
        }

        #endregion
    }
}
