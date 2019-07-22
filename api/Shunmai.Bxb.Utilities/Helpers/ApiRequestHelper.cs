using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public partial class ApiRequestHelper
    {
        /// <summary>
        /// 用于指定将请求参数序列化为 url 的 query 格式，即：k1=v1&k2=v2
        /// </summary>
        private const string FORMAT_QUERY = "query";
        /// <summary>
        /// 用于指定将请求参数序列化为 json 格式
        /// </summary>
        private const string FORMAT_JSON = "json";

        /// <summary>
        /// fix：当传入的 data 本身是 string 类型的值时，仍然调用 JsonSerialize 方法进行序列化
        ///      导致得到错误的参数值，从而导致接口调用失败的问题
        /// </summary>
        /// <author>谭光洪</author>
        /// <since>2018-11-08 17:51</since>
        private static string Serialize(object data, string format = FORMAT_JSON)
        {
            if (data == null)
            {
                return string.Empty;
            }
            if (data.GetType() == typeof(string))
            {
                return data.ToString();
            }
            if (format == FORMAT_JSON)
            {
                return data.JsonSerialize();
            }
            return data.ToQueryString();
        }



        private static string GetSerializeFormat(HttpMethod method)
        {
            if (method == HttpMethod.Get || method == HttpMethod.Delete)
            {
                return FORMAT_QUERY;
            }
            else
            {
                return FORMAT_JSON;
            }
        }



        private static void ConcatHeaders(Dictionary<string, string> commonHeaders, Dictionary<string, string> additionalHeaders)
        {
            if (additionalHeaders != null)
            {
                foreach (var item in additionalHeaders)
                {
                    if (commonHeaders.ContainsKey(item.Key))
                    {
                        commonHeaders[item.Key] = item.Value;
                    }
                    else
                    {
                        commonHeaders.Add(item.Key, item.Value);
                    }
                }
            }
        }

        private static HttpRequestResult HttpSend(
            HttpMethod method,
            HttpRequestParameterType paramType,
            string contextType,
            string url,
            string data, Dictionary<string, string> headers = null)
        {


            HttpRequestResult result = HttpHelper.Send(
                url,
                data,
                headers,
                method,
                paramType,
                contextType,
                Encoding.UTF8);
            return result;
        }

        private static TResult Request<TResult>(
            HttpMethod httpMethod,
            string api,
            object data,
            Dictionary<string, string> headers,bool isToJson=true)
        {
            var serializeFormat = GetSerializeFormat(httpMethod);
            var requestData = Serialize(data, serializeFormat);
            var paramType = HttpRequestParameterType.Body;
            if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
            {
                paramType = HttpRequestParameterType.Query;
            }
            HttpRequestResult result = null;
            try
            {
                result = HttpSend(httpMethod, paramType, HttpContextType.JSON, api, requestData, headers);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                if (!isToJson)
                {
                    return (TResult)Convert.ChangeType(result.Result,typeof(TResult));
                }
                return result.Result.JsonDeserialize<TResult>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过向指定资源发送 Get 请求，并尝试将最终结果反序列化为指定实体类型
        /// </summary>
        /// <typeparam name="TResult">对请求结果进行反序列化时，所指定的数据类型参数</typeparam>
        /// <param name="api">请求接口相对地址</param>
        /// <param name="data">请求接口所需要的参数实体</param>
        /// <param name="headers">本次请求所需要携带的额外头部信息</param>
        /// <param name="headers">是否要序列化</param>
        /// <returns>将请求结果反序列化后的实体，实体类型参数泛型类型参数指定</returns>
        public static TResult Get<TResult>(
            string api,
            object data = null,
            Dictionary<string, string> headers = null,bool isToJson = true)
        {
            return Request<TResult>(HttpMethod.Get, api, data, headers,isToJson);
        }

        /// <summary>
        /// 通过向指定资源发送 Post 请求，并尝试将最终结果反序列化为指定实体类型
        /// </summary>
        /// <typeparam name="TResult">对请求结果进行反序列化时，所指定的数据类型参数</typeparam>
        /// <param name="api">请求接口相对地址</param>
        /// <param name="data">请求接口所需要的参数实体</param>
        /// <param name="headers">本次请求所需要携带的额外头部信息</param>
        /// <returns>将请求结果反序列化后的实体，实体类型参数泛型类型参数指定</returns>
        public static TResult Post<TResult>(
            string api,
            object data = null,
            Dictionary<string, string> headers = null)
        {
            return Request<TResult>(HttpMethod.Post, api, data, headers);
        }

        /// <summary>
        /// 通过向指定资源发送 Put 请求，并尝试将最终结果反序列化为指定实体类型
        /// </summary>
        /// <typeparam name="TResult">对请求结果进行反序列化时，所指定的数据类型参数</typeparam>
        /// <param name="api">请求接口相对地址</param>
        /// <param name="data">请求接口所需要的参数实体</param>
        /// <param name="headers">本次请求所需要携带的额外头部信息</param>
        /// <returns>将请求结果反序列化后的实体，实体类型参数泛型类型参数指定</returns>
        public static TResult Put<TResult>(
            string api,
            object data = null,
            Dictionary<string, string> headers = null)
        {
            return Request<TResult>(HttpMethod.Put, api, data, headers);
        }

        /// <summary>
        /// 通过向指定资源发送 Delete 请求，并尝试将最终结果反序列化为指定实体类型
        /// </summary>
        /// <typeparam name="TResult">对请求结果进行反序列化时，所指定的数据类型参数</typeparam>
        /// <param name="api">请求接口相对地址</param>
        /// <param name="data">请求接口所需要的参数实体</param>
        /// <param name="headers">本次请求所需要携带的额外头部信息</param>
        /// <returns>将请求结果反序列化后的实体，实体类型参数泛型类型参数指定</returns>
        public static TResult Delete<TResult>(
            string api,
            object data = null,
            Dictionary<string, string> headers = null)
        {
            return Request<TResult>(HttpMethod.Delete, api, data, headers);
        }

        /// <summary>
        /// 通过向指定资源发送 Delete 请求，并尝试将最终结果反序列化为指定实体类型
        /// </summary>
        /// <typeparam name="TResult">对请求结果进行反序列化时，所指定的数据类型参数</typeparam>
        /// <param name="api">请求接口相对地址</param>
        /// <param name="data">请求接口所需要的参数实体</param>
        /// <param name="headers">本次请求所需要携带的额外头部信息</param>
        /// <returns>将请求结果反序列化后的实体，实体类型参数泛型类型参数指定</returns>
        public static async Task<TResult> PostAsync<TResult>(
            string api,
            object data = null,
            Dictionary<string, string> headers = null)
        {
            return await Task.Factory.StartNew(() => Request<TResult>(HttpMethod.Post, api, data, headers));
        }

    }

}
