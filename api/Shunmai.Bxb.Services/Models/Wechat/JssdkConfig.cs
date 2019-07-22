using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    /// <summary>
    /// JS-SDK 权限配置对象
    /// </summary>
    /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141115</doc>
    public class JssdkConfig
    {
        public string AppId { get; set; }
        public string Timestamp { get; set; }
        public string NonceStr { get; set; }
        public string Signature { get; set; }
    }
}
