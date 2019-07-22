using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;

namespace Shunmai.Bxb.Common.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            var request = context.Request;
            var requestLog = await FormatRequest(context.Request);
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    StringBuilder logBuilder = new StringBuilder();
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
                }               

                var responseLog = await FormatResponse(context.Response);

                watch.Stop();

                var log = new StringBuilder();
                log.AppendLine($"[{request.Method} {watch.ElapsedMilliseconds}ms] " +
                    $" {request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString.ToUriComponent()}");
                log.Append(requestLog);
                log.Append(responseLog);
                _logger.LogInformation(log.ToString());

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Request Headers: ");
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    builder.AppendLine($"\t{header.Key}: {header.Value}");
                }
            }

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            request.EnableRewind();
            try
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                var requestBody = Encoding.UTF8.GetString(buffer);
                builder.AppendLine($"Request Body: {requestBody}");
                request.Body.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                builder.AppendLine($"{ex.Message}");
                builder.AppendLine($"{ex.StackTrace}");
            }

            return builder.ToString();
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            try
            {
                //We need to read the response stream from the beginning...
                response.Body.Seek(0, SeekOrigin.Begin);

                //...and copy it into a string
                string text = await new StreamReader(response.Body).ReadToEndAsync();

                //We need to reset the reader for the response so that the client can read it.
                response.Body.Seek(0, SeekOrigin.Begin);

                //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
                return $"Status: {response.StatusCode}{Environment.NewLine}Response: {text}{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                return $"Exception occurs on logging response{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}Stack: {ex.StackTrace}";
            }
        }
    }
}
