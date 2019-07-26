using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Api.App.Models.Response;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utilities.Helpers;
using System;

namespace Shunmai.Bxb.Api.App.Controllers
{
    public class UserController : AppBaseController
    {
        private readonly ILogger _logger;
        private readonly ICache _cache;
        private readonly UserService _userService;

        public UserController(
            ILogger<UserController> logger
            , ICache cache
            , UserService userService
        )
        {
            _logger = logger;
            _cache = cache;
            _userService = userService;
        }

        private string GenerateToken(User user)
        {
            var firstMd5 = Encrypt.Md5By32($"{user.UserId}:{user.WxOpenId}:{Guid.NewGuid()}");
            var salt = DateTime.Now.ToLongTimeString();
            return Encrypt.Md5By32($"{firstMd5}-{salt}");
        }

        private IActionResult CacheUser(User user)
        {
            var token = GenerateToken(user);
            var key = token;
            var success = _cache.Set(key, user.UserId, Constants.Defaults.TOKEN_EXPIRES);
            if (success == false)
            {
                _logger.LogError("Cache token failed");
                return Failed();
            }
            return Success(new
            {
                token,
                user = user.MapTo<UserResponse>(),
                expires = Constants.Defaults.TOKEN_EXPIRES.TotalMilliseconds
            });
        }

        [SkipLoginVerification]
        [HttpPost("register")]
        public IActionResult Register(
            [FromBody]RegisterRequest request
            , [FromServices]IOptions<SmsConfig> options
            , [FromServices]SmsService smsService
            , [FromServices]WechatService wechatService
        )
        {
            var valid = smsService.Validate(request.Phone, request.SmsCode, options.Value.ExpiresMinutes * 60);
            if (valid == false)
            {
                return Failed("短信验证码有误");
            }

            var wechatUser = wechatService.GetUserInfoByCode(request.WechatCode);
            if (wechatUser == null)
            {
                _logger.LogError($"Get wechat user info failed.");
                return Failed("拉取微信用户信息失败");
            }

            var user = new User
            {
                Avatar = wechatUser.Avatar,
                Nickname = wechatUser.NickName,
                Realname = wechatUser.NickName,
                Phone = request.Phone,
                WxCodePhoto = request.QrCodeUrl,
                WxOpenId = wechatUser.OpenId,
                WxUnionId = wechatUser.UnionId,
            };
            var success = _userService.AddUser(user, out var message);
            return success ? Success(message) : Failed(message);
        }
    }
}
