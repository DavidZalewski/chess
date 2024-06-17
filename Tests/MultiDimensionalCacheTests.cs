using Chess.GameState;
using NUnit.Framework.Internal;

namespace Tests
{
    [TestFixture]
    public class MultiDimensionalCacheTests
    {
        [Test]
        public void AddOrUpdate_MainCache_Success()
        {
            // Arrange
            var cache = new MultiDimensionalCache<string, int>(2);

            // Act
            cache.AddOrUpdate("key1", 1);
            cache.AddOrUpdate("key2", 2);

            // Assert
            Assert.That(cache.TryGetValue("key1", out int value1));
            Assert.That(1, Is.EqualTo(value1));
            Assert.That(cache.TryGetValue("key2", out int value2));
            Assert.That(2, Is.EqualTo(value2));
        }

        [Test]
        public void TryGetValue_IndexCache_Success()
        {
            // Arrange
            var cache = new MultiDimensionalCache<string, int>(2);
            cache.AddOrUpdate("key1", 1);
            cache.AddOrUpdate("key2", 2);
            cache.AddOrUpdate("key3", 3);

            // Act
            cache.TryGetValue("key1", out int value1);
            cache.TryGetValue("key2", out int value2);
            cache.TryGetValue("key3", out int value3);

            // Assert
            Assert.That(1, Is.EqualTo(value1));
            Assert.That(2, Is.EqualTo(value2));
            Assert.That(3, Is.EqualTo(value3));
        }

        [Test]
        public void TryGetValue_CatchAllPartition_Success()
        {
            // Arrange
            var cache = new MultiDimensionalCache<string, int>(2);
            cache.AddOrUpdate("key1", 1);
            cache.AddOrUpdate("key2", 2);
            cache.AddOrUpdate("otherKey", 3);

            // Act
            cache.TryGetValue("key1", out int value1);
            cache.TryGetValue("key2", out int value2);
            cache.TryGetValue("otherKey", out int value3);

            // Assert
            Assert.That(1, Is.EqualTo(value1));
            Assert.That(2, Is.EqualTo(value2));
            Assert.That(3, Is.EqualTo(value3));
        }

        [Test]
        public void IndexCache_DoesNotCreateCopies()
        {
            // Arrange
            var cache = new MultiDimensionalCache<string, int>(2);
            cache.AddOrUpdate("key1", 1);
            cache.AddOrUpdate("key2", 2);

            // Act
            var value1 = cache._mainCache["key1"];
            var value2 = cache._indexCache["ke"].First().Value;

            // Assert
            Assert.That(ReferenceEquals(value1, value2));
        }
    }
}
