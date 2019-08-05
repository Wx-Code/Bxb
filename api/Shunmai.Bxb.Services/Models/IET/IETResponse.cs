using Newtonsoft.Json;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class IETResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        public bool Success => Code == ApiCode.SUCCESS;
    }

    public static class ApiCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        public const int SUCCESS = 0;
        /// <summary>
        /// 未登录
        /// </summary>
        public const int NOT_LOGIN = 21099;
        /// <summary>
        /// 解析密码异常
        /// </summary>
        public const int PWD_EXCEPTION = 21118;
        /// <summary>
        /// 密码错误
        /// </summary>
        public const int PWD_ERROR = 21012;
    }
}
