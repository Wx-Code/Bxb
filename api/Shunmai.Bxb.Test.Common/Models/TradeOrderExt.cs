
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class TradeOrderExt : TradeOrder
    {
        [Key]
        public override long OrderId { get; set; }
    }
}

