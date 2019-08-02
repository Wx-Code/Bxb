using System;

namespace Shunmai.Bxb.Api.App.Constants
{
    public static class Defaults
    {
        /// <summary>
        /// TOKEN 有效期
        /// </summary>
        public static readonly TimeSpan TOKEN_EXPIRES = TimeSpan.FromMinutes(3 * 24 * 60);

        /// <summary>
        /// 待转币 剩余 过期时间
        /// </summary>
        public static readonly TimeSpan SellerOperating_EXPIRES = TimeSpan.FromHours(2);

        /// <summary>
        /// 待收款 剩余 过期时间
        /// </summary>
        public static readonly TimeSpan BuyerPaying_EXPIRES = TimeSpan.FromHours(24);
    }
}
