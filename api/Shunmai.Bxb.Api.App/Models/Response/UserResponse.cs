namespace Shunmai.Bxb.Api.App.Models.Response
{
    public class UserResponse
    {
        public int UserId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Realname { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public int ShareId { get; set; }
        /// <summary>
        /// 推荐人昵称
        /// </summary>
        public string ShareNickname { get; set; }
        /// <summary>
        /// 是否关注了公众号
        /// </summary>
        public bool Subscribed { get; set; }
        /// <summary>
        /// 是否是新用户
        /// </summary>
        public bool IsNewUser { get; set; }
    }
}
