using Shunmai.Bxb.Common.Models;

namespace Shunmai.Bxb.Api.App.Constansts
{
    public static class Errors
    {
        /// <summary>
        /// 描述用户尚未注册的错误信息
        /// </summary>
        public static ErrorInfo UserNotRegister = new ErrorInfo("0100", "此用户尚未注册");

        /// <summary>
        /// 描述用户尚未填写钱包地址的错误信息
        /// </summary>
        public static ErrorInfo UserWalletAddressNotExists = new ErrorInfo("0120", "此用户地址尚未填写");

        /// <summary>
        /// 描述交易信息不存在的错误信息
        /// </summary>
        public static ErrorInfo TradeHallNotExists = new ErrorInfo("1001", "交易信息不存在");

        /// <summary>
        /// 描述交易信息状态异常的错误信息
        /// </summary>
        public static ErrorInfo CannotTrade = new ErrorInfo("1002", "此交易信息无法交易");

        /// <summary>
        /// 描述交易码输入有误的错误信息
        /// </summary>
        public static ErrorInfo TradeCodeError = new ErrorInfo("1003", "交易码输入有误");

        /// <summary>
        /// 描述可交易数量不足的错误信息
        /// </summary>
        public static ErrorInfo NotEnoughCount = new ErrorInfo("1004", "可交易数量不足");
    }
}
