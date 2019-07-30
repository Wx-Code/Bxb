using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.ComponentModel;

namespace Shunmai.Bxb.Services.Utils
{
    public sealed class IdGenerator
    {
        /// <summary>
        /// 生成的ID类型
        /// </summary>
        public enum IdType
        {
            [Description("订单号")]
            OrderId = 11,
        }
        /// <summary>
        /// 生成唯一的单号
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static long Generate(IdType t)
        {
            DateTime now = DateTime.Now;
            long ts = now.TotalSeconds() - now.Date.TotalSeconds();
            string year = now.Year.ToString().Substring(2);
            int day = now.DayOfYear;
            int rd = Randoms.Next(999);
            string IdStr = $"{((int)t)}{rd.ToString("000")}{year}{day.ToString("000")}{ts.ToString("00000")}";
            return IdStr.ToInt64();
        }
        /// <summary>
        /// 生成唯一的交易码
        /// </summary>
        /// <returns></returns>
        public static string GenerateTradeCode()
        {
            int prefix = Randoms.Next(16, 1);
            long ts = DateTime.Now.TotalSeconds();
            int postfix = Randoms.Next(4096, 256);
            string result = $"{Convert.ToString(prefix, 16)}{Convert.ToString(ts, 16)}{Convert.ToString(postfix, 16)}";
            return result.ToUpper();
        }
    }
}
