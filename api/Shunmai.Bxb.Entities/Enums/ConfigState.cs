using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    /// <summary>
    /// 配置状态， 可用于  1、 平台钱包地址状态
    /// </summary>
    public enum ConfigState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 1,
    }
}
