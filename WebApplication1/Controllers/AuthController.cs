using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private string _connectionString;

        public AuthController(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("Db_Connection");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            bool isAuthenticated = AuthenticateUser(username, password, out int userId, out int userRole);
            
            if (isAuthenticated && userId != -1 && userRole > 0 && userRole > 0)
            {
                var logHelper = new LogHelper(_config);
                logHelper.LogAction(userId, "login", $"user{userId} logged in", 1);
                // Сохранение ID пользователя и его роли в сессии
                HttpContext.Session.SetInt32("UserId", userId);
                HttpContext.Session.SetInt32("UserRole", userRole);
                // Успешный вход, перенаправляем пользователя на защищенную страницу
                return RedirectToRoleBasedPage(userRole);
            }
            else
            {
                return View(); // Исправление пути к представлению
            }
        }

        private IActionResult RedirectToRoleBasedPage(int userRole)
        {
            switch (userRole)
            {
                case 1:
                    return RedirectToAction("RequestsList", "User"); 
                case 2:
                    return RedirectToAction("Requests", "Admin"); 
                default:
                    return RedirectToAction("Login", "Auth"); 
            }
        }

        private bool AuthenticateUser(string username, string password, out int userId, out int userRole)
        {
            userId = -1; // Начальная установка ID
            userRole = 0; // Начальная установка роли
            using (var connection = new OracleConnection(_connectionString))
            {
                using (var command = new OracleCommand("AUTHENTICATE_USER", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Входные параметры
                    command.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                   // command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = PasswordHelper.HashPassword(password);
                    command.Parameters.Add("p_password", OracleDbType.Varchar2).Value =password;

                    // Выходной параметр
                    command.Parameters.Add("p_is_authenticated", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                    command.Parameters.Add("p_user_id", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                    command.Parameters.Add("p_user_role", OracleDbType.Decimal).Direction = ParameterDirection.Output; // Укажите соответствующий размер

                    connection.Open();
                    command.ExecuteNonQuery();

                    
                    OracleDecimal result = (OracleDecimal)command.Parameters["p_is_authenticated"].Value;
                    userId = ((OracleDecimal)command.Parameters["p_user_id"].Value).ToInt32();
                    userRole = ((OracleDecimal)command.Parameters["p_user_role"].Value).ToInt32();
                    
                    int isAuthenticated = result.ToInt32(); // Преобразуем OracleDecimal в int

                    return isAuthenticated == 1; // 1 означает успешную аутентификацию
                }
            }
        }


        public IActionResult CheckSession()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            int? userRole = HttpContext.Session.GetInt32("UserRole");

            if (userId.HasValue && userRole.HasValue)
            {
                return Content($"Session is active. UserId: {userId}, UserRole: {userRole}");
            }
            else
            {
                return Content("Session is not active or values are missing.");
            }
        }

        public IActionResult Logout()
        {
            // Получаем userId из сессии
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Логируем действие пользователя, если userId не null
            if (userId.HasValue)
            {
                var logHelper = new LogHelper(_config);
                logHelper.LogAction(userId.Value, "logout", $"user {userId} logout from system", 1);
            }

            // Очищаем сессию
            HttpContext.Session.Clear();

            // Перенаправляем пользователя на страницу входа
            return RedirectToAction("Login", "Auth");
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Отображение страницы Login.cshtml
        }
    }
}
