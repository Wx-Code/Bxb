using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Shunmai.Bxb.Timer.Utils
{
    internal static class LoggingHelper
    {
        internal static void LogException(ILogger logger, Exception ex)
        {
            logger.LogInformation("Exception occurs");
            logger.LogInformation($"Message:  {ex.Message}");
            logger.LogInformation($"Source: {ex.Source}");
            logger.LogInformation($"TargetSite: {ex.TargetSite}");
            logger.LogInformation($"StackTrace: {ex.StackTrace}");
        }

        /// <summary>
        /// 封装打印任务执行所花时间的日志的模板代码
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="logger"></param>
        /// <param name="jobName"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        internal static Func<TResult> WrapTaskTimeLog<TResult>(ILogger logger, string jobName, Func<TResult> function)
        {
            return () =>
            {
                var stopwatch = new Stopwatch();
                logger.LogInformation(Constants.LOG_SEPERATOR_LINE);
                logger.LogInformation($"{jobName} started");
                stopwatch.Start();

                var result = function();

                stopwatch.Stop();
                logger.LogInformation($"{jobName} finished in {stopwatch.ElapsedMilliseconds}ms");
                logger.LogInformation(Constants.LOG_SEPERATOR_LINE);

                return result;
            };
        }

        /// <summary>
        /// 封装打印任务执行所花时间的日志的模板代码
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="jobName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static Action WrapTaskTimeLog(ILogger logger, string jobName, Action action)
        {
            return () =>
            {
                var stopwatch = new Stopwatch();
                logger.LogInformation(Constants.LOG_SEPERATOR_LINE);
                logger.LogInformation($"{jobName} started");
                stopwatch.Start();

                action();

                stopwatch.Stop();
                logger.LogInformation($"{jobName} finished in {stopwatch.ElapsedMilliseconds}ms");
                logger.LogInformation(Constants.LOG_SEPERATOR_LINE);
            };
        }
    }
}
