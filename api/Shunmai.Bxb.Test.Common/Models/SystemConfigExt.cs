using Shunmai.Bxb.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class SystemConfigExt : SystemConfig
    {
        [Key]
        public override string ConfigName { get; set; }
    }
}
