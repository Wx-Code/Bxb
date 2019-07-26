using Shunmai.Bxb.Entities.Enums;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    public class WechatUserInfo
    {
        public string NickName { get; set; }
        public string OpenId { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public string UnionId { get; set; }
        public int Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }
}
