using Shunmai.Bxb.Entities.Enums;

namespace Shunmai.Bxb.Services.Models
{
    public class TradeHallQuery : Pager
    {
        /// <summary>
        /// 发布人ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        public CurrencyType? BType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public TradeHallShelfStatus? Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}