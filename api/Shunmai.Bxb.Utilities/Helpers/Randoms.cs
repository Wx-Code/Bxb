using System;
using System.Text;
using Util.Helpers;
using Util.Randoms;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public class Randoms
    {
        private const string UPPER_LETTERS = "QWERTYUIOPLKJHGFDSAZXCVBNM";
        private const string LOWER_LETTERS = "qwertyuioplkjhgfdsazxcvbnm";
        private const string LETTERS = UPPER_LETTERS + LOWER_LETTERS;

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
        /// 生成 [0,1) 之间的随机小数
        /// </summary>
        /// <returns></returns>
        public static double Next()
        {
            return new System.Random(CreateRandomSeed()).NextDouble();
        }

        public static string Text(int length, string from)
        {
            Check.Check.EnsureMoreThanZero(length, nameof(length));
            Check.Check.Empty(from, nameof(from));
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append(GetRandomChar(from));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 生成指定长度的随机数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Numbers(int length)
        {
            return Text(length, Const.Numbers);
        }

        /// <summary>
        /// 生成指定长度的字符串（由大小写英文字母组成）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Letters(int length)
        {
            return Text(length, LETTERS);
        }

        /// <summary>
        /// 生成指定长度的随机字符串（由数字和英文字母组成）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string String(int length)
        {
            return Text(length, Const.Numbers + LETTERS);
        }

        /// <summary>
        /// 生成随机的移动电话号码
        /// </summary>
        /// <returns></returns>
        public static string Mobile()
        {
            return $"1{GetRandomChar("3456789")}{Numbers(9)}";
        }
    }
}
