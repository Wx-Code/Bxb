using Shunmai.Bxb.Entities;
using System.Collections.Generic;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public partial interface IAdminUserRepository
    {
        int Insert(AdminUser user);
        bool Update(AdminUser user);
        bool Exists(string username, int excludedUserId = 0);
        AdminUser QuerySingle(object condition);
        int Count(object condition);
        List<AdminUser> QueryList(object condition);
    }
}
