using Chess.Collections;
using NUnit.Framework;

namespace Tests.Collections
{
    [Category("CORE")]
    internal class SortedTupleBagTests
    {
        [Test]
        public void SortedTupleBag_AddsItemsInOrder()
        {
            // Arrange
            var bag = new SortedTupleBag<int, string>();

            // Act
            bag.Add(3, "three");
            bag.Add(1, "one");
            bag.Add(2, "two");

            // Assert
            Assert.That(bag.Count, Is.EqualTo(3));
            Assert.That(bag, Is.EquivalentTo(new[] { Tuple.Create(1, "one"), Tuple.Create(2, "two"), Tuple.Create(3, "three") }));
        }

        [Test]
        public void SortedTupleBag_AddsDuplicateKeys()
        {
            // Arrange
            var bag = new SortedTupleBag<int, string>();

            // Act
            bag.Add(1, "one");
            bag.Add(1, "another one");

            // Assert
            Assert.That(bag.Count, Is.EqualTo(2));
            Assert.That(bag, Is.EquivalentTo(new[] { Tuple.Create(1, "one"), Tuple.Create(1, "another one") }));
        }
    }
}