using Shunmai.Bxb.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.UnitTests
{
    public sealed class TestCache : ICache
    {
        private static TestCache _instance;
        private static object _lockKey = new object();
        private ConcurrentDictionary<string, object> _cache;

        private TestCache()
        {
            _cache = new ConcurrentDictionary<string, object>();
        }

        public static TestCache GetInstance()
        {
            if (_instance == null)
            {
                lock(_lockKey)
                {
                    if (_instance == null)
                    {
                        _instance = new TestCache();
                    }
                }
            }
            return _instance;
        }

        public bool Set(string key, object data, TimeSpan? expires)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.TryRemove(key, out _);
            }
            return _cache.TryAdd(key, data);
        }

        public T Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default(T);
        }

        public string Get(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                return value?.ToString();
            }
            return default(string);
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public long ListLength(string redisKey)
        {
            throw new NotImplementedException();
        }

        public long ListRightPush(string redisKey, string redisValue)
        {
            throw new NotImplementedException();
        }

        public string ListLeftPop(string redisKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ListRange(string redisKey)
        {
            throw new NotImplementedException();
        }

        public bool SetAdd(string redisKey, string redisValue)
        {
            throw new NotImplementedException();
        }

        public long SetLength(string redisKey)
        {
            throw new NotImplementedException();
        }
    }
}
