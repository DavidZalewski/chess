using Chess.GameState;
using NUnit.Framework;
using System.Diagnostics;

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

        [Test]
        public void CacheHitRatioTest1()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);
            const int numItems = 10000;
            const int numRetrievals = 100000;
            for (int i = 0; i < numItems; i++)
            {
                cache.AddOrUpdate($"key{i}", i);
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            int hits = 0;
            for (int i = 0; i < numRetrievals; i++)
            {
                int value;
                if (cache.TryGetValue($"key{i % numItems}", out value))
                {
                    hits++;
                }
            }
            stopwatch.Stop();

            // Assert
            double hitRatio = (double)hits / numRetrievals;
            Assert.That(hitRatio, Is.GreaterThan(0.99));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000));
        }

        [Test]
        public void CacheRetrievalPerformanceTest()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);
            const int numItems = 10000;
            for (int i = 0; i < numItems; i++)
            {
                cache.AddOrUpdate($"key{i}", i);
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < numItems; i++)
            {
                int value;
                cache.TryGetValue($"key{i}", out value);
            }
            stopwatch.Stop();

            // Assert
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000));
        }

        [Test]
        public void CacheHitRatioTest()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);
            const int numItems = 10000;
            for (int i = 0; i < numItems; i++)
            {
                cache.AddOrUpdate($"key{i}", i);
            }

            // Act
            int hits = 0;
            for (int i = 0; i < numItems; i++)
            {
                int value;
                if (cache.TryGetValue($"key{i}", out value))
                {
                    hits++;
                }
            }

            // Assert
            double hitRatio = (double)hits / numItems;
            Assert.That(hitRatio, Is.EqualTo(1.0));
        }

        [Test]
        public void MultithreadedCacheAccessTest()
        {
            // Arrange
            var cache = new MultiDimensionalCache<int>(2);
            const int numItems = 10000;
            const int numThreads = 10;
            for (int i = 0; i < numItems; i++)
            {
                cache.AddOrUpdate($"key{i}", i);
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < numItems; j++)
                    {
                        int value;
                        cache.TryGetValue($"key{j}", out value);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            // Assert
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000));
        }
    }
}