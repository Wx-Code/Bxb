using Shunmai.Bxb.Entities;
using SmartSql.DyRepository.Annotations;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int Insert(User user);
        bool Update(User user);
        [Statement(Id = "QuerySingle")]
        User FindById(int userId);
        [Statement(Id = "QuerySingle")]
        User FindByOpenId(string openId);
        [Statement(Id = "Exists")]
        bool ExistsByPhone(string phone);
        [Statement(Id = "Exists")]
        bool ExistsByOpenId(string openId);
    }
}
