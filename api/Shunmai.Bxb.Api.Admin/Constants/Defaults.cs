using System;

namespace Shunmai.Bxb.Api.Admin.Constants
{
    public static class Defaults
    {
        /// <summary>
        /// 后台管理员默认密码
        /// </summary>
        public const string ADMIN_USER_DEFAULT_PASSWORD = "123456";

        /// <summary>
        /// 登录 TOKEN 有效期
        /// </summary>
        public const double API_TOKEN_EXPIRE_SECONDS = 7 * 24 * 3600;

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
