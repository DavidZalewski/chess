using System.Collections.Concurrent;

namespace Chess.GameState
{
    public class MultiDimensionalCache<TKey, TValue>
    {
        public readonly ConcurrentDictionary<TKey, TValue> _mainCache;
        public readonly ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TValue>> _indexCache;
        private int _partitionSize;
        public int IndexCacheMisses { get; private set; }
        public int MainCacheAccesses { get; private set; }

        public MultiDimensionalCache(int partitionSize)
        {
            _mainCache = new ConcurrentDictionary<TKey, TValue>();
            _indexCache = new ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TValue>>();
            _partitionSize = partitionSize;
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            _mainCache.AddOrUpdate(key, value, (k, v) => value);
            var prefix = key.ToString().Substring(0, _partitionSize);
            if (!_indexCache.TryGetValue((TKey)(object)prefix, out var index))
            {
                index = new ConcurrentDictionary<TKey, TValue>();
                _indexCache.TryAdd((TKey)(object)prefix, index);
            }
            index.TryAdd(key, _mainCache[key]);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var prefix = key.ToString().Substring(0, _partitionSize);
            if (_indexCache.TryGetValue((TKey)(object)prefix, out var index))
            {
                if (index.TryGetValue(key, out value))
                {
                    return true;
                }
            }

            // If we reach this point, it means the item was not found in the index cache
            IndexCacheMisses++;

            if (_mainCache.TryGetValue(key, out value))
            {
                MainCacheAccesses++;
                return true;
            }

            value = default;
            return false;
        }
    }
}