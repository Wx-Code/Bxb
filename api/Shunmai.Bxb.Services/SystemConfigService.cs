using Newtonsoft.Json;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Constans;
using Shunmai.Bxb.Utilities.Validation;
using Shunmai.Bxb.Entities;
using System;

namespace Shunmai.Bxb.Services
{
    public class SystemConfigService
    {
        private readonly ISystemConfigRepository _systemConfigRepository;

        public SystemConfigService(
            ISystemConfigRepository systemConfigRepository
            )
        {
            _systemConfigRepository = systemConfigRepository;
        }

        public T GetConfig<T>(string configName)
        {
            var config = _systemConfigRepository.QuerySingle(configName);
            if (config == null || string.IsNullOrEmpty(config.ConfigValue))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(config.ConfigValue);
        }

        public bool AddOrUpdateConfig(SystemConfig config)
        {
            Check.Null(config, nameof(config));
            var exists = _systemConfigRepository.QuerySingle(config.ConfigName) != null;
            if (exists)
            {
                return _systemConfigRepository.Update(config);
            }
            config.CreateTime = DateTime.Now;
            return _systemConfigRepository.Insert(config);
        }
    }
}
