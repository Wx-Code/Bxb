using Microsoft.AspNetCore.Http;
using Shunmai.Bxb.Api.App.Constants;

namespace Shunmai.Bxb.Api.App.Utils
{
    internal static class HttpContextHelper
    {
        /// <summary>
        /// 将当前请求的用户 ID 缓存在给定的 <see cref="HttpContext.Items"/> 集合中
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="userId"></param>
        internal static void SetUserId(HttpContext httpContext, int userId)
        {
            httpContext.Items[Names.USERID_CACHE] = userId;
        }

        /// <summary>
        /// 从给定的 <see cref="HttpContext.Items"/> 集合中，取出当前请求的用户 ID
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        internal static int GetUserId(HttpContext httpContext)
        {
            var cache = httpContext.Items[Names.USERID_CACHE];
            if (cache == null)
            {
                return 0;
            }
            return (int)cache;
        }
    }
}
