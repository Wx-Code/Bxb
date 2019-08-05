using Newtonsoft.Json;
using Shunmai.Bxb.Services.Models.IET;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Services.UnitTests
{
    public class IETServiceTests
    {
        private readonly IETService _service;

        public IETServiceTests()
        {
            _service = TestSuite.GetService<IETService>();
        }

        private IETWallet GetTestWallet()
        {
            return new IETWallet
            {
                Phone = "15041113056",
                LoginPassword = "6lPuDI+ByQNmDdaa1REdBg==",
                TradePassword = "Jp2d\\/9Wb6YNGCxnJAWInpA==",
                WalletId = "6da2540dadbe46d5b8eb6ad7d6c6944b",
            };
        }

        [Fact]
        public async Task Pay_Should_Succeed()
        {
            var wallet = GetTestWallet();
            var success = await _service.PayAsync(wallet, TestSuite.TEST_WALLET_ADDRESS, 0.000001M, "测试转出到目标钱包");
            Assert.True(success);
        }

        [Fact]
        public async Task QueryRecords_Should_Succeed()
        {
            var wallet = GetTestWallet();
            var result = await _service.QueryTradeRecordsAsync(wallet);
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
