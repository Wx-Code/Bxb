
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class TradeHallLogExt : TradeHallLog
    {
        [Key]
        public override long LogId { get; set; }
    }
}

