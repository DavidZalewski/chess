using System.Collections.Concurrent;

namespace Chess.GameState
{
    public class MultiDimensionalCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _mainCache;
        private readonly ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TValue>> _indexCache;
        private int _partitionSize;

        public MultiDimensionalCache(int partitionSize)
        {
            _mainCache = new ConcurrentDictionary<TKey, TValue>();
            _indexCache = new ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TValue>>();
            _partitionSize = partitionSize;
        }

        //public void AddOrUpdate(TKey key, TValue value)
        //{
        //    _mainCache.AddOrUpdate(key, value, (k, v) => value);
        //    RepartitionCache();
        //}
        public void AddOrUpdate(TKey key, TValue value)
        {
            _mainCache.AddOrUpdate(key, value, (k, v) => value);
            var prefix = key.ToString().Substring(0, _partitionSize);
            if (!_indexCache.TryGetValue((TKey)(object)prefix, out var index))
            {
                index = new ConcurrentDictionary<TKey, TValue>();
                _indexCache.TryAdd((TKey)(object)prefix, index);
            }
            index.TryAdd(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var prefix = key.ToString().Substring(0, _partitionSize);
            if (_indexCache.TryGetValue((TKey)(object)prefix, out var index))
            {
                return index.TryGetValue(key, out value);
            }

            if (_mainCache.TryGetValue(key, out value))
            {
                return true;
            }


            value = default;
            return false;
        }

        private void RepartitionCache()
        {
            var commonPrefixes = _mainCache.Select(x => x.Key.ToString().Substring(0, _partitionSize)).Distinct();

            foreach (var prefix in commonPrefixes)
            {
                var partition = new ConcurrentDictionary<TKey, TValue>();
                foreach (var item in _mainCache.Where(x => x.Key.ToString().StartsWith(prefix)))
                {
                    partition.TryAdd(item.Key, item.Value);
                }
                _indexCache.TryAdd((TKey)(object)prefix, partition);
            }

            // Create a catch-all partition for items that don't fit into any other partition
            var catchAllPartition = new ConcurrentDictionary<TKey, TValue>();
            foreach (var item in _mainCache.Where(x => !_indexCache.Any(y => y.Value.ContainsKey(x.Key))))
            {
                catchAllPartition.TryAdd(item.Key, item.Value);
            }
            _indexCache.TryAdd((TKey)(object)"_catchAll", catchAllPartition);
        }
    }
}
