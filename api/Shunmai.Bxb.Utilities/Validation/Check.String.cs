using System;
using System.Collections.Generic;
using System.Linq;

namespace Shunmai.Bxb.Utilities.Validation
{
    /// <summary>
    /// 封装对 <see cref="string"/> 类型参数的常用检查逻辑
    /// </summary>
    public static partial class Check
    {
        /// <summary>
        /// 检查指定参数是否为 <see cref="null"/>、<see cref="string.Empty"/> 或者完全由空白字符组成，若是则抛出 <see cref="ArgumentException"/> 异常
        /// </summary>
        /// <param name="value">待检查的参数值</param>
        /// <param name="parameterName">待检查的参数名称</param>
        public static void Empty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("parameter cannot be null or empty", parameterName);
            }
        }

        /// <summary>
        /// 当指定数据集是否为 <see cref="null"/> 或者为空集合时，抛出 <see cref="ArgumentException"/> 异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">待检查的参数值</param>
        /// <param name="parameterName">待检查的参数名称</param>
        public static void Empty<T>(IEnumerable<T> value, string parameterName)
        {
            if (value == null || !value.Any())
            {
                throw new ArgumentException("parameter cannot be null or empty", parameterName);
            }
        }
    }
}
