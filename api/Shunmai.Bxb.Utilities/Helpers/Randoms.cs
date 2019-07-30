using System;
using System.Text;
using Util.Helpers;
using Util.Randoms;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public class Randoms
    {
        /// <summary>
        /// 随机数的偏移量生成
        /// </summary>
        /// <returns></returns>
        private static int CreateRandomSeed() => Guid.NewGuid().GetHashCode();

        private static IRandomNumberGenerator _random = new RandomNumberGenerator();

        private static string GetRandomChar(string text)
        {
            return text[_random.Generate(0, text.Length)].ToString();
        }

        /// <summary>
        /// 生成随机数,允许指定最大值和最小值。 
        /// </summary>
        /// <param name="min"> 起始数字，可选参数，不传递时为1 </param>
        /// <param name="max"> 结束数字 </param>
        /// <returns></returns>
        public static int Next(int max, int min = 1)
        {
            return _random.Generate(min, max);
        }

        /// <summary>
        /// 生成指定长度的随机数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Numbers(int length)
        {
            Check.Check.EnsureMoreThanZero(length, nameof(length));
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append(GetRandomChar(Const.Numbers));
            }
            return builder.ToString();
        }
    }
}
