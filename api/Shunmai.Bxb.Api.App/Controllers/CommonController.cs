using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Common.Enums;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Common.Constans;
using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utils.Helpers;
using System.Linq;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Services.Models.IET;
using System.Threading.Tasks;
using System.Reflection;
using Shunmai.Bxb.Utilities.Helpers;

namespace Shunmai.Bxb.Api.App.Controllers
{
    [Consumes("application/json", "multipart/form-data")]
    public class CommonController : AppBaseController
    {
        private const string ENUM_ASSEMBLY_NAME = "Shunmai.Bxb.Entities";

        public CommonController()
        {

        }

        [SkipLoginVerification]
        [HttpPost("wechat/qrcode")]
        public IActionResult UploadWeixinPersonalQRCode(IFormCollection form, [FromServices]IOptions<AliOssServiceConfig> ossServiceConfig)
        {
            if (!form.Files.Any())
            {
                return Failed("图片文件不能为空");
            }

            var config = ossServiceConfig.Value;
            var file = form.Files[0];
            var dir = (OssFileDirType.PersonalWechatQrCode).GetDescription();
            var url = OssHelper.UploadFile(
                file.FileName
                , file.OpenReadStream()
                , dir
                , config.Website
                , config.AccessKeyId
                , config.AccessKeySecret
                , config.Endpoint
                , config.BucketName
            );
            return Success(url);
        }

        [SkipLoginVerification]
        [HttpGet("trade/rules")]
        public IActionResult GetTradeRules([FromServices]SystemConfigService configService)
        {
            var rules = configService.GetConfig<string>(SystemConfigNames.TRADE_RULES);
            return Success(rules);
        }

        [SkipLoginVerification]
        [HttpPost("sms/code")]
        public IActionResult SendVerificationCode([FromBody]SmsCodeRequest request, [FromServices]IOptions<SmsConfig> smsConfig, [FromServices]SmsService smsService)
        {
            var config = smsConfig.Value;
            var success = smsService.SendSmsCode(request.Phone, config.VerificationCodeLength, ApplicationType.WeixinMiniProgram, config.ExpiresMinutes * 60);
            return success ? Success() : Failed();
        }

        [SkipLoginVerification]
        [HttpGet("iet/test")]
        public async Task<IActionResult> TestIETApi([FromServices]IETService service)
        {
            var wallet = new IETWallet
            {
                Phone = "15041113056",
                LoginPassword = "6lPuDI+ByQNmDdaa1REdBg==",
                TradePassword = "Jp2d\\/9Wb6YNGCxnJAWInpA==",
                WalletId = "6da2540dadbe46d5b8eb6ad7d6c6944b",
            };
            var payRes = await service.PayAsync(wallet, "jn2P895ePPzQXSwaXn7y7hUuT9YSJ5fb1w", 0.000001M, "测试 linux 环境转账");
            var queryRes = await service.QueryTradeRecordsAsync(wallet);
            return Success(new
            {
                payRes,
                queryRes,
            });
        }

        [SkipLoginVerification]
        [HttpGet("types/{name}")]
        public IActionResult GetEnumDict(string name)
        {
            Assembly assembly = Assembly.Load(ENUM_ASSEMBLY_NAME);
            Type enumType = assembly.GetTypes().Where(t => t.IsEnum).FirstOrDefault(t => t.Name == name);
            Dictionary<int, string> dict = Enums.ToDictionary(enumType, t => t.ToString());
            var result = dict.Select(t => new { label = t.Value, value = t.Key });
            return Success(result);
        }
    }
}
