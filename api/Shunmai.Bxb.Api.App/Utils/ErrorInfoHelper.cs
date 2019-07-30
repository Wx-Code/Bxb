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
        public static ErrorInfo FromSubmitResult(this ErrorInfo errorInfo, OrderSubmitResult result)
        {
            switch (result)
            {
                case OrderSubmitResult.Success:
                    return ErrorInfo.OfRequestSuccess();
                case OrderSubmitResult.TradeHallNotExists:
                    return Errors.TradeHallNotExists;
                case OrderSubmitResult.TradeHallStateException:
                    break;
                case OrderSubmitResult.TradeHallStatusException:
                    break;
                case OrderSubmitResult.TradeCodeInputError:
                    break;
                case OrderSubmitResult.CountNotEnough:
                    break;
                case OrderSubmitResult.PersistenceFailed:
                    break;
                default:
                    throw new UnsupportedTypeException(nameof(result), result);
            }
        }
    }
}
