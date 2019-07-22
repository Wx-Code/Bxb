using Shunmai.Bxb.Utilities.Extenssions;
using System;

namespace Shunmai.Bxb.Utilities
{
    /// <summary>
    /// Object 工具方法
    /// </summary>
    public  class ObjectUtils
    {

        /// <summary>
        /// 确保指定参数的值不等于 null ，否则抛出 <see cref="ArgumentNullException"/> 类型的异常
        /// </summary>
        /// <param name="value">待检查的参数值</param>
        /// <param name="argumentName">待检查的参数名称</param>
        /// <param name="message">异常消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void EnsureNotNull(object value, string argumentName, string message = null)
        {
            if (value == null) 
            {
                message = message ?? $"{argumentName} must be specified";
                throw new ArgumentNullException(argumentName, message);
            }
        }

        /// <summary>
        /// 确保指定参数的值不等于 null 或者 string.Empty，否则抛出 <see cref="ArgumentNullException"/> 类型的异常
        /// </summary>
        /// <param name="value">待检查的参数值</param>
        /// <param name="argumentName">待检查的参数名称</param>
        /// <param name="message">异常消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void EnsureNotNullOrEmpty(string value, string argumentName, string message = null)
        {
            if (value.IsNullOrEmpty())
            {
                message = message ?? $"{argumentName} must be specified";
                throw new ArgumentNullException(argumentName, message);
            }
        }

        /// <summary>
        /// 确保指定参数的值大于 0，否则抛出 <see cref="ArgumentException"/> 类型的异常
        /// </summary>
        /// <typeparam name="T">指定参数的类型，必须是实现了 <see cref="IComparable"/> 接口的且不可以为 null 的类型</typeparam>
        /// <param name="value">待检查的参数的值</param>
        /// <param name="argumentName">等检查的参数名称</param>
        /// <param name="message">异常消息</param>
        /// <exception cref="ArgumentException"></exception>
        public static void EnsureMoreThanZero<T>(T value, string argumentName, string message = null) where T : struct, IComparable
        {
            var temp = value as IComparable;
            var zero = Convert.ChangeType(0, typeof(T));
            var result = temp.CompareTo(zero);
            if (result != 1)
            {
                message = message ?? $"{argumentName} must be more than zero";
                throw new ArgumentException(message, argumentName);
            }
        }
    }
}
