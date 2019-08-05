﻿namespace Shunmai.Bxb.Services.Enums
{
    public enum ChangeOrderStateResult
    {
        PersistenceFailed = -4,

        OrderStateException = -3,

        Unautherized = -2,

        OrderNotExists = -1,

        Success = 1,
    }
}