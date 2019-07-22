namespace Shunmai.Bxb.Services.Models
{
    public class AdminUserQuery: Pager
    {
        public int? AdminUserId { get; set; }
        public string Username { get; set; }
        public int? State { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
