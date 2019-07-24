using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Services;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    [Route("api/config")]
    public class ConfigController : AdminBaseController
    {
        private readonly ILogger<ConfigController> _logger;

        private readonly SystemConfigService _systemConfigService;

        public ConfigController(ILogger<ConfigController> logger, SystemConfigService systemConfigService)
        {
            _logger = logger;
            _systemConfigService = systemConfigService;
        }

        //[HttpGet("GetPlatWalletConfig")]
        //public IActionResult GetPlatWalletConfig()
        //{
        //    var config = _systemConfigService.GetPlatWalletAddrConfigList();
        //    return Success(config);
        //}



    }
}