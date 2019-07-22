using Newtonsoft.Json;
using System;
using System.Linq;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string JsonSerialize(this object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        private static string ToFormatString(this object value, Func<string, string, string> formatFunc, string seperator)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var type = value.GetType();
            if (type == typeof(string))
            {
                return type.ToString();
            }

            var props = type.GetProperties();
            var pairs = props.Select(p => formatFunc(p.Name, p.GetValue(value)?.ToString()));
            return string.Join(seperator, pairs);
        }

        /// <summary>
        /// 将实体中的公共属性及其值，按照 key=value&key1=value1 的格式拼接成字符串并返回
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToQueryString(this object value)
        {
            return ToFormatString(value, (n, v) => $"{n}={v}", "&");
        }

        /// <summary>
        /// 用于将对象的值打印成方便阅读的日志格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLogFormatString(this object value)
        {
            return ToFormatString(value, (n, v) => $"{n}={v}", ", ");
        }
        

    }
}
