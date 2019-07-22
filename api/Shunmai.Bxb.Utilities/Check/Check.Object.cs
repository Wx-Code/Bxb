using System;

namespace Shunmai.Bxb.Utils.Check
{
    /// <summary>
    /// 封装对 <see cref="object"/> 类型的参数的常用检查方法
    /// </summary>
    public static partial class Check
    {
        /// <summary>
        /// 检查指定参数是否为 <see cref="null"/>，若是则抛出 <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="value">待检查的参数值</param>
        /// <param name="parameterName">待检查的参数名称</param>
        public static void Null(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
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

        /// <summary>
        /// 检查给定参数的值是否大于 <see cref="DateTime.Now"/>，若不是则抛出 <see cref="ArgumentException"/> 异常
        /// </summary>
        /// <param name="time">待检查参数的值</param>
        /// <param name="argumentName">参数名称</param>
        /// <param name="message">异常消息</param>
        public static void CreaterThanCurrentTime(DateTime time, string argumentName, string message = null)
        {
            if (time < DateTime.Now)
            {
                throw new ArgumentException(message ?? "parameter must greater than current time", argumentName);
            }
        }
    }
}
