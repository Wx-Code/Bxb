using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Constants;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Test.Common;
using Shunmai.Bxb.Test.Common.Models;
using Shunmai.Bxb.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Shunmai.Bxb.Api.App.IntegrationTests.Controllers
{
    public partial class TradeOrderControllerTests
    {
        private ChangeStateData PrepareOrderDataForChangingState(TradeOrderState initState)
        {
            var db = BxbContext.FromSmartSqlConfig();
            var cache = _fixture.GetService<ICache>();
            var seller = TestSuite.GetTestUser();
            var sellerToken = Randoms.String(32);
            var buyer = TestSuite.GetTestUser();
            var buyerToken = Randoms.String(32);
            db.User.AddRange(seller, buyer);
            db.SaveChanges();
            cache.Set(sellerToken, seller.UserId, null);
            cache.Set(buyerToken, buyer.UserId, null);

            var order = TestSuite.GetTestOrder();
            order.SellerUserId = seller.UserId;
            order.BuyerUserId = buyer.UserId;
            order.State = initState;
            db.TradeOrder.Add(order);
            db.SaveChanges();

            return new ChangeStateData
            {
                Db = db,
                Buyer = buyer,
                BuyerToken = buyerToken,
                Seller = seller,
                SellerToken = sellerToken,
                Order = order,
            };
        }

        private void ClearData(ChangeStateData data)
        {
            var cache = _fixture.GetService<ICache>();
            cache.Remove(data.SellerToken);
            cache.Remove(data.BuyerToken);
            data.Db.User.RemoveRange(data.Seller, data.Buyer);
            data.Db.TradeOrder.Remove(data.Order);
            data.Db.SaveChanges();
            data.Db.Truncate(nameof(TradeOrderLog));
        }

        [Fact]
        public async Task ConfirmShouldSuccess_While_EveryThingIsOkay()
        {
            var data = PrepareOrderDataForChangingState(TradeOrderState.BuyerPaying);

            try
            {
                var db = data.Db;
                var order = data.Order;
                var result = await _client.PutAsync<JsonResponse<string>>($"/orders/{order.OrderId}/confirm", null, new Dictionary<string, string> { { Headers.TOKEN, data.SellerToken } });
                result.success.Should().BeTrue("confirm should success while every thing is okay");
                db.Entry(order).State = EntityState.Detached;
                data.Order = order = db.TradeOrder.Find(order.OrderId);
                order.State.Should().Be(TradeOrderState.PlatformOperating, "the order state should be TradeOrderState.PlatformOperating after confirmed");

                var log = db.TradeOrderLog.SingleOrDefault(l => l.OrderId == order.OrderId);
                log.Should().NotBeNull("the order log should be right inserted after confirmed");
            }
            finally
            {
                ClearData(data);
            }
        }

        [Fact]
        public async Task CancelShouldSuccess_While_EveryThingIsOkay()
        {
            var data = PrepareOrderDataForChangingState(TradeOrderState.SellerOperating);
            try
            {
                var order = data.Order;
                var result = await _client.PutAsync<JsonResponse<string>>($"/orders/{order.OrderId}/cancel", null, new Dictionary<string, string> { { Headers.TOKEN, data.BuyerToken } });
                result.success.Should().BeTrue("cancel should success while every thing is okay");
                data.Db.Entry(order).State = EntityState.Detached;
                data.Order = order = data.Db.TradeOrder.Find(order.OrderId);
                order.State.Should().Be(TradeOrderState.Canceled, "the order state should be TradeOrderState.Canceled after canceled");

                var log = data.Db.TradeOrderLog.SingleOrDefault(l => l.OrderId == order.OrderId);
                log.Should().NotBeNull("the order log should be right inserted after canceled");
            }
            finally
            {
                ClearData(data);
            }
        }
    }

    class ChangeStateData
    {
        public string SellerToken { get; set; }
        public UserExt Seller { get; set; }
        public string BuyerToken { get; set; }
        public UserExt Buyer { get; set; }
        public TradeOrderExt Order { get; set; }
        public BxbContext Db { get; set; }
    }
}
