using System.ComponentModel.DataAnnotations;

namespace Shunmai.Bxb.Services.Models
{
    public class Pager
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Offset => (Page - 1) * Size;

        public Pager()
        {

        }
        public Pager(int page, int size)
        {
            Page = page;
            Size = size;
        }
    }
}
