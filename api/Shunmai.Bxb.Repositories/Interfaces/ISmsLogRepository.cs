﻿using Shunmai.Bxb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ISmsLogRepository
    {
        long Insert(SmsLog model);
    }
}
