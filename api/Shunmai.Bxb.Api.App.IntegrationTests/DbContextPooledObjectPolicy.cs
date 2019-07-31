using Microsoft.Extensions.ObjectPool;
using Shunmai.Bxb.Test.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Api.App.IntegrationTests
{
    public class DbContextPooledObjectPolicy : PooledObjectPolicy<BxbContext>
    {
        public override BxbContext Create()
        {
            return BxbContext.FromSmartSqlConfig();
        }

        public override bool Return(BxbContext obj)
        {
            return true;
        }
    }
}
