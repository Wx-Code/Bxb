using System;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        /// <summary>
        ///  DateTime(1970, 1, 1, 0, 0, 0);
        /// </summary>
        private static readonly DateTime DATETIME_START = new DateTime(1970, 1, 1, 0, 0, 0);
        public static string ToFormat(this DateTime value,string f="yyyy-MM-dd HH:mm:ss")
        {
            return value.ToString(f);
        }
        public static string ToFormatString(this DateTime value)
        {
            return value.ToFormat("yyyyMMddHHmmss");
        }
        /// <summary>
        /// 获取当前时间的时间戳（秒）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long TotalSeconds(this DateTime value) => Convert.ToInt64((value - DATETIME_START).TotalSeconds);
        /// <summary>
        /// 获取当前时间的时间戳（毫秒）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long TotalMillSeconds(this DateTime value) => Convert.ToInt64((value - DATETIME_START).TotalMilliseconds);

    }
}
