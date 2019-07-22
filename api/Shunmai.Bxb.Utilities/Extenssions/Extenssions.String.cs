using Newtonsoft.Json;
using System;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return IsEmpty(value) == false;
        }

        public static int ToInt32(this string value, int def = default(int))
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return def;
        }

        public static long ToInt64(this string value, long def = default(long))
        {
            if (long.TryParse(value, out long result))
            {
                return result;
            }
            return def;
        }

        public static DateTime ToDateTime(this string value)
        {
            if (DateTime.TryParse(value, out DateTime result))
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 将json字符串转化成指定的对象
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string item)
        {
            try
            {
                T res = JsonConvert.DeserializeObject<T>(item);
                return res;
            }
            catch (Exception ex)
            {
                return default(T);
            }            
        }

    }
}
