using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Services;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Api.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WechatController : AppBaseController
    {
        private readonly ILogger _logger;
        private readonly WechatService _wechatService;
        private readonly UserService _userService;

        public WechatController(
            ILogger<WechatController> logger
            , WechatService wechatService
            , UserService userService
        )
        {
            _logger = logger;
            _wechatService = wechatService;
            _userService = userService;
        }

        [SkipLoginVerification]
        [HttpGet("jsapi/config")]
        public IActionResult GetJssdkConfig([FromQuery, Required]string url)
        {
            var config = _wechatService.GetJssdkConfig(url);
            return Success(config);
        }
    }
}
