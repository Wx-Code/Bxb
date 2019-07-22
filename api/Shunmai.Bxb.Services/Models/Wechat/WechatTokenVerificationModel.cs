namespace Shunmai.Bxb.Services.Models.Wechat
{
    /// <summary>
    /// 微信公众平台服务器 token 验证 model
    /// </summary>
    /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421135319</doc>
    public class WechatTokenVerificationModel
    {
        public string Signature { get; set; }
        public string Timestamp { get; set; }
        public string Nonce { get; set; }
        public string Echostr { get; set; }
    }
}
