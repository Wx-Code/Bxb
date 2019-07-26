using System;
using Shunmai.Bxb.Entities.Enums;

namespace Shunmai.Bxb.Entities.Views
{
    public class TradeHallAppResponse
    {
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TradeId { get; set; }

        /// <summary>
        /// 可售数量
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 交易码
        /// </summary>
        public string TradeCode { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户二维码
        /// </summary>
        public string WxCodePhoto { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        public CurrencyType BType { get; set; }

        /// <summary>
        /// 货币类型显示名称
        /// </summary>
        public string BTypeText => BType.ToString();

        /// <summary>
        /// 状态
        /// </summary>
        public TradeHallShelfStatus Status { get; set; }

        /// <summary>
        /// 状态显示文本
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 发布人ID
        /// </summary>
        public int ReleaseUserId { get; set; }
    }

}
