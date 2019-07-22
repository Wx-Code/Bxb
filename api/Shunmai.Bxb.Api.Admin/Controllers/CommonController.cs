using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Utilities.Helpers;
using System.Linq;
using System.Reflection;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("/common")]
    public class CommonController: AdminBaseController
    {
        private readonly ILogger _logger;

        private const string ENUM_ASSEMBLY_NAME = "Shunmai.Bxb.Entities";

        public CommonController(ILogger<CommonController> logger)
        {
            _logger = logger;
        }

        [SkipLoginVerification]
        [HttpGet("types/{name}")]
        public IActionResult GetEnumDict(string name)
        {
            var assembly = Assembly.Load(ENUM_ASSEMBLY_NAME);
            var enumType = assembly.GetTypes().Where(t => t.IsEnum).FirstOrDefault(t => t.Name == name);
            var dict = Enums.GetValueDescDict(enumType);
            var result = dict.Select(c => new { label = c.Value, value = c.Key });
            return Success(result);
        }
    }
}
