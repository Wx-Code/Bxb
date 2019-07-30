using Microsoft.Extensions.Logging;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Attributes;
using Shunmai.Bxb.Utilities.Extenssions;
using Shunmai.Bxb.Utilities.Sms;
using Shunmai.Bxb.Utilities.Check;
using System;
using Shunmai.Bxb.Utilities.Helpers;

namespace Shunmai.Bxb.Services
{
    public class SmsService
    {
        private readonly ILogger _logger;
        private readonly ISmsProvider _smsProvider;
        private readonly ISmsVerificationCodeRepository _verificationCodeRepository;
        private readonly ISmsLogRepository _smsLogRepository;

        public SmsService(
            ILogger<SmsService> logger
            , ISmsProvider smsProvider
            , ISmsVerificationCodeRepository verificationCodeRepository
            , ISmsLogRepository smsLogRepository
        )
        {
            _logger = logger;
            _smsProvider = smsProvider;
            _verificationCodeRepository = verificationCodeRepository;
            _smsLogRepository = smsLogRepository;
        }

        public SmsVerificationCode QueryNonExpired(string phone, int expireSeconds)
        {
            return _verificationCodeRepository.QueryNonExpired(phone, expireSeconds);
        }

        private bool IsValid(SmsVerificationCode code)
        {
            return code.State == SmsCodeState.Default;
        }

        /// <summary>
        /// 向给定手机号码发送短信验证码，采用以下策略：
        /// 将数据库中状态为 0 的验证码设置为已过期
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="length"></param>
        /// <param name="expireSeconds">验证码过期时间（单位：秒），默认 10 分钟</param>
        /// <returns></returns>
        [SmartSqlTransaction]
        public virtual bool SendSmsCode(string phone, int length, ApplicationType appType, int expireSeconds = 10 * 60)
        {
            Check.Empty(phone, nameof(phone));
            Check.EnsureMoreThanZero(length, nameof(length));
            Check.EnsureMoreThanZero(expireSeconds, nameof(expireSeconds));

            var cnt = _verificationCodeRepository.Count(phone, SmsCodeState.Default);
            var success = _verificationCodeRepository.SetExpired(phone) == cnt;
            if (success == false)
            {
                _logger.LogError($"Set code expired failed.");
                return false;
            }

            var code = Randoms.Numbers(length);
            var smsLog = new SmsLog
            {
                Content = code,
                Phone = phone,
                Sign = string.Empty,
                RequestPlat = appType,
                SendTime = DateTime.Now,
                State = SmsSendState.Failed,
                Type = SmsType.Verification,
            };
            var logId = _smsLogRepository.Insert(smsLog);
            if (logId == 0)
            {
                _logger.LogError($"Insert sms log failed.");
                return false;
            }

            var codeModel = new SmsVerificationCode
            {
                Phone = phone,
                State = SmsCodeState.Default,
                VerificationCode = code,
            };
            var codeId = _verificationCodeRepository.Insert(codeModel);
            if (codeId == 0)
            {
                _logger.LogError($"Insert verification code failed.");
                return false;
            }

            var res = _smsProvider.SendCode(phone, code);
            if (res == null || res.Code.IsEmpty())
            {
                _logger.LogError($"Send verification code failed.");
                return false;
            }

            return true;
        }

        [SmartSqlTransaction]
        public virtual bool Validate(string phone, string code, int expireSeconds)
        {
            Check.Empty(phone, nameof(phone));
            Check.Empty(code, nameof(code));
            Check.EnsureMoreThanZero(expireSeconds, nameof(expireSeconds));

            var nonExpired = QueryNonExpired(phone, expireSeconds);
            if (nonExpired == null 
                || nonExpired.VerificationCode != code 
                || IsValid(nonExpired) == false)
            {
                return false;
            }

            var success = _verificationCodeRepository.UpdateState(nonExpired.VcId, SmsCodeState.Verified);
            if (success == false)
            {
                _logger.LogError("Failed to change the code state to verified.");
                return false;
            }
            return true;
        }
    }
}
