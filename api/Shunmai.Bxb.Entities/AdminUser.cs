using Shunmai.Bxb.Entities.Enums;
using System;

namespace Shunmai.Bxb.Entities
{
    public class AdminUser
    {
        public int AdminUserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Contact { get; set; }
        public UserState State { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
