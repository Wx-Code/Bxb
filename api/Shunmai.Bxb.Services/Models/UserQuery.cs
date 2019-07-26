using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models
{
    public class UserQuery:Pager
    {
        public string NickName { get; set; }
        public string Phone { get; set; }
        public string WalletAddress { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
