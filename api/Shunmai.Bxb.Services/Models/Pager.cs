namespace Shunmai.Bxb.Services.Models
{
    public class Pager
    {
        private int _page;

        public int Page
        {
            get => _page;

            set => _page = value <= 0 ? 1 : value;
        }

        private int _size;

        public int Size
        {
            get => _size;

            set => _size = value <= 0 ? 10 : value;
        }

        public int Offset => (Page - 1) * Size;
    }
}
