using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class InlineEqualityComparer_Tests
    {
        [Test]
        public void Ctor_EqualsIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineEqualityComparer<int>(null, x => x.GetHashCode()));
        }

        [Test]
        public void Ctor_GethashcodeEqualsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new InlineEqualityComparer<int>((x, y) => x.Equals(y), null));
        }

        [Test]
        public void GroupBy_CaseInsensitiveString_GroupsCaseInsensitive()
        {
            // arrange
            var items = new(string, int)[]
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
    }
}
