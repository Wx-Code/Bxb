using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.Models.Request
{
    public class SubmitRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int TradeId { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int RequiredCount { get; set; }
        [Required]
        public string TradeCode { get; set; }
    }
}
