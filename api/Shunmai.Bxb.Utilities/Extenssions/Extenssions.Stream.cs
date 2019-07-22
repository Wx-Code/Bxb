using System;
using System.IO;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        /// <summary>
        /// 调用Close和Dispose，并捕获异常
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool Clear(this Stream s)
        {
            if (s == null)
            {
                return true;
            }
            try
            {
                s.Close();
                s.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
