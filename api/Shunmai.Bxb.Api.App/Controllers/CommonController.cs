using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shunmai.Bxb.Common.Attributes;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Common.Enums;
using Shunmai.Bxb.Services;
using Shunmai.Bxb.Services.Constans;
using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utils.Helpers;
using System.Linq;
using Shunmai.Bxb.Api.App.Models.Request;
using System;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Services.Models.IET;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.Controllers
{
    [Consumes("application/json", "multipart/form-data")]
    public class CommonController : AppBaseController
    {
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
        public async Task<IActionResult> TestIETApi()
        {
            var config = new IETConfig
            {
                Cookie = "WEBID=7c476f81-7d54-4ab8-94d8-3978218e9c11",
                Password = "Jp2d\\/9Wb6YNGCxnJAWInpA==",
                Phone = "15041113056",
                ServiceFeeRate = 0.05m,
                ServiceFeeReceiveAddr = "jn2P895ePPzQXSwaXn7y7hUuT9YSJ5fb1w",
                WalletId = "6da2540dadbe46d5b8eb6ad7d6c6944b"
            };
            var service = new IETService(config);
            var payRes = await service.PayAsync("jn2P895ePPzQXSwaXn7y7hUuT9YSJ5fb1w", 0.001m, "测试 linux 环境转账");
            var queryRes = await service.QueryTradeRecordsAsync();
            return Success(new
            {
                payRes,
                queryRes,
            });
        }
    }
}
