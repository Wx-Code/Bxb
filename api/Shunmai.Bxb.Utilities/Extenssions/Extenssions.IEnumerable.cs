using System.Collections.Generic;
using System.Linq;

namespace Shunmai.Bxb.Utilities.Extenssions
{
    public static partial class Extenssions
    {
        /// <summary>
        /// 判断集合是否为空（不包含元素或者为 null 均返回true）；
        /// code:bool empty=list.IsNullOrEmpty();
        /// <para>[2017.12.13,fuzhi.zhao]</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value) => value == null || !value.Any();

    }
}
