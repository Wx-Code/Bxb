using System.ComponentModel;

namespace Shunmai.Bxb.Entities.Enums
{
    public enum UserLogType
    {
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Register = 0,

        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login = 1,

        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share = 2,

        /// <summary>
        /// 更新微信二维码图像
        /// </summary>
        [Description("更新微信二维码图像")]
        UpdateQrCode = 3,

        /// <summary>
        /// 填写银行卡信息
        /// </summary>
        [Description("填写银行卡信息")]
        Authenticated = 9,

        /// <summary>
        /// 填写银行卡信息
        /// </summary>
        [Description("填写银行卡信息")]
        FillBankInfo = 10,

        /// <summary>
        /// 更新钱包的地址
        /// </summary>
        [Description("更新钱包地址")]
        UpdateWalletAddress=11,
    }
}
