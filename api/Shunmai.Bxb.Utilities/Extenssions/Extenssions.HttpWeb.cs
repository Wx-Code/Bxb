using System;
using System.Net;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        /// <summary>
        /// 调用Abort并捕获异常
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool Clear(this HttpWebRequest request)
        {
            if (request == null)
            {
                return true;
            }
            try
            {
                request.Abort();
                request = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 调用Close和Dispose，并捕获异常
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool Clear(this HttpWebResponse response)
        {
            if (response == null)
            {
                return true;
            }
            try
            {
                response.Close();
                response.Dispose();
                response = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
