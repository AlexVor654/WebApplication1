namespace WebApplication1.Models
{
    public class UserRequest
    {
        public string Username { get; set; }
        public int RequestId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiresDate { get; set; }
        public string Icon { get; set; }
    }
}
