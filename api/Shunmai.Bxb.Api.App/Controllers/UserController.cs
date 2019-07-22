using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Api.App.Models.Response;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Utilities.Extenssions;
using System;

namespace Shunmai.Bxb.Api.App.Controllers
{
    public class UserController : AppBaseController
    {
        private readonly ILogger _logger;
        private readonly ICache _cache;
        private readonly UserService _userService;
        private readonly WechatService _wechatService;

        public UserController(
            ILogger<UserController> logger
            , ICache cache
            , UserService userService
            , WechatService wechatService
        )
        {
            _logger = logger;
            _cache = cache;
            _userService = userService;
            _wechatService = wechatService;
        }

        [SkipLoginVerification]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            //var openId = _wechatService.GetOpenId(request.Code);
            //var user = _userService.FindByOpenId(openId);
            //if (user != null)
            //{
            //    // 登录成功
            //    return CacheUser(user);
            //}

            //var wechatUser = _wechatService.GetUserInfo(openId);
            //if (wechatUser == null)
            //{
            //    // 生成二维码，引导用户关注公众号
            //    return GetQRCodeResult(request.ActivityId, request.GroupId, user);
            //}

            //wechatUser.GroupId = request.GroupId;
            //// 用户注册
            //return Register(wechatUser);
            throw new NotImplementedException();
        }

        private IActionResult Register(WechatUserInfo wechatUser)
        {
            var success = _userService.AddUser(wechatUser, out var user);
            if (success == false)
            {
                _logger.LogError("Register failed");
                return Failed();
            }

            return CacheUser(user);
        }

        private string GenerateToken(User user)
        {
            //var firstMd5 = Encrypt.Md5By32($"{user.UserId}:{user.WxOpenId}:{Guid.NewGuid()}");
            //var salt = DateTime.Now.ToLongTimeString();
            //return Encrypt.Md5By32($"{firstMd5}-{salt}");
            return string.Empty;
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
    }
}
