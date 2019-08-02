using Shunmai.Bxb.Api.App.Constansts;
using Shunmai.Bxb.Common.Exceptions;
using Shunmai.Bxb.Common.Models;
using Shunmai.Bxb.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.Utils
{
    public static class ErrorInfoHelper
    {
        public static ErrorInfo FromSubmitResult(OrderSubmitResult result)
        {
            switch (result)
            {
                case OrderSubmitResult.Success:
                    return ErrorInfo.OfRequestSuccess();
                case OrderSubmitResult.TradeHallNotExists:
                    return Errors.TradeHallNotExists;
                case OrderSubmitResult.TradeHallStateException:
                case OrderSubmitResult.TradeHallStatusException:
                    return Errors.CannotTrade;
                case OrderSubmitResult.TradeCodeInputError:
                    return Errors.TradeCodeError;
                case OrderSubmitResult.CountNotEnough:
                    return Errors.NotEnoughCount;
                case OrderSubmitResult.PersistenceFailed:
                    return ErrorInfo.OfRequestFailed();
                default:
                    throw new UnsupportedTypeException(nameof(result), result);
            }
        }

        public static ErrorInfo FromConfirmResult(ChangeOrderStateResult result)
        {
            switch (result)
            {
                case ChangeOrderStateResult.PersistenceFailed:
                    return ErrorInfo.OfRequestFailed();
                case ChangeOrderStateResult.OrderStateException:
                    return Errors.OrderStateException;
                case ChangeOrderStateResult.Unautherized:
                    return ErrorInfo.OfUnauthorized();
                case ChangeOrderStateResult.OrderNotExists:
                    return Errors.OrderNotExists;
                case ChangeOrderStateResult.Success:
                    return ErrorInfo.OfRequestSuccess();
                default:
                    throw new UnsupportedTypeException(nameof(result), result);
            }
        }
    }
}
