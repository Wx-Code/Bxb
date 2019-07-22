using System;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        /// <summary>
        /// 确定给定类型是否为可空类型（包括 <see cref="object"/> 和 <see cref="Nullable"/> 类型）
        /// </summary>
        /// <param name="type">待判断的类型实例</param>
        /// <returns>指定类型为可空类型时返回 <c>true</c>，否则返回 <c>false</c></returns>
        public static bool IsNullable(this Type type)
        {
            ObjectUtils.EnsureNotNull(type, nameof(type));

            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// 返回表示指定类型是否为 <see cref="Nullable"/> 类型的 <see cref="bool"/> 值
        /// </summary>
        /// <param name="type">待判断类型实例</param>
        /// <returns>指定类型为 <see cref="Nullable"/> 类型时返回 <c>true</c>，否则返回 <c>false</c></returns>
        public static bool IsNullableType(this Type type)
        {
            ObjectUtils.EnsureNotNull(type, nameof(type));

            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}
