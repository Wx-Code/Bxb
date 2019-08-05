using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Exceptions
{
    public class CacheException : Exception
    {
        private const string DEFAULT_MESSAGE = "An exception occurs on operating the cache.";

        public CacheException() : base(DEFAULT_MESSAGE)
        {
        }
    }
}
