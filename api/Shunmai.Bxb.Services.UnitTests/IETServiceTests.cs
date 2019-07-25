using Newtonsoft.Json;
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

        [Fact]
        public async Task Pay_Should_Succeed()
        {
            var result = await _service.PayAsync(TestSuite.TEST_WALLET_ADDRESS, 0.001m, "测试转出到目标钱包");
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
        }

        [Fact]
        public async Task QueryRecords_Should_Succeed()
        {
            var result = await _service.QueryTradeRecordsAsync();
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
        }
    }
}
