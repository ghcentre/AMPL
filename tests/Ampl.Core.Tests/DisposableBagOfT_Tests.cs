using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class DisposableBagOfT_Tests
    {
        private readonly List<string> _messages = new();

        [SetUp]
        public void Setup()
        {
            _messages.Clear();
        }


        private class BagObject
        {
        }


        [Test]
        public void Ctor_Value_SameReference()
        {
            var bagObject = new BagObject();
            var bag = new DisposableBag<BagObject>(bagObject);

            var actual = bag.Value;

            Assert.That(actual, Is.SameAs(bagObject));
        }

        [Test]
        public void Ctor_Null_AllowsNulls()
        {
            var bag = new DisposableBag<string>(null);
            var actual = bag.Value;
            Assert.That(actual, Is.Null);
        }

        [Test]
        public void Add_Action_Executes()
        {
            Action action = () => _messages.Add("string");
            var bag = new DisposableBag<string>(null);

            bag.Add(action);
            bag.Dispose();

            Assert.That(_messages[0], Is.EqualTo("string"));
        }

        [Test]
        public void Add_AfterDispose_Throws()
        {
            Action action = () => _messages.Add("string");
            var bag = new DisposableBag<string>(null);

            bag.Dispose();

            Assert.Throws<ObjectDisposedException>(() => bag.Add(action));
        }

        [Test]
        public void Dispose_ExecutedOnce()
        {
            Action action = () => _messages.Add("string");
            var bag = new DisposableBag<string>(null);
            bag.Add(action);

            bag.Dispose();
            bag.Dispose();

            Assert.That(_messages.Count, Is.EqualTo(1));
        }

        [Test]
        public void Dispose_AddedInOrder_ExecutedInReverseOrder()
        {
            Action action1 = () => _messages.Add("1");
            Action action2 = () => _messages.Add("2");
            Action action3 = () => _messages.Add("3");
            var bag = new DisposableBag<string>(null);
            bag.Add(action1);
            bag.Add(action2);
            bag.Add(action3);

            bag.Dispose();

            Assert.That(_messages[0], Is.EqualTo("3"));
            Assert.That(_messages[1], Is.EqualTo("2"));
            Assert.That(_messages[2], Is.EqualTo("1"));
        }
    }
}
