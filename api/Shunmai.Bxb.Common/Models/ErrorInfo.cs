// TODO: 创建 ErrorInfo 对象缓存池，避免重复创建大量对象

namespace Shunmai.Bxb.Common.Models
{
    public class ErrorInfo
    {
        public string Code { get; }
        public string Message { get; }

        public ErrorInfo(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 创建表示请求成功的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <returns></returns>
        public static ErrorInfo OfRequestSuccess()
        {
            return new ErrorInfo("0000", "请求成功");
        }

        /// <summary>
        /// 创建表示请求失败的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <returns></returns>
        public static ErrorInfo OfRequestFailed(string message = null)
        {
            return new ErrorInfo("0001", message ?? "请求失败");
        }

        /// <summary>
        /// 创建表示数据更新失败的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <returns></returns>
        public static ErrorInfo OfLoginSuccess()
        {
            return new ErrorInfo("0002", "登录成功");
        }

        /// <summary>
        /// 创建表示数据更新失败的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <returns></returns>
        public static ErrorInfo OfLoginFailed(string message = null)
        {
            return new ErrorInfo("0003", message ?? "登录失败");
        }

        /// <summary>
        /// 创建表示未授权的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ErrorInfo OfUnauthorized(string message = null)
        {
            return new ErrorInfo("0004", message ?? "未登录或者登录已失效");
        }

        /// <summary>
        /// 创建表示非法请求的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ErrorInfo OfIllegalRequest(string message = null)
        {
            return new ErrorInfo("0005", message ?? "非法的请求");
        }

        /// <summary>
        /// 创建表示服务器内部错误的 <see cref="ErrorInfo"/> 实体
        /// </summary>
        /// <returns>The server interal error.</returns>
        /// <param name="message">Message.</param>
        public static ErrorInfo OfServerInteralError(string message = null)
        {
            return new ErrorInfo("0006", message ?? "服务器繁忙");
        }
    }
}
