using System;

namespace Shunmai.Bxb.Utilities.Sms
{
    public class ResponseEntity
    {
        public string Content{ get; set; }

        public DateTime SendTime { get; set; }

        public string PhoneNo { get; set; }

        public string Code { get; set; }

        public int Provider { get; set; } = 0;
    }
}
