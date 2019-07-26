using Shunmai.Bxb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface IUserLogRepository
    {
        int Insert(UserLog user);

        int Count(object condition);
        List<UserLog> QueryList(object condition);

        UserLog QuerySigle(int userId);
    }
}
