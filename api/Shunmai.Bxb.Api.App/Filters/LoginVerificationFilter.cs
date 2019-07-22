using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Constants;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Dto;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Utilities.Extenssions;
using System.Linq;

namespace Shunmai.Bxb.Api.App.Filters
{
    public class LoginVerificationFilter : IActionFilter
    {
        private readonly ILogger _logger;
        private readonly ICache _cache;
        private readonly UserService _userService;

        public LoginVerificationFilter(ILogger<LoginVerificationFilter> logger, ICache cache, UserService userService)
        {
            _logger = logger;
            _cache = cache;
            _userService = userService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("executing login verification");
            if (HasSkipLoginVerificationAttribute(context))
            {
                _logger.LogInformation("login verification is terminated because the action has SkipLoginVerificationAttribute");
                return;
            }

            var verifyResult = VerifyToken(context, out User user);
            if (verifyResult.Success)
            {
                context.HttpContext.Items[Names.USER_CACHE] = user;
            }
            else
            {
                _logger.LogInformation("request is terminated because valid login token is not presented");
                context.Result = new JsonResult(verifyResult);
            }
        }

        private ApiResponse VerifyToken(ActionExecutingContext context, out User user)
        {
            user = null;

            var illegalResponse = ApiResponse.OfFailed(ErrorInfo.OfIllegalRequest());
            var headers = context.HttpContext.Request.Headers;
            string token = headers[Headers.TOKEN];
            if (token.IsEmpty())
            {
                return illegalResponse;
            }

            var key = token;
            var userId = _cache.Get<int>(key);
            if (userId <= 0)
            {
                return illegalResponse;
            }

            user = _userService.FindById(userId);
            if (user == null)
            {
                _cache.Remove(token);
                return illegalResponse;
            }

            return ApiResponse.OfSuccess(ErrorInfo.OfRequestSuccess());
        }

        private bool HasSkipLoginVerificationAttribute(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
                return actionAttributes.Any(a => a is SkipLoginVerificationAttribute);
            }

            return false;
        }
    }
}
