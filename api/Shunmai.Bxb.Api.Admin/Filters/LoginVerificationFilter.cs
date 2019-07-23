using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.Admin.Constants;
using Shunmai.Bxb.Api.Admin.Models;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Constants;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Common.Exceptions;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Utilities.Extenssions;
using System;
using System.Linq;

namespace Shunmai.Bxb.Api.Admin.Filters
{
    public class LoginVerificationFilter : IActionFilter
    {
        private readonly ILogger<LoginVerificationFilter> _logger;
        private readonly AdminUserService _userService;

        public LoginVerificationFilter(ILogger<LoginVerificationFilter> logger, AdminUserService userService)
        {
            _logger = logger;
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

            var verifyResult = VerifyToken(context, out AdminUser user);
            var failed = verifyResult.Success == false;
            if (failed)
            {
                _logger.LogInformation("request is terminated because valid login token is not presented");
                context.Result = new JsonResult(verifyResult);
                return;
            }

            // 判断用户状态
            if (IsUserStateNormal(user))
            {
                // 将查询到的用户信息缓存入内存中，避免后续操作再次查询数据库
                context.HttpContext.Items[Names.CURRENT_LOGON_USER_CACHE_NAME] = user;
            }
            else
            {
                // 用户状态不正常，停止访问 API
                context.Result = new JsonResult(ApiResponse.OfFailed(ErrorInfo.OfUnauthorized()));
            }
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

        private ApiResponse VerifyToken(ActionExecutingContext context, out AdminUser user)
        {
            user = null;
            var unloginResponse = ApiResponse.OfFailed(ErrorInfo.OfUnauthorized());

            var headers = context.HttpContext.Request.Headers;
            string userId = headers[Headers.USER_ID];
            string tokenString = headers[Headers.TOKEN];
            if (userId.IsEmpty() || tokenString.IsEmpty())
            {
                return unloginResponse;
            }

            user = _userService.QuerySingle(userId.ToInt32());
            if (user == null)
            {
                return unloginResponse;
            }

            try
            {
                var token = new Token(tokenString, user.Salt);
                if (token.Expires < DateTime.Now || token.UserId != userId)
                {
                    return unloginResponse;
                }
                return ApiResponse.OfSuccess(ErrorInfo.OfRequestSuccess(), token);
            }
            catch (InvalidTokenException)
            {
                return unloginResponse;
            }
            catch (ArgumentException)
            {
                return unloginResponse;
            }
        }

        private bool IsUserStateNormal(AdminUser user)
        {
            return user.State == UserState.Normal;
        }
    }
}
