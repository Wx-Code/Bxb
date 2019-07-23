using Shunmai.Bxb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public partial interface ISystemConfigRepository
    {
        bool Insert(SystemConfig config);
        bool Update(SystemConfig config);
        bool Exists(string configName);
        SystemConfig QuerySingle(object condition);

    }
}
