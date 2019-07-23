using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shunmai.Bxb.Common.Dto;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public class CommonCtrollerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CommonCtrollerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UploadFile_Should_Return_FullUrl()
        {
            var filename = "test-upload.jpg";
            var imageFilename = PathHelper.MapPath(filename);

            using (var fileStream = new FileStream(imageFilename, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            using (var content = new MultipartFormDataContent($"Upload----{DateTime.Now.ToString(CultureInfo.InvariantCulture)}"))
            using (var client = _factory.CreateClient())
            {
                content.Add(new StreamContent(fileStream), "test-image", filename);
                var response = await client.PostAsync("/common/wechat/qrcode", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ApiResponse>(json);
                Assert.NotNull(res);
                Assert.NotNull(res.Data);
                var url = res.Data.ToString();
                Assert.NotEmpty(url);
            }
        }
    }
}
