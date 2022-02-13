using NUnit.Framework;
using System;
using System.Linq;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class InlineEqualityComparer_Tests
    {
        #region Ctor

        [Test]
        public void Ctor_EqualsIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineEqualityComparer<int>(null!, x => x.GetHashCode()));
        }

        [Test]
        public void Ctor_GethashcodeEqualsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineEqualityComparer<int>((x, y) => x.Equals(y), null!));
        }

        #endregion

        #region GroupBy with InlineEqualityComparer

        [Test]
        public void GroupBy_CaseInsensitiveString_GroupsCaseInsensitive()
        {
            // arrange
            var items = new (string, int)[]
            {
                ("One", 1),
                ("Two", 2),   ("two", 22),
                ("Three", 3), ("Three", 33), ("three", 333)
            };

            // act
            var comparer = new InlineEqualityComparer<string>(
                (x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase),
                x => x.ToLower().GetHashCode());
            var result = items.GroupBy(x => x.Item1, comparer);
            var ones = result.First(x => x.Key.ToLower() == "one");
            var twos = result.First(x => x.Key.ToLower() == "two");
            var threes = result.First(x => x.Key.ToLower() == "three");

            // assert
            Assert.That(ones.Count(), Is.EqualTo(1));
            Assert.That(twos.Count(), Is.EqualTo(2));
            Assert.That(threes.Count(), Is.EqualTo(3));
        }

        #endregion
    }
}
