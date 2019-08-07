using FluentAssertions;
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

        #region Tests for method Confirm
        [Theory]
        [InlineData(0, 1)]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(-1, 0)]
        [InlineData(1, -1)]
        [InlineData(1, 0)]
        public void ConfirmShouldThrowException_While_ParametersAreIllegal(long orderId, int userId)
        {
            Action confirm = () => _service.SetConfirmed(orderId, userId, out _);
            confirm.Should().Throw<ArgumentException>("confirm should throw exception to avoid nonsence sql query while the parameters are illegal");
        }

        [Fact]
        public void ConfirmShouldReturnFalse_While_OrderDoesNotExist()
        {
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns((TradeOrder)null);

            var success = _service.SetConfirmed(1, 1, out var result);
            success.Should().BeFalse("confirm should return false while the order does not exist");
            result.Should().Be(ChangeOrderStateResult.OrderNotExists, "ChangeOrderStateResult should be OrderNotExists");
        }

        [Fact]
        public void ConfirmShouldReturnFalse_While_OperatingUserIsNotSellerHimself()
        {
            var order = new TradeOrder { SellerUserId = 1, State = TradeOrderState.BuyerPaying, };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);

            var success = _service.SetConfirmed(1, 2, out var result);
            success.Should().BeFalse("confirm should return false while the operating user is not the seller himself");
            result.Should().Be(ChangeOrderStateResult.Unautherized, "ChangeOrderStateResult should be Unautherized while the operating user is not the seller himself");
        }

        [Theory]
        [InlineData(TradeOrderState.Canceled)]
        [InlineData(TradeOrderState.Completed)]
        [InlineData(TradeOrderState.SellerOperating)]
        [InlineData(TradeOrderState.PlatformOperating)]
        public void ConfirmShouldReturnFasle_While_OccuringOrderStateException(TradeOrderState state)
        {
            var userId = 1;
            var order = new TradeOrder { SellerUserId = userId, State = state };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);

            var success = _service.SetConfirmed(1, userId, out var result);
            success.Should().BeFalse("confirm should return false while occuring order state exception");
            result.Should().Be(ChangeOrderStateResult.OrderStateException, "ChangeOrderStateResult should be OrderStateException while occuring order state exception");
        }

        [Fact]
        public void ConfirmShouldReturnFalse_While_UpdatingOrderStateFailed()
        {
            var userId = 1;
            var order = new TradeOrder { SellerUserId = userId, State = TradeOrderState.BuyerPaying };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);
            // mock insert log succeed to make sure that it is not the reason causes confirm failed
            Mock.Get(_orderLogRepos).Setup(r => r.Insert(It.IsAny<TradeOrderLog>())).Returns(1);
            Mock.Get(_orderRepos).Setup(r => r.UpdateState(It.IsAny<long>(), It.IsAny<TradeOrderState>())).Returns(false);

            var success = _service.SetConfirmed(1, userId, out var result);
            success.Should().BeFalse("confirm should return false while updating order state failed");
            result.Should().Be(ChangeOrderStateResult.PersistenceFailed, "ChangeOrderStateResult should be PersistenceFailed while updating order state failed");
        }

        [Fact]
        public void ConfirmShouldReturnFalse_While_ItWasFailedToAddOrderLog()
        {
            var userId = 1;
            var order = new TradeOrder { SellerUserId = userId, State = TradeOrderState.BuyerPaying };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);
            // mock update order state succeed to make sure that it is not the reason causes confirm failed
            Mock.Get(_orderRepos).Setup(r => r.UpdateState(It.IsAny<long>(), It.IsAny<TradeOrderState>())).Returns(true);
            Mock.Get(_orderLogRepos).Setup(r => r.Insert(It.IsAny<TradeOrderLog>())).Returns(0);

            var success = _service.SetConfirmed(1, userId, out var result);
            success.Should().BeFalse("confirm should return false while it is failed to add order log");
            result.Should().Be(ChangeOrderStateResult.PersistenceFailed, "ChangeOrderStateResult should be PersistenceFailed while it is failed to add order log");
        }
        #endregion

        #region Tests for method Cancel
        [Theory]
        [InlineData(0, 1)]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(-1, 0)]
        [InlineData(1, -1)]
        [InlineData(1, 0)]
        public void CancelShouldThrowException_While_ParametersAreIllegal(long orderId, int userId)
        {
            Action cancel = () => _service.SetCanceled(orderId, userId, out _);
            cancel.Should().Throw<ArgumentException>("cancel should throw exception to avoid nonsence sql query while the parameters are illegal");
        }

        [Fact]
        public void CancelShouldReturnFalse_While_OrderDoesNotExist()
        {
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns((TradeOrder)null);

            var success = _service.SetCanceled(1, 1, out var result);
            success.Should().BeFalse("cancel should return false while the order does not exist");
            result.Should().Be(ChangeOrderStateResult.OrderNotExists, "ChangeOrderStateResult should be OrderNotExists");
        }

        [Fact]
        public void CancelShouldReturnFalse_While_OperatingUserIsNeitherTheSellerNorTheBuyer()
        {
            var order = new TradeOrder { SellerUserId = 1, BuyerUserId = 2, State = TradeOrderState.SellerOperating, };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);

            var success = _service.SetCanceled(1, 3, out var result);
            success.Should().BeFalse("cancel should return false while the operating user is not the seller himself");
            result.Should().Be(ChangeOrderStateResult.Unautherized, "ChangeOrderStateResult should be Unautherized while the operating user is not the seller himself");
        }

        [Theory]
        [InlineData(TradeOrderState.Canceled)]
        [InlineData(TradeOrderState.Completed)]
        [InlineData(TradeOrderState.BuyerPaying)]
        [InlineData(TradeOrderState.PlatformOperating)]
        public void CancelShouldReturnFasle_While_OccuringOrderStateException(TradeOrderState state)
        {
            var userId = 1;
            var order = new TradeOrder { SellerUserId = userId, State = state };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);

            var success = _service.SetCanceled(1, userId, out var result);
            success.Should().BeFalse("cancel should return false while occuring order state exception");
            result.Should().Be(ChangeOrderStateResult.OrderStateException, "ChangeOrderStateResult should be OrderStateException while occuring order state exception");
        }

        [Fact]
        public void CancelShouldReturnFalse_While_UpdatingOrderStateFailed()
        {
            var userId = 1;
            var order = new TradeOrder { SellerUserId = userId, State = TradeOrderState.SellerOperating };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);
            // mock insert log succeed to make sure that it is not the reason causes cancel failed
            Mock.Get(_orderLogRepos).Setup(r => r.Insert(It.IsAny<TradeOrderLog>())).Returns(1);
            Mock.Get(_orderRepos).Setup(r => r.UpdateState(It.IsAny<long>(), It.IsAny<TradeOrderState>())).Returns(false);

            var success = _service.SetCanceled(1, userId, out var result);
            success.Should().BeFalse("cancel should return false while updating order state failed");
            result.Should().Be(ChangeOrderStateResult.PersistenceFailed, "ChangeOrderStateResult should be PersistenceFailed while updating order state failed");
        }

        [Fact]
        public void CancelShouldReturnFalse_While_ItWasFailedToAddOrderLog()
        {
            var userId = 1;
            var order = new TradeOrder { SellerUserId = userId, State = TradeOrderState.SellerOperating };
            Mock.Get(_orderRepos).Setup(r => r.Find(It.IsAny<long>())).Returns(order);
            // mock update order state succeed to make sure that it is not the reason causes cancel failed
            Mock.Get(_orderRepos).Setup(r => r.UpdateState(It.IsAny<long>(), It.IsAny<TradeOrderState>())).Returns(true);
            Mock.Get(_orderLogRepos).Setup(r => r.Insert(It.IsAny<TradeOrderLog>())).Returns(0);

            var success = _service.SetCanceled(1, userId, out var result);
            success.Should().BeFalse("cancel should return false while it is failed to add order log");
            result.Should().Be(ChangeOrderStateResult.PersistenceFailed, "ChangeOrderStateResult should be PersistenceFailed while it is failed to add order log");
        }
        #endregion
    }
}
