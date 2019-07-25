namespace Shunmai.Bxb.Services.Models
{
    public class Pager
    {
        private int _page;

        public int Page
        {
            get => _page <= 0 ? 1 : _page;

            set => _page = value;
        }

        private int _size;

        public int Size
        {
            get => _size <= 0 ? 10 : _size;

            set => _size = value;
        }

        public int Offset => (Page - 1) * Size;
    }
}
