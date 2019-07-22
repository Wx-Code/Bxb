using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shunmai.Bxb.Api.Admin.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.Admin.UnitTests.Controllers
{
    public class AdminUserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AdminUserControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Api_AddAdminUser()
        {
            var request = new AdminUserRequest { Username = "admin", Contact = "13521942500" };
            var postData = JsonConvert.SerializeObject(request);
            var client = _factory.CreateClient();
            var result = await client.PutAsync("/admin/user", new StringContent(postData, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }
    }
}
