using Shunmai.Bxb.Entities;
using SmartSql.DyRepository.Annotations;
using System.Collections.Generic;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int Insert(User user);
        [Statement(Id = "QuerySingle")]
        User FindById(int userId);
        [Statement(Id = "QuerySingle")]
        User FindByOpenId(string openId);
        [Statement(Id = "QuerySingle")]
        User FindByPhone(string phone);
        [Statement(Id = "Exists")]
        bool ExistsByPhone(string phone);
        [Statement(Id = "Exists")]
        bool ExistsByOpenId(string openId);

        bool Update(object condition);
        int Count(object condition);
        List<User> QueryList(object condition);

        User QueryUserDetail(int userId);
    }
}
