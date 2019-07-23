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
    }
}
