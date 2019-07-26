using Shunmai.Bxb.Entities;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ISmsLogRepository
    {
        long Insert(SmsLog model);
    }
}
