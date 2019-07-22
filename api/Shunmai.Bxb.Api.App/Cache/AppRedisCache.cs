using Shunmai.Bxb.Cache;
using System;

namespace Shunmai.Bxb.Api.App.Cache
{
    public class AppRedisCache : RedisCache
    {
        private const string KEY_PREFIX = "Shunmai:Bxb:Api:App:";

        private string GetKey(string key)
        {
            return KEY_PREFIX + key;
        }

        public override string Get(string key)
        {
            return base.Get(GetKey(key));
        }

        public override T Get<T>(string key)
        {
            return base.Get<T>(GetKey(key));
        }

        public override bool Remove(string key)
        {
            return base.Remove(GetKey(key));
        }

        public override bool Set(string key, object data, TimeSpan? expires)
        {
            return base.Set(GetKey(key), data, expires);
        }
    }
}
