using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shunmai.Bxb.Api.App.Models.Response
{
    public class WXCustomerInfoResponse
    {
        public string CustomerTel { get; set; }

        public List<CustomerNumber> WeiXinCustomerList { get; set; }

        public class CustomerNumber
        {
            public int WXCustomerId { get; set; }

            public string WXCustomerNumber { get; set; }

            public bool IsChecked { get; set; }

        }
    }
}
