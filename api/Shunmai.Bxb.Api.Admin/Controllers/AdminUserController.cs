using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.Admin.Constants;
using Shunmai.Bxb.Api.Admin.Models;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Dto;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;
using System;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("admin/user")]
    public class AdminUserController: AdminBaseController
    {
        private readonly ILogger<AdminUserController> _logger;
        private readonly AdminUserService _userService;

        public AdminUserController(ILogger<AdminUserController> logger, AdminUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <returns></returns>
        /// <param name="data">Data.</param>
        public IActionResult Put([FromBody] AdminUserRequest data)
        {
            var user = data.MapTo<AdminUser>();
            user.Password = Defaults.ADMIN_USER_DEFAULT_PASSWORD;
            var id = _userService.AddUser(user, out string message);
            return id > 0
                ? Success(id)
                : Failed(message);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AdminUserRequest data)
        {
            var user = data.MapTo<AdminUser>();
            var success = _userService.Update(user, out string message);
            return success
                ? Success()
                : Failed(message);
        }

        [HttpGet]
        public IActionResult Get([FromQuery]AdminUserQuery query)
        {
            var (total, list) = _userService.QueryPage(query);
            var data = new ListResponse<AdminUserResponse>
            {
                List = list.MapToList<AdminUserResponse>(),
                Total = total
            };
            return Success(data);
        }

        [HttpPost("pwd")]
        public IActionResult UpdatePassword([FromBody]PasswordUpdateRequest data)
        {
            var success = _userService.UpdatePassword(data.UserId, data.Password, data.OldPassword);
            return success
                ? Success()
                : Failed("旧密码输入不正确");
        }

        [SkipLoginVerification]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminUserRequest data)
        {
            var loginFailedResponse = ApiResponse.OfFailed(ErrorInfo.OfLoginFailed("用户名或密码错误"));

            var user = _userService.QuerySingle(data.Username);
            if (user == null)
            {
                return Json(loginFailedResponse);
            }

            var postPwd = _userService.EncryptPwd(user.Username, data.Password, user.Salt);
            if (postPwd != user.Password)
            {
                return Json(loginFailedResponse);
            }
            
            // 判断用户状态
            if (user.State == UserState.Disabled)
            {
                return Json(loginFailedResponse);
            }

            // 登录成功
            var expires = DateTime.Now.AddSeconds(Defaults.API_TOKEN_EXPIRE_SECONDS);
            var token = new Token(user.AdminUserId.ToString(), expires, user.Salt);
            var response = ApiResponse.OfSuccess(
                ErrorInfo.OfLoginSuccess(),
                new {
                    token = token.Encrypt(),
                    userId = user.AdminUserId,
                    username = user.Username,
                });

            return Json(response);
        }
    }
}
