
using Shunmai.Bxb.Entities;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ISystemConfigRepository
    {
        bool Insert(SystemConfig config);
        bool Update(SystemConfig config);
        SystemConfig QuerySingle(string configName);
    }
}
