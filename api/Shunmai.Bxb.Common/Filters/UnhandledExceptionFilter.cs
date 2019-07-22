using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Common.Dto;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Text;

namespace Shunmai.Bxb.Common.Filters
{
    public class UnhandledExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var logBuilder = new StringBuilder();

            // TODO: 完成全局异常处理逻辑
            var request = context.HttpContext.Request;
            logBuilder.AppendLine($"[{request.Method}] {request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString.ToUriComponent()}");
            try
            {
                request.EnableRewind();
                request.Body.Seek(0, 0);

                var stream = request.Body;
                byte[] buffer = new byte[request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                string bodyStr = Encoding.UTF8.GetString(buffer);
                if (!bodyStr.IsEmpty())
                {
                    logBuilder.AppendLine($"Request Body: {Encoding.UTF8.GetString(buffer)}");
                }
            }
            catch (Exception)
            {

            }

            var ex = context.Exception;
            logBuilder.AppendLine($"Exception Source: {ex.Source}");
            logBuilder.AppendLine($"Exception Message: {ex.Message}");
            logBuilder.AppendLine($"Exception TargetSite: {ex.TargetSite}");
            logBuilder.AppendLine($"Exception StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                logBuilder.AppendLine($"Exception InnerException StackTrace: {ex.InnerException.StackTrace}");
                logBuilder.AppendLine($"Exception InnerException Message: {ex.InnerException.Message}");
            }
            _logger.LogError(logBuilder.ToString());

            var response = ApiResponse.OfFailed(ErrorInfo.OfServerInteralError());
            context.Result = new JsonResult(response);
            context.ExceptionHandled = true;
        }
    }
}
