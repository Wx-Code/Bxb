using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
