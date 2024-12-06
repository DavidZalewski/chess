using Chess.Collections;

namespace Tests.Collections
{
    [Category("CORE")]
    public class SortedTupleBagTests
    {
        [Test]
        public void Add_MultipleItems_SortsByKey()
        {
            var bag = new SortedTupleBag<int, string>();

            bag.Add(2, "two");
            bag.Add(1, "one");
            bag.Add(3, "three");

            var items = bag.ToList();
            Assert.That(items.Select(x => x.Item1), Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void Add_DuplicateKeys_KeepsAllItems()
        {
            var bag = new SortedTupleBag<int, string>();

            bag.Add(1, "one");
            bag.Add(1, "one again");

            Assert.That(bag.Count, Is.EqualTo(2));
        }
    }
}