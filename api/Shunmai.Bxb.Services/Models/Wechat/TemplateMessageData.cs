using Newtonsoft.Json;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    /// <summary>
    /// 发送模板消息时模板数据模型
    /// 需要注意的是：微信对模板消息的格式是有要求的，一般是三段式，即：first、keywords、remark
    ///     这里定义的模型预留了几个 keyword 字段，是因为不同的模板关键字的个数不一样，若使用的模板
    ///     只需要两个关键字，则只使用 keyword1、keyword2 就行，忽略其他关键字
    /// 但是：必须按顺序使用关键字
    /// </summary>
    /// <doc>https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1433751277</doc>
    public sealed class TemplateMessageData
    {
        [JsonProperty("first")]
        public TemplateMessageDataObject First { get; set; }
        [JsonProperty("keyword1", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Keyword1 { get; set; }
        [JsonProperty("keyword2", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Keyword2 { get; set; }
        [JsonProperty("keyword3", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Keyword3 { get; set; }
        [JsonProperty("keyword4", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Keyword4 { get; set; }
        [JsonProperty("keyword5", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Keyword5 { get; set; }
        [JsonProperty("keyword6", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Keyword6 { get; set; }
        [JsonProperty("remark", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateMessageDataObject Remark { get; set; }

        public TemplateMessageData()
        {

        }

        public TemplateMessageData(string title, string keyword1, string keyword2, string remark)
        {
            First = new TemplateMessageDataObject(title);
            Keyword1 = new TemplateMessageDataObject(keyword1);
            Keyword2 = new TemplateMessageDataObject(keyword2);
            Remark = new TemplateMessageDataObject(remark);
        }

        public TemplateMessageData(string title, string keyword1, string keyword2, string keyword3, string remark)
        {
            First = new TemplateMessageDataObject(title);
            Keyword1 = new TemplateMessageDataObject(keyword1);
            Keyword2 = new TemplateMessageDataObject(keyword2);
            Keyword3 = new TemplateMessageDataObject(keyword3);
            Remark = new TemplateMessageDataObject(remark);
        }
    }

    public sealed class TemplateMessageDataObject
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        public TemplateMessageDataObject(string value)
        {
            Value = value;
        }
    }
}
