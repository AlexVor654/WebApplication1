using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Data.Common;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private string _connectionString;

        public AuthController(IConfiguration config) //Конструктор
        {
            _config = config; //переменная _config для с appsetting.json
            _connectionString = config.GetConnectionString("Db_Connection"); //переменная _connectionString для хранннения строки подключения к бд
        }

        
    }
}

