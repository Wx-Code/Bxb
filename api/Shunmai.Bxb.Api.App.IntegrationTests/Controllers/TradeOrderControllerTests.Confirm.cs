using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shunmai.Bxb.Abstractions;
using Shunmai.Bxb.Api.App.Constants;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Test.Common;
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
        [Fact]
        public async Task ConfirmShouldSuccess_While_EveryThingIsOkay()
        {
            var db = BxbContext.FromSmartSqlConfig();
            var cache = _fixture.GetService<ICache>();
            var user = TestSuite.GetTestUser();
            var token = Randoms.String(32);
            db.User.Add(user);
            db.SaveChanges();
            cache.Set(token, user.UserId, null);

            var order = TestSuite.GetTestOrder();
            order.SellerUserId = user.UserId;
            order.State = TradeOrderState.BuyerPaying;
            db.TradeOrder.Add(order);
            db.SaveChanges();

            var result = await _client.PutAsync<JsonResponse<string>>($"/orders/{order.OrderId}/confirm", null, new Dictionary<string, string> { { Headers.TOKEN, token } });
            result.success.Should().BeTrue("confirm should success while every thing is okay");
            db.Entry(order).State = EntityState.Detached;
            order = db.TradeOrder.Find(order.OrderId);
            order.State.Should().Be(TradeOrderState.PlatformOperating, "the order state should be TradeOrderState.PlatformOperating after confirmed");

            var log = db.TradeOrderLog.SingleOrDefault(l => l.OrderId == order.OrderId);
            log.Should().NotBeNull("the order log should be right inserted after confirmed");

            cache.Remove(token);
            db.User.Remove(user);
            db.TradeOrder.Remove(order);
            db.TradeOrderLog.Remove(log);
            db.SaveChanges();
        }
    }
}
