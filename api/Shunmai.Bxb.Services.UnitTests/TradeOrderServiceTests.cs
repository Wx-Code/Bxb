using Microsoft.Extensions.Logging;
using Moq;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Shunmai.Bxb.Services.UnitTests
{
    public class TradeOrderServiceTests
    {
        private readonly ILogger<TradeOrderService> _logger;
        private readonly ITradeHallRepository _hallRepos;
        private readonly ITradeOrderRepository _orderRepos;
        private readonly ITradeOrderLogRepository _orderLogRepos;
        private readonly TradeOrderService _service;

        public TradeOrderServiceTests()
        {
            _logger = Mock.Of<ILogger<TradeOrderService>>();
            _hallRepos = Mock.Of<ITradeHallRepository>();
            _orderRepos = Mock.Of<ITradeOrderRepository>();
            _orderLogRepos = Mock.Of<ITradeOrderLogRepository>();
            _service = new TradeOrderService(_logger, _hallRepos, _orderRepos, _orderLogRepos);
        }
    }
}
