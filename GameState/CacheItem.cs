﻿using Chess.Globals;
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
            StaticLogger.Trace();
            StaticLogger.LogMethod(value);
            Value = value;
            _accessCount = 1;
        }

        public ulong InterlockedIncrement()
        {
            StaticLogger.Trace();
            return Interlocked.Increment(ref _accessCount);
        }
    }
}
