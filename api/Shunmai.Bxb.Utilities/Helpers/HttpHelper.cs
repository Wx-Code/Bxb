using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using Shunmai.Bxb.Utilities.Extenssions;

namespace Shunmai.Bxb.Utilities.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 请求超时时间 600,000
        /// </summary>
        private static readonly int HTTP_REQUEST_TIMEOUT = 600000;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param> 
        /// <param name="method"></param>
        /// <param name="parameterType"></param>
        /// <param name="contextType"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static HttpRequestResult Send(
            string url,
            string data,
            Dictionary<string, string> headers,
            HttpMethod method,
            HttpRequestParameterType parameterType,
            string contextType,
            Encoding encode)
        {
            HttpRequestResult rst = new HttpRequestResult()
            {
                Code = HttpStatusCode.OK
            };
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }
            if (url.IsEmpty())
            {
                rst.Result = "url参数不能为空，必须传入正确的请求地址！";
                rst.Code = HttpStatusCode.BadRequest;
                return rst;
            }
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            try
            {
                if (parameterType == HttpRequestParameterType.Query)
                {
                    if (!data.IsEmpty())
                    {
                        if (url.IndexOf("?") > -1) url = $"{url}&{data}";
                        else url = $"{url}?{data}";
                    }
                }
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = HTTP_REQUEST_TIMEOUT;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                if (!headers.IsNullOrEmpty())
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                request.Method = method.ToString();
                request.Proxy = null;
                request.ContentType = contextType.IsEmpty() ? HttpContextType.Default : contextType;
                if (parameterType == HttpRequestParameterType.Body)
                {
                    byte[] bPostData = encode.GetBytes(data);
                    //request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                    request.ContentLength = bPostData.Length;
                    reqStream = request.GetRequestStream();
                    reqStream.Write(bPostData, 0, bPostData.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                rst.Code = response.StatusCode;
                rst.Headers = new Dictionary<string, string>();
                var resHeaders = response.Headers;
                foreach (var headerName in resHeaders.AllKeys)
                {
                    if (!rst.Headers.ContainsKey(headerName))
                    {
                        rst.Headers.Add(headerName, resHeaders[headerName]);
                    }
                    else
                    {
                        rst.Headers[headerName] = rst.Headers[headerName] + ";" + resHeaders[headerName];
                    }
                }

                if (response.ContentEncoding != null && response.ContentEncoding.ToString().Contains("gzip"))
                {
                    #region 如果是压缩访问，则需要解压
                    //读取流然后解压
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader respStream = new StreamReader(gzip, encode);
                    rst.Result = respStream.ReadToEnd();
                    respStream?.Close();
                    respStream?.Dispose();
                    #endregion
                }
                else
                {
                    #region 如果是非压缩访问，则不用解压
                    StreamReader respStream = new StreamReader(response.GetResponseStream(), encode);
                    rst.Result = respStream.ReadToEnd();
                    respStream?.Close();
                    respStream?.Dispose();
                    #endregion
                }
            }
            catch (WebException exWeb)
            {
                try
                {
                    HttpWebResponse webResponse = exWeb.Response as HttpWebResponse;
                    rst.Code = webResponse.StatusCode;
                }
                catch (Exception)
                {

                }

                rst.Result = exWeb.Message;
            }
            catch (System.Exception err)
            {
                rst.Code = HttpStatusCode.BadRequest;
                rst.Result = err.Message;
            }
            finally
            {
                request.Clear();
                response.Clear();
                reqStream.Clear();
            }
            return rst;
        }

        public static byte[] GetByteSend(
            string url,
            string data,
            Dictionary<string, string> headers,
            HttpMethod method,
            HttpRequestParameterType parameterType,
            string contextType,
            Encoding encode)
        {
            HttpRequestResult rst = new HttpRequestResult()
            {
                Code = HttpStatusCode.OK
            };
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }
            if (url.IsEmpty())
            {
                rst.Result = "url参数不能为空，必须传入正确的请求地址！";
                rst.Code = HttpStatusCode.BadRequest;
                return null;
            }
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            try
            {
                if (parameterType == HttpRequestParameterType.Query)
                {
                    if (!data.IsEmpty())
                    {
                        if (url.IndexOf("?") > -1) url = $"{url}&{data}";
                        else url = $"{url}?{data}";
                    }
                }
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = HTTP_REQUEST_TIMEOUT;
                //request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                //request.Headers.Add("Accept-Encoding", "gzip, deflate");//压缩访问，response中需要解压
                if (!headers.IsNullOrEmpty())
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                request.Method = method.ToString();
                request.Proxy = null;
                request.ContentType = contextType.IsEmpty()? HttpContextType.Default : contextType;
                if (parameterType == HttpRequestParameterType.Body)
                {
                    byte[] bPostData = encode.GetBytes(data);
                    //request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                    request.ContentLength = bPostData.Length;
                    reqStream = request.GetRequestStream();
                    reqStream.Write(bPostData, 0, bPostData.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                rst.Code = response.StatusCode;
                rst.Headers = new Dictionary<string, string>();
                var resHeaders = response.Headers;
                foreach (var headerName in resHeaders.AllKeys)
                {
                    if (!rst.Headers.ContainsKey(headerName))
                    {
                        rst.Headers.Add(headerName, resHeaders[headerName]);
                    }
                    else
                    {
                        rst.Headers[headerName] = rst.Headers[headerName] + ";" + resHeaders[headerName];
                    }
                }

                if (response.ContentEncoding != null && response.ContentEncoding.ToString().Contains("gzip"))
                {
                    #region 如果是压缩访问，则需要解压
                    //读取流然后解压
                    GZipStream gs = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    MemoryStream stmMemory = new MemoryStream();
                    byte[] bf = new byte[4096];
                    int i;
                    while ((i = gs.Read(bf, 0, bf.Length)) > 0)
                    {
                        stmMemory.Write(bf, 0, i);
                    }
                    byte[] buffer = stmMemory.ToArray();
                    stmMemory.Close();
                    if (gs.CanSeek)
                    {
                        gs.Seek(0, SeekOrigin.Begin);
                    }

                    gs.Close();
                    gs.Dispose();

                    return buffer;
                    #endregion
                }
                else
                {
                    #region 如果是非压缩访问，则不用解压
                    var stream = response.GetResponseStream();

                    //StreamReader respStream = new StreamReader(response.GetResponseStream(), encode);
                    //rst.Result = respStream.ReadToEnd();

                    MemoryStream stmMemory = new MemoryStream();
                    byte[] bf = new byte[4096];
                    int i;
                    while ((i = stream.Read(bf, 0, bf.Length)) > 0)
                    {
                        stmMemory.Write(bf, 0, i);
                    }
                    byte[] buffer = stmMemory.ToArray();
                    stmMemory.Close();
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    stream.Close();
                    stream.Dispose();

                    return buffer;
                    #endregion
                }
            }
            catch (WebException exWeb)
            {
                try
                {
                    HttpWebResponse webResponse = exWeb.Response as HttpWebResponse;
                    rst.Code = webResponse.StatusCode;
                }
                catch (Exception)
                {

                }

                rst.Result = exWeb.Message;
            }
            catch (System.Exception err)
            {
                rst.Code = HttpStatusCode.BadRequest;
                rst.Result = err.Message;
            }
            finally
            {
                request.Clear();
                response.Clear();
                reqStream.Clear();
            }

            return null;
        }

        /// <summary>
        /// 测试给定 url 是否可访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlReachable(string url, HttpMethod method)
        {
            if (string.IsNullOrEmpty(url) || method == null)
            {
                return false;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Method = method.Method; // As per Lasse's comment
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex)
            {
                return false;
            }
        }
    }
    /// <summary>
    /// 请求接口的参数类型 枚举
    /// <para>[2018.08.22,fuzhi.zhao]</para>
    /// </summary>
    public enum HttpRequestParameterType
    {
        [Description("Query")]
        /// <summary>
        /// QueryString类型参数
        /// </summary>
        Query = 1,
        /// <summary>
        /// Body类型参数，一般数据类型为json
        /// </summary>
        [Description("Body")]
        Body = 2
    }
    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode Code { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
    /// <summary>
    /// Http请求常用的ContextType类型
    /// <para>[2018.03.30,fuzhi.zhao]</para>
    /// </summary>
    public class HttpContextType
    {
        /// <summary>
        /// 返回：application/json;适用于FromBody的json数据提交
        /// </summary>
        public static readonly string JSON = "application/json;charset=utf-8";


        /// <summary>
        /// 返回：application/x-www-form-urlencoded;适用普通的POST参数提交
        /// </summary>
        public static readonly string Default = "application/x-www-form-urlencoded;";

        /// <summary>
        /// Excel2003文件，返回：application/vnd.ms-excel；适用的文件类型：xls，xlt，xla
        /// </summary>
        public static readonly string OFFICE_XLS = "application/vnd.ms-excel";

        /// <summary>
        /// Excel2007+文件，返回：application/vnd.openxmlformats-officedocument.spreadsheetml.sheet；适用文件类型：xlsx
        /// </summary>
        public static readonly string OFFICE_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// Word2003文件，返回：application/msword；适用文件类型：doc，dot
        /// </summary>
        public static readonly string OFFICE_DOC = "application/msword";

        /// <summary>
        /// Word2007+文件，返回：application/vnd.openxmlformats-officedocument.wordprocessingml.document；适用文件类型：docx
        /// </summary>
        public static readonly string OFFICE_DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    }
}
