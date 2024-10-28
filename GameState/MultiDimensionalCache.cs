using Chess.Globals;
using System.Collections.Concurrent;

namespace Chess.GameState
{
    public class MultiDimensionalCache<TValue>
    {
        /// <summary>
        /// The main cache that stores all items.
        /// </summary>
        public readonly ConcurrentDictionary<string, TValue> _mainCache;

        /// <summary>
        /// The index cache that stores items by partition.
        /// </summary>
        public readonly ConcurrentDictionary<string, ConcurrentDictionary<string, TValue>> _indexCache;

        /// <summary>
        /// The size of each partition in the index cache.
        /// </summary>
        private int _partitionSize;

        /// <summary>
        /// The number of times an item was not found in the index cache.
        /// </summary>
        public int IndexCacheMisses { get; private set; }

        /// <summary>
        /// The number of times an item was accessed through the main cache.
        /// </summary>
        public int MainCacheHits { get; private set; }

        public MultiDimensionalCache(int partitionSize)
        {
            StaticLogger.Trace();
            _mainCache = new ConcurrentDictionary<string, TValue>();
            _indexCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, TValue>>();
            _partitionSize = partitionSize;
        }

        /// <summary>
        /// Adds or updates an item in the cache.
        /// </summary>
        public void AddOrUpdate(string key, TValue value)
        {
            StaticLogger.Trace();
            _mainCache.AddOrUpdate(key, value, (k, v) => value);
            var prefix = key.Substring(0, _partitionSize);
            if (!_indexCache.TryGetValue(prefix, out var index))
            {
                index = new ConcurrentDictionary<string, TValue>();
                _indexCache.TryAdd(prefix, index);
            }
            index.TryAdd(key, _mainCache[key]);
        }

        /// <summary>
        /// Tries to get a value from the cache.
        /// </summary>
        public bool TryGetValue(string key, out TValue value)
        {
            StaticLogger.Trace();
            var prefix = key.Substring(0, _partitionSize);
            if (_indexCache.TryGetValue(prefix, out var index))
            {
                if (index.TryGetValue(key, out value))
                {
                    return true;
                }
            }

            IndexCacheMisses++;

            if (_mainCache.TryGetValue(key, out value))
            {
                MainCacheHits++;
                return true;
            }

            value = default;
            return false;
        }
    }
}