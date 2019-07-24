using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.Admin.Models;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Constans;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Extenssions;

namespace Shunmai.Bxb.Api.Admin.Controllers
{
    public class ConfigController : AdminBaseController
    {
        private readonly ILogger<ConfigController> _logger;

        private readonly SystemConfigService _systemConfigService;

        public ConfigController(ILogger<ConfigController> logger, SystemConfigService systemConfigService)
        {
            _logger = logger;
            _systemConfigService = systemConfigService;
        }

        [HttpGet("{configName}")]
        public IActionResult Get(string configName)
        {
            var data = _systemConfigService.QuerySigle(configName);
            return Success(data);
        }

        [HttpPost]
        public IActionResult AddOrUpdate([FromBody]ConfigRequst request)
        {
            var config = request.MapTo<SystemConfig>();
            config.CreateUser = LogonUser.Username;
            var success = _systemConfigService.AddOrUpdateConfig(config);
            return success ? Success() : Failed();
        }

    }
}