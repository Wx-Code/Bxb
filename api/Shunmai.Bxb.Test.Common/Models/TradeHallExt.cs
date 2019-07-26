
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class TradeHallExt : TradeHall
    {
        [Key]
        public override int TradeId { get; set; }
    }
}

