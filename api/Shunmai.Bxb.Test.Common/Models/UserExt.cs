
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class UserExt : User
    {
        [Key]
        public override int UserId { get; set; }
    }
}

