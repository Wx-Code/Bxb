using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    /// 应用程序类型
    /// </summary>
    public enum ApplicationType
    {
        /// <summary>
        /// 微信小程序
        /// </summary>
        [Description("微信小程序")]
        WeixinMiniProgram = 1,

        /// <summary>
        /// 后台管理系统
        /// </summary>
        [Description("后台管理系统")]
        Admin = 2,
    }
}
