using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Entities
{
    /// <summary>
    /// 基础配置
    /// </summary>
    public class SystemConfig
    {
        public string ConfigName { get; set; }

        public string ConfigValue { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }


    }
}
