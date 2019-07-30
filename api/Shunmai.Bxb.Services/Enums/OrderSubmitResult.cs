using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Enums
{
    public enum OrderSubmitResult
    {
        Success = 1,

        /// <summary>
        /// 交易信息不存在
        /// </summary>
        TradeHallNotExists = -1,

        /// <summary>
        /// 交易状态异常
        /// </summary>
        TradeHallStateException = -2,

        /// <summary>
        /// 上下架状态异常
        /// </summary>
        TradeHallStatusException = -3,

        /// <summary>
        /// 交易码输入有误
        /// </summary>
        TradeCodeInputError = -4,

        /// <summary>
        /// 可交易数量不足
        /// </summary>
        CountNotEnough = -5,

        /// <summary>
        /// 数据保存失败
        /// </summary>
        PersistenceFailed = -6,
    }
}
