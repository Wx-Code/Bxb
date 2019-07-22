using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class PasswordUpdateRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
