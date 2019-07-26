using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public class CommonCtrollerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;
        private readonly HttpClient _client;

        public CommonCtrollerTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
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
                var res = JsonConvert.DeserializeObject<JsonResponse>(json);
                Assert.NotNull(res);
                Assert.NotNull(res.data);
                var url = res.data.ToString();
                Assert.NotEmpty(url);
            }
        }

        [Fact]
        public async Task SendSmsCode_Should_WorkWell()
        {
            var json = new { phone = "13521942500" };
            var result = await TestSuite.PostAsync<JsonResponse>(_client, "/common/sms/code", json);
            Assert.True(result.success);
        }
    }
}
