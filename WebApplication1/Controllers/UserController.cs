using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Diagnostics;
using WebApplication1.Models;





namespace WebApplication1.Controllers
{
   
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private string _connectionString;
        private static int _user_id = 1;

        public UserController(IConfiguration config) //Конструктор
        {
            _config = config; //переменная _config для с appsetting.json
            _connectionString = config.GetConnectionString("Db_Connection"); //переменная _connectionString для хранннения строки подключения к бд
        }      

        public string AllUserReq() // метод для отображения всех req пользователя
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "All_USER_REQ";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                oracleCommand.Parameters.Add("p_user_id", OracleDbType.Int32).Value = _user_id;

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

        [HttpPost]
        public IActionResult CreateReq(RequestDetail model) // метод для создания req
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
                            oracleCommand.CommandText = "CREATE_REQ";
                            oracleCommand.Parameters.Add("P_CATEGORY_ID", OracleDbType.Int32).Value = model.CATEGORY_ID; 
                            oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = 1;
                            oracleCommand.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = model.TITLE;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;
                            oracleCommand.Parameters.Add("P_EXECUTE_DATE", OracleDbType.Date).Value = model.EXECUTE_DATE;

                            foreach (OracleParameter param in oracleCommand.Parameters)
                            {
                                Debug.WriteLine($"Parameter Name: {param.ParameterName}, Value: {param.Value}");
                            }


                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    // Перенаправление на другую страницу или отображение успешного сообщения
                    ViewBag.Message = "Request inserted successfully!";
                    // Например, перенаправление на страницу со списком запросов
                    // return RedirectToAction("Requests");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateReqVal(RequestDetail model) // метод для создания доп полей req
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
                            oracleCommand.CommandText = "CREATE_REQ_VAL";
                            oracleCommand.Parameters.Add("P_REQUEST_ID", OracleDbType.Int32).Value = model.REQUEST_ID;
                            oracleCommand.Parameters.Add("P_FIELD_ID", OracleDbType.Int32).Value = model.FIELD_ID;
                            oracleCommand.Parameters.Add("P_FIELD_VALUE", OracleDbType.Varchar2).Value = model.FIELD_VALUE;

                            foreach (OracleParameter param in oracleCommand.Parameters)
                            {
                                Debug.WriteLine($"Parameter Name: {param.ParameterName}, Value: {param.Value}");
                            }


                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    // Перенаправление на другую страницу или отображение успешного сообщения
                    ViewBag.Message = "Request inserted successfully!";
                    // Например, перенаправление на страницу со списком запросов
                    // return RedirectToAction("Requests");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }

        [HttpPost]
        public IActionResult EditRequest([FromBody] RequestDetail model) // метод для изменения req
        {

            /// test section

            // var username = HttpContext.Request.Form["USERNAME"]; //TODO: Delete this

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
                            oracleCommand.CommandText = "UPDATE_REQUEST_BY_ID";

                            // Параметры процедуры
                            oracleCommand.Parameters.Add("REQUEST_ID", OracleDbType.Int32).Value = model.REQUEST_ID;
                            oracleCommand.Parameters.Add("CATEGORY_ID", OracleDbType.Varchar2).Value = model.CATEGORY_ID;
                            oracleCommand.Parameters.Add("USER_ID", OracleDbType.Varchar2).Value = model.USER_ID;
                            oracleCommand.Parameters.Add("TITLE", OracleDbType.Varchar2).Value = model.TITLE;
                            oracleCommand.Parameters.Add("DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;
                            oracleCommand.Parameters.Add("FIELD_ID", OracleDbType.Varchar2).Value = model.FIELD_ID;
                            oracleCommand.Parameters.Add("FIELD_VALUE", OracleDbType.Varchar2).Value = model.FIELD_VALUE;

                            oracleCommand.ExecuteNonQuery();
                            // model.CATEGORY_NAME = username; //TODO: Delete this
                        }
                    }

                    // Успешное обновление
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Логируем ошибку (по возможности используйте более продвинутое логирование)
                    return Json(new { success = false, message = ex.Message });
                }
            }

            // Если модель не валидна
            return Json(new { success = false, message = "Invalid data" });
        }
       
        public string GetUsersInfo() // метод для отображения подробно req
        {
            try
            {
                Request.Form.TryGetValue("val", out var val);
                int.TryParse(val, out int userId);//чтобы не было ошибок
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "GET_REQUEST_BY_ID";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                oracleCommand.Parameters.Add("P_REQUEST_ID", OracleDbType.Int32).Value = userId;
                oracleCommand.Parameters.Add("p_user_id", OracleDbType.Int32).Value = _user_id;

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
        public IActionResult DeleteReq(int id) // метод для блокирования пользователя
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    oracleConnection.Open();

                    using (var oracleCommand = oracleConnection.CreateCommand())
                    {
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        oracleCommand.CommandText = "DELETE_REQ";
                        oracleCommand.Parameters.Add("P_REQUEST_ID", OracleDbType.Int32).Value = id;

                        oracleCommand.ExecuteNonQuery();
                    }
                }

                // Редирект на страницу, например, список пользователей
                return RedirectToAction("AllReq"); // Или на другую страницу
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // В случае ошибки можно вернуть представление с ошибкой или другую логику
                return View("Error"); // Показываем страницу с ошибкой
            }
        }

        public IActionResult Privacy(CalculatorModel calculatorModel)
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

        public IActionResult Requestes()
        {
            return View("Views/User/RequestDetails.cshtml");
        }
        public IActionResult EditRequests()
        {
            return View("Views/User/EditRequest.cshtml");
        }
        
        public IActionResult Privacy() => View();

        public IActionResult CreatesReq()
        {
            return View("Views/User/CreateReq.cshtml");
        }
        public IActionResult AllReq()
        {
            return View("Views/User/AllRequest.cshtml");
        }

        public IActionResult CreatesReqVal()
        {
            return View("Views/User/CreateReqVal.cshtml");
        }

        [HttpGet]
        public JsonResult GetFormData() //выборка данных для dropdown
        {
            var formData = new FormDataViewModel();

            var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
            oracleConnection.Open(); //открываем наше соеденение

            using (var command = new OracleCommand("GET_FORM_DATA", oracleConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Параметры для категорий
                var categoryCursor = command.Parameters.Add("p_categories", OracleDbType.RefCursor);
                categoryCursor.Direction = ParameterDirection.Output;

                // Параметры для пользователей
                var userCursor = command.Parameters.Add("p_users", OracleDbType.RefCursor);
                userCursor.Direction = ParameterDirection.Output;

                // Параметры для полей
                var fieldCursor = command.Parameters.Add("p_fields", OracleDbType.RefCursor);
                    fieldCursor.Direction = ParameterDirection.Output;

                // Параметры для типов полей
                var field_typeCursor = command.Parameters.Add("p_field_types", OracleDbType.RefCursor);
                    field_typeCursor.Direction = ParameterDirection.Output;

                var requestCursor = command.Parameters.Add("p_requests", OracleDbType.RefCursor);
                    requestCursor.Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                // Чтение категорий
                using (var reader = ((OracleRefCursor)categoryCursor.Value).GetDataReader())
                {
                    while (reader.Read())
                    {
                        formData.Categories.Add(new Categoryes
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }

                // Чтение пользователей
                using (var reader = ((OracleRefCursor)userCursor.Value).GetDataReader())
                {
                    while (reader.Read())
                    {
                        formData.Users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }

                // Чтение полей
                using (var reader = ((OracleRefCursor)fieldCursor.Value).GetDataReader())
                {
                    while (reader.Read())
                    {
                        formData.Fields.Add(new Field
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }

                // Чтение типов полей
                using (var reader = ((OracleRefCursor)field_typeCursor.Value).GetDataReader())
                {
                    while (reader.Read())
                    {
                        formData.Field_types.Add(new Field_type
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }

                using (var reader = ((OracleRefCursor)requestCursor.Value).GetDataReader())
                {
                    while (reader.Read())
                    {
                        formData.Requests.Add(new Requeset
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1)
                        });
                    }
                }
            }


            return Json(formData);
        }



    }

}
