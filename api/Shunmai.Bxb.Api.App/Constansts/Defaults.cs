using System;

namespace Shunmai.Bxb.Api.App.Constants
{
    public static class Defaults
    {
        /// <summary>
        /// TOKEN 有效期
        /// </summary>
        public static readonly TimeSpan TOKEN_EXPIRES = TimeSpan.FromMinutes(3 * 24 * 60);
    }
}
