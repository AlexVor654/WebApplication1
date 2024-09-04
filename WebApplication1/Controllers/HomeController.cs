using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebApplication1.Models;




namespace WebApplication1.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;  
        private string _connectionString;

        public HomeController(IConfiguration config) //Конструктор
        {
            _config = config; //переменная _config для с appsetting.json
            _connectionString = config.GetConnectionString("Db_Connection"); //переменная _connectionString для хранннения строки подключения к бд
        }

        public string ProcedureBlockUser(int Val) 
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "BLOCK_USER";
                oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = Val ;

                oracleCommand.ExecuteNonQuery();
                //для закрытия connection
                //oracleCommand.Dispose();
                //oracleConnection.Close();
                //oracleConnection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public string ProcedureDeleteUSER(int Val)
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "DELETE_USER";
                oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = Val;

                oracleCommand.ExecuteNonQuery();
                //для закрытия connection
                //oracleCommand.Dispose();
                //oracleConnection.Close();
                //oracleConnection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

public IActionResult Requests()
        {
            return View("Views/Home/RequestDetails.cshtml");
        }

        public string GetUsersInfo(string val) // список участников функция
        {
            try
            {
                int.TryParse(val, out int userId);
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "GET_REQUEST_BY_ID_2";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                oracleCommand.Parameters.Add("p_user_id", OracleDbType.Int32).Value = userId;

                oracleCommand.ExecuteNonQuery();

                return ((Oracle.ManagedDataAccess.Types.OracleClob)(oracleCommand.Parameters["RETURN"].Value)).Value;
                //для закрытия connection
                //oracleCommand.Dispose();
                //oracleConnection.Close();
                //oracleConnection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }


        

        public IActionResult Privacy() => View();

        [HttpPost]
        public IActionResult Privacy(CalculatorModel calculatorModel )
        {

            if (calculatorModel.Operation == "plus")
            {
                calculatorModel.Result = calculatorModel.FirstNumber + calculatorModel.SecondNumber;
            }
            else if (calculatorModel.Operation == "minus")
            {
                calculatorModel.Result = calculatorModel.FirstNumber - calculatorModel.SecondNumber;
            }
            else if (calculatorModel.Operation == "umn")
            {
                calculatorModel.Result = calculatorModel.FirstNumber * calculatorModel.SecondNumber;
            }
            else if (calculatorModel.Operation == "delete")
            {
                calculatorModel.Result = calculatorModel.FirstNumber / calculatorModel.SecondNumber;
            }

            return Content($"Your result: {calculatorModel.Result}");
        }




        [HttpGet]
        public IActionResult Insert()
        {
            return View();
        }

        // Обработка данных из формы и вставка в базу данных
        [HttpPost]
        public IActionResult Insert(UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var oracleConnection = new OracleConnection(_connectionString))
                    {
                        oracleConnection.Open();

                        using (var oracleCommand = oracleConnection.CreateCommand())
                        {
                            oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            oracleCommand.CommandText = "InsertUser";

                            oracleCommand.Parameters.Add("p_username", OracleDbType.Varchar2).Value = model.Username;
                            oracleCommand.Parameters.Add("p_email", OracleDbType.Varchar2).Value = model.Email;
                            oracleCommand.Parameters.Add("p_password", OracleDbType.Varchar2).Value = model.Password;

                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    ViewBag.Message = "User inserted successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }
            return View(model);
        }

    }

}
