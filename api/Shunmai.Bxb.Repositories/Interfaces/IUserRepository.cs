using Shunmai.Bxb.Entities;
using SmartSql.DyRepository.Annotations;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int Insert(User user);
        bool Update(User user);
        bool Exists(User user);
        [Statement(Id = "QuerySingle")]
        User FindById(int userId);
        [Statement(Id = "QuerySingle")]
        User FindByOpenId(string openId);
    }
}
