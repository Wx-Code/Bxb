using Shunmai.Bxb.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Api.App.Models.Request
{
    public class LoginRequest
    {
        [Required, MobilePhone]
        public string Phone { get; set; }
        [Required, Pattern(@"^\d{4,6}$")]
        public string SmsCode { get; set; }
    }
}
