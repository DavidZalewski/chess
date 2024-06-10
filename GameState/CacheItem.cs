using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameState
{
    public class CacheItem
    {
        private ulong _accessCount;
        public ulong Value { get; set; }
        public ulong AccessCount { get { return _accessCount; } }

        public CacheItem(ulong value)
        {
            Value = value;
            _accessCount = 1;
        }

        public ulong InterlockedIncrement()
        {
            return Interlocked.Increment(ref _accessCount);
        }
    }
}
