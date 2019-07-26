using Shunmai.Bxb.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class SmsCode : SmsVerificationCode
    {
        [Key]
        public override int VcId { get; set; }
    }
}
