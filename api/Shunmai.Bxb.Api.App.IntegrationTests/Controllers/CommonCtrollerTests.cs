using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services.Constans;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Test.Common.Models;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.App.IntegrationTests.Controllers
{
    public class CommonCtrollerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;
        private readonly HttpClient _client;
        private readonly BxbContext _dbContext;
        private readonly object _lockKey = new object();

        public CommonCtrollerTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
            _dbContext = TestSuite.GetDbContext();
        }

        [Fact]
        public async Task UploadFile_Should_Return_FullUrl()
        {
            var filename = "test-upload.jpg";
            var imageFilename = PathHelper.MapPath(filename);

            using (var fileStream = new FileStream(imageFilename, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            using (var content = new MultipartFormDataContent($"Upload----{DateTime.Now.ToString(CultureInfo.InvariantCulture)}"))
            {
                content.Add(new StreamContent(fileStream), "test-image", filename);
                var response = await _client.PostAsync("/common/wechat/qrcode", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<JsonResponse<string>>(json);
                Assert.NotNull(res);
                Assert.NotNull(res.data);
            }
        }

        [Fact]
        public async Task SendSmsCode_Should_WorkWell()
        {
            var json = new { phone = "13521942500" };
            var result = await TestSuite.PostAsync<JsonResponse<string>>(_client, "/common/sms/code", json);
            Assert.True(result.success);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456")]
        [InlineData("\"_$%{}\"")]
        [InlineData("abcd\naced\r\n")]
        public async Task GetTradeRules_Should_WorkWell(string content)
        {
            _dbContext.Truncate(nameof(SystemConfig));
            var entity = await _dbContext.SystemConfig.FindAsync(SystemConfigNames.TRADE_RULES);
            if (entity == null)
            {
                entity = new SystemConfigExt
                {
                    ConfigName = SystemConfigNames.TRADE_RULES,
                    ConfigValue = content,
                    CreateUser = "test_user",
                    Remark = "test_remark"
                };
            }
            _dbContext.SystemConfig.Add(entity);
            _dbContext.SaveChanges();

            var result = await _client.GetAsync<JsonResponse<string>>("/common/trade/rules");
            Assert.True(result.success);
            Assert.Equal(content, result.data);

            _dbContext.SystemConfig.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
