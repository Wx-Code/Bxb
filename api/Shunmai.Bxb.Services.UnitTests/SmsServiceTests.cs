using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Entities.Enums;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Utilities.Sms;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Shunmai.Bxb.Services.UnitTests
{
    public class SmsServiceTests
    {
        ILogger<SmsService> _logger;
        ISmsProvider _smsProvider;
        ISmsVerificationCodeRepository _codeRepos;
        ISmsLogRepository _logRepos;
        SmsService _service;

        public SmsServiceTests()
        {
            _logger = Mock.Of<ILogger<SmsService>>();
            _smsProvider = Mock.Of<ISmsProvider>();
            _codeRepos = Mock.Of<ISmsVerificationCodeRepository>();
            _logRepos = Mock.Of<ISmsLogRepository>();
            _service = new SmsService(_logger, _smsProvider, _codeRepos, _logRepos);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(null, -1)]
        [InlineData("", 0)]
        [InlineData("", -1)]
        public void SendSmsCode_Should_ThrowsException_WhileArgsInvalid(string phone, int length)
        {
            Assert.Throws<ArgumentException>(() => _service.SendSmsCode(phone, length, default(ApplicationType)));
        }

        [Theory]
        [InlineData(null, null, 0)]
        [InlineData(null, null, -1)]
        [InlineData("", "", 0)]
        [InlineData("", "", -1)]
        public void Validate_Should_ThrowsException_WhileArgsInvalid(string phone, string code, int length)
        {
            Assert.Throws<ArgumentException>(() => _service.Validate(phone, code, length));
        }

        [Fact]
        public void Validate_Should_ReturnFalse_While_QueryNonExpiredReturnsNull()
        {
            Mock.Get(_codeRepos).Setup(p => p.QueryNonExpired(It.IsAny<string>(), It.IsAny<int>()))
                .Returns((SmsVerificationCode)null);
            var valid = _service.Validate("13521942500", "123456", 10 * 60);
            Assert.False(valid);
        }

        [Fact]
        public void Validate_Should_ReturnFalse_While_CodeIsNotEqual()
        {
            Mock.Get(_codeRepos).Setup(p => p.QueryNonExpired(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new SmsVerificationCode {
                    VerificationCode = "456789"
                });
            var valid = _service.Validate("13521942500", "123456", 10 * 60);
            Assert.False(valid);
        }

        [Theory]
        [InlineData(SmsCodeState.Expired)]
        [InlineData(SmsCodeState.Verified)]
        public void Validate_Should_ReturnFalse_While_CodeStateIsNotValid(SmsCodeState state)
        {
            var code = "123456";
            Mock.Get(_codeRepos).Setup(p => p.QueryNonExpired(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new SmsVerificationCode
                {
                    VerificationCode = code,
                    State = state,
                });
            var valid = _service.Validate("13521942500", code, 10 * 60);
            Assert.False(valid);
        }

        [Fact]
        public void Validate_Should_ReturnTrue_While_EveryThingMatches()
        {
            var code = "123456";
            Mock.Get(_codeRepos).Setup(r => r.QueryNonExpired(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new SmsVerificationCode
                {
                    VerificationCode = code,
                    State = SmsCodeState.Default,
                });
            Mock.Get(_codeRepos).Setup(r => r.UpdateState(It.IsAny<int>(), It.IsAny<SmsCodeState>())).Returns(true);
            var valid = _service.Validate("13521942500", code, 10 * 60);
            Assert.True(valid);
        }
    }
}
