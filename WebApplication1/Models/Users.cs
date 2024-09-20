namespace WebApplication1.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Храните хэш пароля, а не сам пароль
    }

}
