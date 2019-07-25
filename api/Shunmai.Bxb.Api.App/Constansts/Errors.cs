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
    }
}
