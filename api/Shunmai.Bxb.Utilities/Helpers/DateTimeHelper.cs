using System;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 将给定时间与当前时间对比，返回如：刚刚、几分钟前、几天前、12-21、2019-01-11 这样的字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetTimeSpan(DateTime dateTime)
        {
            var span = DateTime.Now - dateTime;
            if (span.TotalSeconds < 60)
            {
                return "刚刚";
            }
            if (span.TotalMinutes < 10)
            {
                return "几分钟前";
            }
            if (span.TotalMinutes < 60)
            {
                return $"{(int)span.TotalMinutes} 分钟前";
            }
            if (span.TotalHours < 24)
            {
                return $"{(int)span.TotalHours} 小时前";
            }
            if (span.TotalDays < 10)
            {
                return "几天前";
            }
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
