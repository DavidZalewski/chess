using Chess.GameState;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MultiDimensionalCacheTests
    {
        [Test]
        public void AddOrUpdate_MainCache_Success()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);

            // Act
            cache.AddOrUpdate("key1", 1);
            cache.AddOrUpdate("key2", 2);

            // Assert
            Assert.That(cache.TryGetValue("key1", out int value1));
            Assert.That(1, Is.EqualTo(value1));
            Assert.That(cache.TryGetValue("key2", out int value2));
            Assert.That(2, Is.EqualTo(value2));
            Assert.That(cache.IndexCacheMisses, Is.EqualTo(0));
            Assert.That(cache.MainCacheHits, Is.EqualTo(0));
        }

        [Test]
        public void TryGetValue_IndexCache_Success()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);
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
            Assert.That(cache.IndexCacheMisses, Is.EqualTo(0));
            Assert.That(cache.MainCacheHits, Is.EqualTo(0));
        }

        [Test]
        public void TryGetValue_CatchAllPartition_Success()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);
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
            Assert.That(cache.IndexCacheMisses, Is.EqualTo(0));
            Assert.That(cache.MainCacheHits, Is.EqualTo(0));
        }

        [Test]
        public void IndexCache_DoesNotCreateCopies()
        {
            // Arrange
            var cache = new MultiDimensionalCache<CacheItem>(2);
            cache.AddOrUpdate("key1", new CacheItem(111));

            // Act
            cache.TryGetValue("key1", out CacheItem value);
            var mainCacheValue = cache._mainCache["key1"];
            var indexCacheValue = cache._indexCache["ke"]["key1"];

            // Assert
            Assert.That(ReferenceEquals(mainCacheValue, indexCacheValue));
            Assert.That(cache.IndexCacheMisses, Is.EqualTo(0));
            Assert.That(cache.MainCacheHits, Is.EqualTo(0));
        }

        [Test]
        public void AddManyItems_PartitionsCorrectly()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);

            // Act
            for (int i = 0; i < 100; i++)
            {
                cache.AddOrUpdate($"key{i}", i);
            }

            // Assert
            Assert.That(cache._indexCache.Keys.Count, Is.EqualTo(10));
            Assert.That(cache.TryGetValue("key42", out int value));
            Assert.That(42, Is.EqualTo(value));
            Assert.That(cache.IndexCacheMisses, Is.EqualTo(0));
            Assert.That(cache.MainCacheHits, Is.EqualTo(0));
        }
    }
}