using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum UserState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,

        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled = -1,
    }
}
