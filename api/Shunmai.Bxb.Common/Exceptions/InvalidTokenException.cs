using System;

namespace Shunmai.Bxb.Common.Exceptions
{
    /// <summary>
    /// 表示非法的 Token 的异常信息
    /// </summary>
    [Serializable]
    public class InvalidTokenException : Exception
    {
        private const string DEFAULT_MESSAGE = "invalid token";

        public InvalidTokenException() : base(DEFAULT_MESSAGE) { }
        public InvalidTokenException(string message) : base(message) { }
        public InvalidTokenException(string message, Exception inner) : base(message, inner) { }
    }
}
