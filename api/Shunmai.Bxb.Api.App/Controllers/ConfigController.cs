using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Api.App.Models.Response;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Common.Constans;
using Shunmai.Bxb.Common.Extensions;

namespace Shunmai.Bxb.Api.App.Controllers
{
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
        [HttpGet("platwallet/addr")]
        public IActionResult GetPlatformWalletAddressList()
        {
            var data = _systemConfigService.GetConfig<List<PaltWalletAddressResponse>>(SystemConfigNames.PLATFORM_WALLET_ADDRESS);
            return Success(data);
        }

        [SkipLoginVerification]
        [HttpGet("customer/service")]
        public IActionResult GetWXCustomerList()
        {
            var data = _systemConfigService.GetServiceCustomer();
            return Success(data);
        }

    }
}