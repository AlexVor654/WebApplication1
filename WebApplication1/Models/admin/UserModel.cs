namespace WebApplication1.Models
{
    public class UserModel
    {
        public int RoleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FIRST_NAME { get; set; } = string.Empty;
        public string LAST_NAME { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PHONE { get; set; } = string.Empty;
        public string PASSWD { get; set; } = string.Empty;
    }
}
