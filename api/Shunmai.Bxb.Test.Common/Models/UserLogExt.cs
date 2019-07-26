
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{
    ///<summary>
    /// Table, userlog
    ///</summary>
    public class UserLogExt : UserLog
    {
        [Key]
        public override long UserLogId { get; set; }
    }
}

