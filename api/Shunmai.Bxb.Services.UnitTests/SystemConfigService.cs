using Moq;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Shunmai.Bxb.Services.UnitTests
{
    public class SystemConfigServiceTests
    {
        SystemConfigService _service;
        ISystemConfigRepository _repos;

        public SystemConfigServiceTests()
        {
            _repos = Mock.Of<ISystemConfigRepository>();
            _service = new SystemConfigService(_repos);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123456")]
        [InlineData("\"_$%{}\"")]
        [InlineData("abcd\naced\r\n")]
        public void GetConfig_Should_WorkWell_OnVariesOfConfigValues(string value)
        {
            Mock.Get(_repos).Setup(r => r.QuerySingle(It.IsAny<string>()))
                .Returns(new SystemConfig
                {
                    ConfigValue = value
                });
            var actual = _service.GetConfig<string>(It.IsAny<string>());
            Assert.Equal(value, actual);
        }
    }
}
