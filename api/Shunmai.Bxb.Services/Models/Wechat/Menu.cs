using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    /// <summary>
    /// 微信公众号菜单模型
    /// </summary>
    /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013</doc>
    public class Menu
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonIgnore]
        public MenuType? Type { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeString => Type == null ? null : Enum.GetName(typeof(MenuType), Type);
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("media_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaId { get; set; }
        [JsonProperty("appid", NullValueHandling = NullValueHandling.Ignore)]
        public string AppId { get; set; }
        [JsonProperty("pagepath", NullValueHandling = NullValueHandling.Ignore)]
        public string PagePath { get; set; }
        [JsonProperty("sub_button", NullValueHandling = NullValueHandling.Ignore)]
        public List<Menu> SubMenus { get; set; }
    }

    /// <summary>
    /// 微信公众号菜单类型
    /// </summary>
    /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013</doc>
    public enum MenuType
    {
        click,
        view,
        scancode_push,
        scancode_waitmsg,
        pic_sysphoto,
        pic_photo_or_album,
        pic_weixin,
        location_select,
        media_id,
        view_limited,
        miniprogram,
    }
}
