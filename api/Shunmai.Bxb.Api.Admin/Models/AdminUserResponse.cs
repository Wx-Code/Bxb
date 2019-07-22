using System;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class AdminUserResponse
    {
        public int AdminUserId { get; set; }
        public string Username { get; set; }
        public string Contact { get; set; }
        public int State { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
