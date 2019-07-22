using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.Open.QRConnect;
using Senparc.Weixin.TenPay.V3;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;
using Shunmai.Bxb.Services.Models.Wechat;
using Shunmai.Bxb.Utilities;
using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Services
{
    public class WechatService
    {
        private readonly ILogger _logger;
        private readonly WechatConfig _wechatConfig;

        private T AccessTokenWrapper<T>(string appId, string appSecret, Func<T> action)
        {
            if (!AccessTokenContainer.CheckRegistered(appId))
            {
                RegisterAccessToken(appId, appSecret).GetAwaiter().GetResult();
            }
            return action();
        }

        private async Task RegisterAccessToken(string appId, string appSecret)
        {
            await AccessTokenContainer.RegisterAsync(appId, appSecret);
        }

        private void RegisterTenPayV3(WechatConfig wechatConfig)
        {
            var info = new TenPayV3Info(
                wechatConfig.AppId
                , wechatConfig.AppSecrect
                , wechatConfig.MerchantId
                , wechatConfig.ApiKey
                , wechatConfig.CertAbsolutePath
                , wechatConfig.CertPassword
                , null
                , null
                , null
                , string.Empty
                , string.Empty
            );
            TenPayV3InfoCollection.Register(info, "ShunmaiBxb");
        }

        public WechatService(ILogger<WechatService> logger, WechatConfig wechatConfig)
        {
            _logger = logger;
            _wechatConfig = wechatConfig;

            RegisterAccessToken(wechatConfig.AppId, wechatConfig.AppSecrect).GetAwaiter().GetResult();
            RegisterTenPayV3(wechatConfig);
        }

        /// <summary>
        /// 请求微信接口默认超时时间
        /// </summary>
        public const int DEFAULT_TIMEOUT = 10 * 1000;

        /// <summary>
        /// 通过 code 获取用户标识
        /// </summary>
        /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140842</doc>
        /// <param name="code"></param>
        public string GetOpenId(string code)
        {
            Check.Empty(code, nameof(code));
            var result = QRConnectAPI.GetAccessToken(_wechatConfig.AppId, _wechatConfig.AppSecrect, code);
            if (result.openid.IsEmpty())
            {
                _logger.LogError($"Get openid failed, result: {result.ToLogFormatString()}");
            }

            return result.openid;
        }

        /// <summary>
        /// 获取微信用户信息，当用户未关注公众号时，返回 null
        /// </summary>
        /// <param name="openId">普通用户的标识，对当前公众号唯一</param>
        /// <returns>微信用户信息，当用户未关注公众号时，返回 null</returns>
        public WechatUserInfo GetUserInfo(string openId)
        {
            Check.Empty(openId, nameof(openId));
            var result = UserApi.Info(_wechatConfig.AppId, openId);
            if (result.subscribe == 0)
            {
                _logger.LogError($"Get wechat user info failed, result: {result}");
                return null;
            }
            return new WechatUserInfo
            {
                Avatar = result.headimgurl,
                City = result.city,
                Country = result.country,
                NickName = result.nickname,
                OpenId = result.openid,
                Province = result.province,
                Sex = result.sex,
                UnionId = result.unionid,
            };
        }

        /// <summary>
        /// 生成带参数的二维码，成功之后返回微信生成的 ticket，前端可通过此 ticket 获取二维码图片
        /// </summary>
        /// <param name="sceneId">场景 ID</param>
        /// <param name="expireSeconds">过期时间（单位：秒）</param>
        /// <returns></returns>
        public string CreateQRCode(int sceneId, int expireSeconds)
        {
            Check.EnsureGreaterThanZero(sceneId, nameof(sceneId));
            Check.EnsureGreaterThanZero(expireSeconds, nameof(expireSeconds));
            var result = QrCodeApi.Create(_wechatConfig.AppId, expireSeconds, sceneId, QrCode_ActionName.QR_SCENE);
            if (result.ticket.IsEmpty())
            {
                _logger.LogError($"Create QR code failed, result: {result.ToLogFormatString()}");
                return default(string);
            }
            return result.ticket;
        }

        /// <summary>
        /// 生成带参数的二维码，成功之后返回微信生成的 ticket，前端可通过此 ticket 获取二维码图片
        /// </summary>
        /// <param name="sceneString"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        public string CreateQRCode(string sceneString, int expireSeconds)
        {
            Check.Empty(sceneString, nameof(sceneString));
            Check.EnsureGreaterThanZero(expireSeconds, nameof(expireSeconds));
            var result = QrCodeApi.Create(_wechatConfig.AppId, expireSeconds, 0, QrCode_ActionName.QR_STR_SCENE, sceneString);
            if (result.ticket.IsEmpty())
            {
                _logger.LogError($"Create QR code failed, result: {result.ToLogFormatString()}");
                return default(string);
            }
            return result.ticket;
        }

        /// <summary>
        /// 通过分析给定请求对象中的信息，判断请求是否来自微信且用于验证公众平台服务器配置 token 验证
        /// </summary>
        /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421135319</doc>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool IsTokenVerificationRequest(HttpRequest request, out WechatTokenVerificationModel data)
        {
            data = null;
            Check.Null(request, nameof(request));
            string signature = request.Query["signature"];
            string timestamp = request.Query["timestamp"];
            string nonce = request.Query["nonce"];
            string echostr = request.Query["echostr"];
            bool isNotEmpty(string s) => s.IsEmpty() == false;
            if (isNotEmpty(signature) && isNotEmpty(timestamp) && isNotEmpty(nonce) && isNotEmpty(echostr))
            {
                data = new WechatTokenVerificationModel
                {
                    Signature = signature,
                    Timestamp = timestamp,
                    Nonce = nonce,
                    Echostr = echostr,
                };
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证签名是否有效
        /// </summary>
        /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421135319</doc>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool IsSignValid(WechatTokenVerificationModel request)
        {
            Check.Null(request, nameof(request));
            var model = new PostModel
            {
                Nonce = request.Nonce,
                Timestamp = request.Timestamp,
                Token = _wechatConfig.Token,
            };
            return CheckSignature.Check(request.Signature, model);
        }

        /// <summary>
        /// 获取 JS-SDK 接口权限配置
        /// </summary>
        /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141115</doc>
        /// <param name="url"></param>
        /// <returns></returns>
        public JssdkConfig GetJssdkConfig(string url)
        {
            var config = JSSDKHelper.GetJsSdkUiPackage(_wechatConfig.AppId, _wechatConfig.AppSecrect, url);
            return config.MapTo<JssdkConfig>();
        }

        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="message">模板消息数据</param>
        /// <param name="timeOut">请求超时时间</param>
        /// <returns>表示请求成功与否的布尔值</returns>
        public bool SendTemplateMessage(TemplateMessage message, int timeOut = DEFAULT_TIMEOUT)
        {
            TempleteModel_MiniProgram miniProgram = null;
            if (message.MiniProgramAppId.IsNotEmpty() && message.MiniProgramPagePath.IsNotEmpty())
            {
                miniProgram = new TempleteModel_MiniProgram
                {
                    appid = message.MiniProgramAppId,
                    pagepath = message.MiniProgramPagePath,
                };
            }

            var result = TemplateApi.SendTemplateMessage(
                _wechatConfig.AppId
                , message.ToUserId
                , message.TemplateId
                , message.Url
                , message.Data
                , miniProgram
                , timeOut
            );
            _logger.LogInformation($"Send template result: {result.ToLogFormatString()}");
            return result.msgid > 0;
        }

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="userOpenId">用户 openId</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        public bool SendCustomMessage(string userOpenId, string content)
        {
            var result = CustomApi.SendText(_wechatConfig.AppId, userOpenId, content);
            _logger.LogInformation($"Send custom message result: {result.ToLogFormatString()}");
            return result.errcode == ReturnCode.请求成功;
        }

        /// <summary>
        /// 更新微信公众号菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        public bool UpdateWechatMenu(List<Menu> menus)
        {
            var data = new
            {
                button = menus
            };
            var result = CommonApi.CreateMenu(_wechatConfig.AppId, data);
            _logger.LogInformation(result.ToLogFormatString());
            return result.errcode == ReturnCode.请求成功;
        }
    }
}
