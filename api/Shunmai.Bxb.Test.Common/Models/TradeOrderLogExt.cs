
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{

    ///<summary>
    /// Table, tradeorderlog
    ///</summary>
    public class TradeOrderLogExt : TradeOrderLog
    {
        [Key]
        public override long LogId { get; set; }
    }
}

