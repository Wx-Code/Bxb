using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.IET
{
    public class LoginRequest
    {
        [JsonProperty("account")]
        public string Acount { get; set; }
        [JsonProperty("passwd")]
        public string Password { get; set; }
        [JsonProperty("appId")]
        public string AppId { get; set; } = "58a6ab604f594e19a727ecdaa1043fb5";
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = "99001229688451";
        /// <summary>
        /// 设备操作系统
        /// </summary>
        [JsonProperty("deviceos")]
        public string DeviceOS { get; set; } = "Android 9";
        /// <summary>
        /// 设备制造商
        /// </summary>
        [JsonProperty("firm")]
        public string Firm { get; set; } = "xiaomi";
        /// <summary>
        /// 地理位置坐标
        /// </summary>
        [JsonProperty("gps")]
        public string Coordinate { get; set; } = "26.6186873242,106.6912794113";
        /// <summary>
        /// 移动虚拟运营商 (MVNO: Mobile Virtual Network Operator)
        /// </summary>
        [JsonProperty("mno")]
        public string Mvno { get; set; } = "中国电信";
    }
}
