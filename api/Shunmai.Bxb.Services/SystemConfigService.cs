using Newtonsoft.Json;
using Shunmai.Bxb.Common.Constants;
using Shunmai.Bxb.Entities;
using Shunmai.Bxb.Repositories.Interfaces;
using Shunmai.Bxb.Services.Models;
using Shunmai.Bxb.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services
{
    public class SystemConfigService
    {
        private readonly ISystemConfigRepository _systemConfigRepository;

        public SystemConfigService(ISystemConfigRepository systemconfigRepository)
        {
            _systemConfigRepository = systemconfigRepository;
        }

        public bool AddOrUpdateConfig(SystemConfig systemconfig)
        {
            Check.Null(systemconfig, nameof(systemconfig));

            var exists = _systemConfigRepository.QuerySingle(systemconfig.ConfigName) != null;
            if (exists)
            {
                return _systemConfigRepository.Update(systemconfig);
            }

            systemconfig.CreateTime = DateTime.Now;
            var query = _systemConfigRepository.Insert(systemconfig);

            return query;
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        private T GetConfig<T>(string configName)
        {
            var config = _systemConfigRepository.QuerySingle(configName);
            if (config == null || string.IsNullOrEmpty(config.ConfigValue))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(config.ConfigValue);
        }

        /// <summary>
        /// 获取平台钱包地址配置信息
        /// </summary>
        /// <returns></returns>
        public List<PlatWalletAddrConfigModel> GetPlatWalletAddrConfigList()
        {
            return GetConfig<List<PlatWalletAddrConfigModel>>("PlatWalletAddrConfig");
        }

    }
}
