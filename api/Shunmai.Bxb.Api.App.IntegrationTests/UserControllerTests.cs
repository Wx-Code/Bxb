using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Controllers;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Test.Common.Models;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.App.IntegrationTests
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
            var result = await TestSuite.PostAsync<JsonResponse>(_client, "/user/register", data);
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
            dbContext.Truncate(nameof(SmsVerificationCode));
            dbContext.Truncate(nameof(User));
            dbContext.Truncate(nameof(UserLog));

            var smsCode = dbContext.SmsVerificationCode.Add(new SmsCode { Phone = phone, State = SmsCodeState.Default, VerificationCode = code }).Entity;
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
    }
}
