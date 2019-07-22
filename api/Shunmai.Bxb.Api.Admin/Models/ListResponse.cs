using System.Collections.Generic;

namespace Shunmai.Bxb.Api.Admin.Models
{
    public class ListResponse<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> List { get; set; }
    }
}
