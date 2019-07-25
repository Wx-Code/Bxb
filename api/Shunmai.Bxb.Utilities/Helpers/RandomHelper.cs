using System;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public class RandomHelper
    {
        /// <summary>
        /// 随机数的偏移量生成
        /// </summary>
        /// <returns></returns>
        private static int CreateRandomSeed() => Guid.NewGuid().GetHashCode();

        /// <summary>
        /// 生成随机数,允许指定最大值和最小值。 
        /// </summary>
        /// <param name="min"> 起始数字，可选参数，不传递时为1 </param>
        /// <param name="max"> 结束数字 </param>
        /// <returns></returns>
        public static int Next(int max, int min = 1)
        {
            Random random = new Random(CreateRandomSeed());
            return random.Next(min, max);
        }
    }
}
