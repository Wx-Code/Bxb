using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using SmartSql.DyRepository.Annotations;

namespace Shunmai.Bxb.Repositories.Interfaces
{
    public interface ISmsVerificationCodeRepository
    {
        int Insert(SmsVerificationCode model);
        SmsVerificationCode QuerySingle(string phone, string code);
        [Statement(Id = nameof(ISmsVerificationCodeRepository.QuerySingle))]
        SmsVerificationCode QueryNonExpired(string phone, int expires);
        bool UpdateState(int id, SmsCodeState state);
    }
}
