using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// 异常日志格式化工具函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Exception(this ILogger logger, string message, Exception ex)
        {
            logger.LogError($"{message}\n" +
                $"Exception Message: {ex.Message}\n" +
                $"Exception Target: {ex.TargetSite}\n" +
                $"Exception Source: {ex.Source}\n" +
                $"Exception Stack: {ex.StackTrace}\n");
            if (ex.InnerException != null)
            {
                Exception(logger, "=======================inner exception=======================", ex.InnerException);
            }
        }
    }
}
