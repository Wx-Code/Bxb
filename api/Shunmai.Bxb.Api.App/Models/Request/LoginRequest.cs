using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Api.App.Models.Request
{
    public class LoginRequest
    {
        /// <summary>
        /// 微信用户授权所得的 CODE
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 活动 ID
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 拼团 ID
        /// </summary>
        public int GroupId { get; set; }
    }
}
