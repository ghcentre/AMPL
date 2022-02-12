using NUnit.Framework;
using System;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class DisposableBag_Tests
    {
        #region Utility Classes

        private class DisposableActionWrapper : IDisposable
        {
            private readonly Action _actionOnDispose;

            public DisposableActionWrapper(Action actionOnDispose)
            {
                _actionOnDispose = actionOnDispose ?? throw new ArgumentNullException(nameof(actionOnDispose));
            }

            public void Dispose()
            {
                _actionOnDispose();
            }
        }

        #endregion

        #region Create

        [Test]
        public void Create_WithoutParameter_ValueIsNull()
        {
            var actual = DisposableBag.Create();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.Null);
        }

        [Test]
        public void Create_WithParameter_CreatesNonNull()
        {
            var actual = DisposableBag.Create(42);
            Assert.That(actual, Is.Not.Null);
        }

        #endregion

        #region With

        [Test]
        public void With_AnyArg_ReturnsSameBag()
        {
            var bag = DisposableBag.Create(42);
            var actual = bag.With(() => { });
            Assert.That(actual, Is.SameAs(bag));
        }

        #endregion
    }
}
