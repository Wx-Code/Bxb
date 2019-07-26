
using Shunmai.Bxb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Test.Common.Models
{
    public class UserExt : User
    {
        [Key]
        public override int UserId { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;
            if (!(obj is User user))
                return false;
            return UserId == user.UserId
                && Phone == user.Phone;
        }
    }
}

