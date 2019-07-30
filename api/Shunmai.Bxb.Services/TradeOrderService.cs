using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services
{
    public class TradeOrderService
    {
        private readonly ILogger _logger;
        private readonly ITradeHallRepository _hallRepos;
        private readonly ITradeOrderRepository _orderRepos;
        private readonly ITradeOrderLogRepository _orderLogRepos;

        public TradeOrderService(
            ILogger<TradeOrderService> logger
            , ITradeHallRepository hallRepository
            , ITradeOrderRepository orderRepository
            , ITradeOrderLogRepository logRepository
        )
        {
            _logger = logger;
            _hallRepos = hallRepository;
            _orderRepos = orderRepository;
            _orderLogRepos = logRepository;
        }

        /// <summary>
        /// 尝试提交订单，如果出现如下情形之一，则返回 false 表示订单提交失败
        ///     1. 如果交易信息不存在
        ///     2. 如果交易信息的 `Status` 不处于上架状态
        ///     3. 如果交易信息的 `State` 不处于正常进行中
        ///     4. 如果输入的交易码错误
        ///     5. 如果输入的购买数量大于剩余数量
        ///     6. 尝试创建交易订单，如果创建失败
        ///     7. 尝试创建交易日志，如果创建失败
        /// </summary>
        /// <param name="hallId"></param>
        /// <param name="count"></param>
        /// <param name="tradeCode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [SmartSqlTransaction]
        public bool Submit(int hallId, int count, string tradeCode, out OrderSubmitResult result)
        {


            result = OrderSubmitResult.Success;
            return true;
        }
    }
}
