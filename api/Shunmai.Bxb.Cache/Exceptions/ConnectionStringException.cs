using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Cache.Exceptions
{

    [Serializable]
    public class ConnectionStringException : Exception
    {
        private const string DEFAULT_MESSAGE = "connection string exception";

        public ConnectionStringException() : this(DEFAULT_MESSAGE) { }

        public ConnectionStringException(string message) : base(message) { }
    }
}
