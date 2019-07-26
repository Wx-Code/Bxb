﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.App.Models.Response;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Constans;

namespace Shunmai.Bxb.Api.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : AppBaseController
    {

        private readonly ILogger _logger;
        private readonly SystemConfigService _systemConfigService;

        public ConfigController(ILogger<ConfigController> logger,SystemConfigService systemConfigService)
        {
            _logger = logger;
            _systemConfigService = systemConfigService;
        }

        [SkipLoginVerification]
        [HttpGet("GetPlatformWalletAddressList")]
        public IActionResult GetPlatformWalletAddressList()
        {
            var data = _systemConfigService.GetConfig<List<PaltWalletAddressResponse>>(SystemConfigNames.PLATFORM_WALLET_ADDRESS);
            return Success(data);
        }

        [SkipLoginVerification]
        [HttpGet("GetWXCustomerList")]
        public IActionResult GetWXCustomerList()
        {
            var data = _systemConfigService.GetConfig<WXCustomerInfoResponse>(SystemConfigNames.CUSTOMER_SERVICE);
            return Success(data);
        }

    }
}