using System;
using System.Collections.Generic;

namespace Shunmai.Bxb.Abstractions
{
    public interface ICache
    {
        bool Set(string key, object data, TimeSpan? expires);
        T Get<T>(string key);
        string Get(string key);
        bool Remove(string key);
        long ListLength(string redisKey);
        long ListRightPush(string redisKey, string redisValue);
        string ListLeftPop(string redisKey);
        IEnumerable<string> ListRange(string redisKey);

        bool SetAdd(string redisKey, string redisValue);

        long SetLength(string redisKey);
    }
}
