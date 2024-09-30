using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразуем строку в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Вычисляем хэш
                byte[] hash = sha256.ComputeHash(bytes);

                // Преобразуем хэшированный массив байтов обратно в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                return builder.ToString(); // Возвращаем хэшированную строку
            }
        }
    }
}
