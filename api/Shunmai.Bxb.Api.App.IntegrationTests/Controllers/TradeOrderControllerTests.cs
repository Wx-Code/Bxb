
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.ObjectPool;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Api.App.Constants;
using Shunmai.Bxb.Api.App.Models.Request;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Common.Constans;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Test.Common.Models;
using Shunmai.Bxb.Test.Common.TestPriority;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using Shunmai.Bxb.Common.Models.Config;
using Shunmai.Bxb.Common.Enums;

namespace Shunmai.Bxb.Api.App.IntegrationTests.Controllers
{
    [TestCaseOrderer("Shunmai.Bxb.Test.Common.TestPriority.PriorityOrderer", "Shunmai.Bxb.Test.Common")]
    public class TradeOrderControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;
        private readonly HttpClient _client;
        private readonly ICache _cache;
        private readonly List<string> _tokens = new List<string>();
        private readonly object _lockKey = new object();
        private readonly ObjectPool<BxbContext> _contextPool = new DefaultObjectPool<BxbContext>(new DbContextPooledObjectPolicy());

        private const string SUBMT_URL = "/orders";

        public TradeOrderControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
            _cache = _fixture.GetService<ICache>();
        }

        /**
         * Submit 测试用例
         *  1. 当平台钱包地址等配置信息未填写时，则提交订单应该失败
         *  2. 如果交易信息的 Status 不处于上架状态，则提交订单应该失败
         *  3. 如果交易信息的 State 不处于正常进行中，则提交订单应该失败
         *  4. 如果输入的交易码错误，则提交订单应该失败
         *  5. 如果输入的购买数量大于剩余数量，则提交订单应该失败
         *  6. 如果一切正常，则提交订单应该成功，同时应该生成正确的订单信息、
         *     生成正确的订单日志，且交易信息应该被更新
         */

        private SystemConfigExt CreateConfig(string name, object value)
        {
            var json = value.GetType() == typeof(string) ? value.ToString() : JsonConvert.SerializeObject(value);
            return new SystemConfigExt
            {
                ConfigName = name,
                ConfigValue = json,
                CreateTime = DateTime.Now,
                CreateUser = "Test",
                Remark = "Test",
            };
        }

        // 由于 SystemConfig 具有 ConfigName 唯一特性，因此只需要初始化一次即可
        private bool _platConfigInited = false;
        private (SystemConfigExt PlatformAddrConfig, SystemConfigExt RateConfig) _platConfig;
        private (SystemConfigExt PlatformAddrConfig, SystemConfigExt RateConfig)
            PreparePlatformConfig()
        {
            if (_platConfigInited == false)
            {
                lock (_lockKey)
                {
                    if (_platConfigInited == false)
                    {
                        var dbContext = _contextPool.Get();
                        dbContext.Truncate(nameof(SystemConfig));

                        var platformAddrConfig = CreateConfig(SystemConfigNames.PLATFORM_WALLET_ADDRESS, new List<PlatWalletAddrInfo> {
                            TestSuite.GetTestWalletConfig(PurposeType.TurnCoin),
                            TestSuite.GetTestWalletConfig(PurposeType.CommissionCharge),
                        });
                        var rateConfig = CreateConfig(SystemConfigNames.TRADE_FEE, new TradeFeeInfo { SigleServiceFee = 0.1M, SigleTradeFee = 1M });
                        dbContext.SystemConfig.AddRange(platformAddrConfig, rateConfig);
                        dbContext.SaveChanges();

                        _contextPool.Return(dbContext);
                        _platConfig = (platformAddrConfig, rateConfig);
                    }
                }
            }

            return _platConfig;
        }

        private SubmitPreparedData PrepareDataBeforeTestSubmit()
        {
            var dbContext = _contextPool.Get();
            dbContext.Database.BeginTransaction();
            var seller = TestSuite.CreateTestUser();
            var buyer = TestSuite.CreateTestUser();
            if (dbContext.User.Any(u => u.Phone == seller.Phone || u.Phone == buyer.Phone))
            {
                // 因为 Phone 字段具有唯一索引，为了避免添加用户出错，在添加之前先将已存在的用户删除
                dbContext.ExecuteSql($"DELETE FROM `User` WHERE `Phone` IN('${seller.Phone}', '{buyer.Phone}')");
            }
            dbContext.User.AddRange(seller, buyer);
            dbContext.SaveChanges();
            var token = Randoms.String(32);
            _tokens.Add(token);
            _cache.Set(token, buyer.UserId, null);

            var hall = new TradeHallExt
            {
                Amount = 100,
                BType = CurrencyType.GDT,
                Price = 10,
                ReleaseName = seller.Nickname,
                ReleaseTime = DateTime.Now,
                ReleaseUserId = seller.UserId,
                State = TradeHallState.Working,
                Status = TradeHallShelfStatus.On,
                TotalAmount = 100,
                TradeCode = Randoms.String(20),
                TradeType = TradeType.Selling,
            };
            dbContext.TradeHall.Add(hall);
            dbContext.SaveChanges();

            var submitRequest = new SubmitRequest
            {
                TradeId = hall.TradeId,
                TradeCode = hall.TradeCode,
                RequiredCount = 10,
            };
            dbContext.Database.CommitTransaction();
            _contextPool.Return(dbContext);

            var (platformAddrConfig,  rateConfig) = PreparePlatformConfig();
            return new SubmitPreparedData
            {
                Seller = seller,
                Buyer = buyer,
                Hall = hall,
                PlatformAddrConfig = platformAddrConfig,
                RateConfig = rateConfig,
                SubmitRequest = submitRequest,
                LoginToken = token,
            };
        }

        private async Task<JsonResponse<string>> Submit(SubmitPreparedData data)
        {
            return await _client.PostAsync<JsonResponse<string>>(SUBMT_URL, data.SubmitRequest, new Dictionary<string, string> { { Headers.TOKEN, data.LoginToken } });
        }

        [Fact, TestPriority(2)]
        public async Task SubmitShouldFail_While_PlatformWalletAddressNotPresented()
        {
            var dbContext = _contextPool.Get();
            var data = PrepareDataBeforeTestSubmit();
            dbContext.SystemConfig.RemoveRange(data.PlatformAddrConfig);
            dbContext.SaveChanges();
            _contextPool.Return(dbContext);

            var result = await Submit(data);
            Assert.False(result.success);
        }

        [Theory, TestPriority(1)]
        [InlineData(TradeHallShelfStatus.Off)]
        [InlineData(TradeHallShelfStatus.Deleted)]
        public async Task SubmitShouldFail_While_TradeStatusException(TradeHallShelfStatus status)
        {
            var dbContext = _contextPool.Get();
            var data = PrepareDataBeforeTestSubmit();
            data.Hall.Status = status;
            dbContext.TradeHall.Update(data.Hall);
            dbContext.SaveChanges();
            _contextPool.Return(dbContext);

            var result = await Submit(data);
            Assert.False(result.success);
            Assert.Equal(Errors.CannotTrade.Code, result.errorCode);
        }

        [Theory, TestPriority(1)]
        [InlineData(TradeHallState.Completed)]
        public async Task SubmitShouldFail_While_TradeStateException(TradeHallState state)
        {
            var dbContext = _contextPool.Get();
            var data = PrepareDataBeforeTestSubmit();
            data.Hall.State = state;
            dbContext.TradeHall.Update(data.Hall);
            dbContext.SaveChanges();
            _contextPool.Return(dbContext);

            var result = await Submit(data);
            Assert.False(result.success);
            Assert.Equal(Errors.CannotTrade.Code, result.errorCode);
        }

        [Fact, TestPriority(1)]
        public async Task SubmitShouldFail_While_FeedErrorTradeCode()
        {
            var data = PrepareDataBeforeTestSubmit();
            data.SubmitRequest.TradeCode = "wrong code" + Randoms.String(32);

            var result = await Submit(data);
            Assert.False(result.success);
            Assert.Equal(Errors.TradeCodeError.Code, result.errorCode);
        }

        [Fact, TestPriority(1)]
        public async Task SubmitShouldFail_While_RequiredCountGreaterThanActualCount()
        {
            var data = PrepareDataBeforeTestSubmit();
            data.SubmitRequest.RequiredCount = (int)data.Hall.Amount + 1;

            var result = await Submit(data);
            Assert.False(result.success);
            Assert.Equal(Errors.NotEnoughCount.Code, result.errorCode);
        }

        [Fact, TestPriority(1)]
        public async Task SubmitShouldSuccess_While_EveryThingOkay()
        {
            var dbContext = _contextPool.Get();
            var data = PrepareDataBeforeTestSubmit();
            var result = await Submit(data);
            Assert.True(result.success);

            var hall = dbContext.TradeHall.Find(data.Hall.TradeId);
            Assert.Equal(data.Hall.Amount - data.SubmitRequest.RequiredCount, hall.Amount); // 可交易数量应该减少
            Assert.Equal(data.Hall.TotalAmount, hall.TotalAmount); // 总数应该保持不变
            data.Hall.State.Should().Be(TradeHallState.Working, "the state should be working");

            // 订单信息应该保存正确
            var order = dbContext.TradeOrder.FirstOrDefault(o => o.TradeId == hall.TradeId);
            Assert.NotNull(order);
            order.Should().NotBeNull("the order should not be null");
            order.BuyerUserId.Should().Be(data.Buyer.UserId, "the buyer's userId should be right setted");
            order.BuyerPhone.Should().Be(data.Buyer.Phone, "the buyer's phone should be right setted");
            order.BuyerWalletAddress.Should().Be(data.Buyer.WalletAddress, "the buyer's wallet address should be right setted");
            order.SellerUserId.Should().Be(data.Seller.UserId, "the seller's userId should be right setted");
            order.SellerPhone.Should().Be(data.Seller.Phone, "the seller's phone should be right setted");
            order.SellerWalletAddress.Should().Be(data.Seller.WalletAddress, "the seller's wallet address should be right setted");
            order.State.Should().Be(TradeOrderState.SellerOperating, "the order state should be right setted");
            order.TotalAmount.Should().Be(data.SubmitRequest.RequiredCount * data.Hall.Price, "the total amount should be right calculated");
            order.Price.Should().Be(data.Hall.Price, "the price should be right setted");
            order.Btype.Should().Be(data.Hall.BType, "the currency type should be right setted");
            order.Amount.Should().Be(data.SubmitRequest.RequiredCount, "the required count should be right setted");
            order.TradeCode.Should().Be(data.Hall.TradeCode, "the trade code should be right setted");

            var list = JsonConvert.DeserializeObject<List<PlatWalletAddrInfo>>(data.PlatformAddrConfig.ConfigValue);
            var platWalletAddr = list.Single(c => c.Purpost == PurposeType.TurnCoin).PlatWalletAddr;
            var serviceFeeWalletAddr = list.Single(c => c.Purpost == PurposeType.CommissionCharge).PlatWalletAddr;
            order.PlatWalletAddress.Should().Be(platWalletAddr, "the platform wallet address should be right setted");
            order.PlatServiceWalletAddress.Should().Be(serviceFeeWalletAddr, "the wallet address for receiving service fee should be right setted");

            var tradeConf = JsonConvert.DeserializeObject<TradeFeeInfo>(data.RateConfig.ConfigValue);
            var serviceFee = (data.SubmitRequest.RequiredCount * tradeConf.SigleServiceFee) + tradeConf.SigleTradeFee;
            order.ServiceAmount.Should().Be(serviceFee, "the service fee should be right calculated");
            // 应该生成订单日志
            var orderLog = dbContext.TradeOrderLog.FirstOrDefault(log => log.OrderId == order.OrderId);
            orderLog.Should().NotBeNull("the order log should be inserted");

            _contextPool.Return(dbContext);
        }

        [Fact, TestPriority(1)]
        public async Task TradeState_Should_BeCompleted_AfterSubmit()
        {
            var dbContext = _contextPool.Get();
            var data = PrepareDataBeforeTestSubmit();
            data.SubmitRequest.RequiredCount = (int)data.Hall.Amount;
            var result = await Submit(data);
            result.success.Should().Be(true, "submit should success");

            var trade = dbContext.TradeHall.Find(data.Hall.TradeId);
            trade.State.Should().Be(TradeHallState.Completed, "the trade state should be completed after all coins were sold out");

            _contextPool.Return(dbContext);
        }

        ~TradeOrderControllerTests()
        {
            if (_tokens.Any())
            {
                _tokens.ForEach(t => _cache?.Remove(t));
            }
            var dbContext = _contextPool.Get();
            dbContext.Truncate(nameof(User), nameof(TradeHall), nameof(TradeHallLog), nameof(TradeOrder), nameof(TradeOrderLog));
            _contextPool.Return(dbContext);
        }

    }

    class SubmitPreparedData
    {
        public UserExt Seller { get; set; }
        public UserExt Buyer { get; set; }
        public TradeHallExt Hall { get; set; }
        public SystemConfigExt PlatformAddrConfig { get; set; }
        public SystemConfigExt RateConfig { get; set; }
        public SubmitRequest SubmitRequest { get; set; }
        public string LoginToken { get; set; }
    }
}
