namespace Shunmai.Bxb.Services.Models.Wechat
{
    /// <summary>
    /// 发送模板消息时的 POST 数据模型
    /// </summary>
    /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1433751277</doc>
    public sealed class TemplateMessage
    {
        /// <summary>
        /// 【必填】用户 OPEN_ID
        /// </summary>
        public string ToUserId { get; set; }
        /// <summary>
        /// 【必填】模板 ID
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 【必填】模板数据
        /// </summary>
        public TemplateMessageData Data { get; set; }
        /// <summary>
        /// 点击消息跳转的 Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系，暂不支持小游戏）
        /// </summary>
        public string MiniProgramAppId { get; set; }
        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），要求该小程序已发布，暂不支持小游戏
        /// </summary>
        public string MiniProgramPagePath { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string Color { get; set; }
    }
}
