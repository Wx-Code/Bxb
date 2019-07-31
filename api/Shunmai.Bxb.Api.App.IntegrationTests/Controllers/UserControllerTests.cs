using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Constants;
using Shunmai.Bxb.Api.App.Controllers;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Test.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.App.IntegrationTests.Controllers
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;
        private readonly HttpClient _client;
        private readonly BxbContext _dbContext;

        private UserController CreateUserController()
        {
            var logger = _fixture.Server.GetService<ILoggerFactory>().CreateLogger<UserController>();
            var cache = _fixture.Server.GetService<ICache>();
            var userService = _fixture.Server.GetService<UserService>();
            return new UserController(logger, cache, userService);
        }

        public UserControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
            _client = fixture.CreateClient();
            _dbContext = TestSuite.GetDbContext();
        }

        [Fact]
        public async Task Register_ShouldFail_While_SmsCodeDoesNotExist()
        {
            var phone = "13521942500";
            var code = "123456";
            var model = _dbContext.SmsVerificationCode.FirstOrDefault(c => c.Phone == phone && c.VerificationCode == code);
            if (model != null)
            {
                _dbContext.Remove(model);
                _dbContext.SaveChanges();
            }

            var data = new {
                wechatCode = "test_wechat_code",
                phone,
                smsCode = code,
                qrCodeUrl = "test_qr_code_url",
            };
            var result = await TestSuite.PostAsync<JsonResponse<string>>(_client, "/user/register", data);
            Assert.False(result.success);
            Assert.Equal(ErrorInfo.OfRequestFailed().Code, result.errorCode);
        }

        [Fact]
        public void Register_Should_AddUserSuccessfully_While_UserDoesNotExist()
        {
            // 此测试需要使用 Mock 的 WechatService，因为无法在测试时手动获取 code

            var controller = CreateUserController();
            var mock = TestSuite.GetMockWechatService();
            var mockUser = new WechatUserInfo
            {
                Avatar = "test_avatar",
                NickName = "test_nickname",
                OpenId = "test_openId",
                UnionId = "test_unionId",
            };
            mock.Setup(w => w.GetUserInfoByCode(It.IsAny<string>()))
                .Returns(mockUser);
            var wechatService = mock.Object;

            var phone = "13521942500";
            var code = "123456";
            var dbContext = TestSuite.GetDbContext();
            dbContext.Truncate(nameof(SmsVerificationCode), nameof(User), nameof(UserLog));

            var smsCode = dbContext.SmsVerificationCode.Add(new SmsCode { Phone = phone, State = SmsCodeState.Default, VerificationCode = code, CreateTime = DateTime.Now }).Entity;
            dbContext.SaveChanges(); 

            var request = new RegisterRequest
            {
                Phone = "13521942500",
                QrCodeUrl = "test_qrcodeUrl",
                SmsCode = code,
                WechatCode = "test_code",
            };
            var result = controller.Register(request, _fixture.Server.GetService<IOptions<SmsConfig>>(), _fixture.Server.GetService<SmsService>(), wechatService) as JsonResult;
            var response = result.Value as ApiResponse;
            Assert.True(response.Success);

            var user = _dbContext.User.FirstOrDefault(u => u.Phone == phone);
            Assert.NotNull(user);
            Assert.Equal(request.Phone, user.Phone);
            Assert.Equal(request.QrCodeUrl, user.WxCodePhoto);
            Assert.Equal(mockUser.Avatar, user.Avatar);
            Assert.Equal(mockUser.NickName, user.Nickname);
            Assert.Equal(mockUser.OpenId, user.WxOpenId);
            Assert.Equal(mockUser.UnionId, user.WxUnionId);
            var userLog = _dbContext.UserLog.FirstOrDefault(u => u.UserId == user.UserId && u.LogType == UserLogType.Register);
            Assert.NotNull(userLog);

            _dbContext.SmsVerificationCode.Remove(smsCode);
            _dbContext.User.Remove(user);
            _dbContext.UserLog.Remove(userLog);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Login_Should_ExecuteSuccessfully_While_UserExists()
        {
            _dbContext.Truncate(nameof(User), nameof(SmsVerificationCode));
            var user = TestSuite.CreateTestUser();
            _dbContext.User.Add(user);
            var code = _dbContext.SmsVerificationCode.Add(new SmsCode { Phone = user.Phone, VerificationCode = "123456", CreateTime = DateTime.Now }).Entity;
            _dbContext.SaveChanges();

            var result = await TestSuite.PostAsync<JsonResponse<JToken>>(_client, "/user/login", new LoginRequest { Phone = user.Phone, SmsCode = code.VerificationCode });
            Assert.NotNull(result);
            Assert.True(result.success);
            Assert.NotNull(result.data);
            var data = result.data;
            var token = data.Value<string>("token");
            Assert.NotNull(token);
            var cache = _fixture.Server.GetService<ICache>();
            var userId = cache.Get<int>(token);
            Assert.Equal(user.UserId, userId);

            cache.Remove(token);
            _dbContext.User.Remove(user);
            _dbContext.SmsVerificationCode.Remove(code);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetInfo_Should_ReturnUserInfo_While_UserHasLogin()
        {
            _dbContext.Truncate(nameof(User), nameof(SmsVerificationCode));
            var user = TestSuite.CreateTestUser();
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
            var token = Guid.NewGuid().ToString("N");
            var cache = _fixture.Server.GetService<ICache>();
            cache.Set(token, user.UserId, null);

            var result = await _client.GetAsync<JsonResponse<UserExt>>("/user", null, new Dictionary<string, string> { { Headers.TOKEN, token } });
            Assert.NotNull(result);
            Assert.True(result.success);
            Assert.NotNull(result.data);
            Assert.Equal(user.UserId, result.data.UserId);

            cache.Remove(token);
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}
