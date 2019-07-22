using Shunmai.Bxb.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class AdminUserRequest
    {
        public int AdminUserId { get; set; }
        [Required, MaxLength(20)]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public UserState State { get; set; }
    }
}
