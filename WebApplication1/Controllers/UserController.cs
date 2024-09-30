using Microsoft.AspNetCore.Mvc;

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using WebApplication1.Models;
using WebApplication1.Helpers;





namespace WebApplication1.Controllers
{
    //[CustomAuthorize(1)]
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private string _connectionString;

        private LogHelper _logHelper; // Поле для LogHelper
        public UserController(IConfiguration config) //Конструктор
        {
            _config = config; // Переменная для работы с appsettings.json
            _connectionString = config.GetConnectionString("Db_Connection"); // Переменная для строки подключения к БД
            _logHelper = new LogHelper(_config); // Создание LogHelper с помощью конфигурации
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

        //+
        public string GetRequestsList()
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString)) // Используем using для автоматического закрытия соединения
                {
                    oracleConnection.Open(); // Открываем соединение

                    using (var oracleCommand = oracleConnection.CreateCommand())
                    {
                        oracleCommand.CommandType = CommandType.StoredProcedure;
                        oracleCommand.CommandText = "All_USER_REQ";//??

                        // Возвращаемое значение
                        oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                        // Передаем ID пользователя
                        oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = HttpContext.Session.GetInt32("UserId");

                        oracleCommand.ExecuteNonQuery(); // Выполняем команду

                        // Получаем данные из CLOB
                        var returnValue = oracleCommand.Parameters["RETURN"].Value as Oracle.ManagedDataAccess.Types.OracleClob;

                        // Возвращаем содержимое CLOB
                        if (returnValue != null)
                        {
                            return returnValue.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или возвращаем сообщение об ошибке 
                Console.WriteLine($"Ошибка в методе GetRequestsList: {ex.Message}");
                // Возможно, стоит добавить более подробное логирование
            }

            return string.Empty; // Если произошла ошибка или данных нет, возвращаем пустую строку
        }


        //+
        public string GetDetailRequest([FromForm] int RequestId) // метод для отображения подробно req
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString)) { 
                   // Request.Form.TryGetValue("val", out var val);
                    //int.TryParse(val, out int ReqId);//чтобы не было ошибок    
                    oracleConnection.Open(); //открываем наше соеденение
                    using (var oracleCommand = oracleConnection.CreateCommand()) { 
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "GET_REQUEST_BY_ID";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                oracleCommand.Parameters.Add("P_REQUEST_ID", OracleDbType.Int32).Value = RequestId;
                oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = HttpContext.Session.GetInt32("UserId");

                oracleCommand.ExecuteNonQuery();

                        _logHelper.LogAction((int)userId, "GetDetailRequest page", $"user with id:{userId} view detail Req page", 1);
                        return ((Oracle.ManagedDataAccess.Types.OracleClob)(oracleCommand.Parameters["RETURN"].Value)).Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "AllReq page", $"user with id:{userId} can not view AllReq page", 2);
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }


        //+
        [HttpPost]
        public IActionResult CreateRequest(Request model) // метод для создания req
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
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
                            oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = HttpContext.Session.GetInt32("UserId");
                            oracleCommand.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = model.TITLE;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;
                            oracleCommand.Parameters.Add("P_EXECUTE_DATE", OracleDbType.Date).Value = model.EXECUTE_DATE;

                           

                            oracleCommand.ExecuteNonQuery();
                        }
                    }
                    _logHelper.LogAction((int)userId, "CreateReq", $"user with id:{userId} creating req", 1);
                    // Перенаправление на другую страницу или отображение успешного сообщения
                    ViewBag.Message = "Request inserted successfully!";
                    
                }
                catch (Exception ex)
                {
                    _logHelper.LogAction((int)userId, "Failed CreateReq", $"user with id:{userId} can not creating req", 2);
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }

        //+(но будут улучшения, так как данные заполняются не динамически с методом CreateReq)
        [HttpPost]
        public IActionResult CreateReqVal(Request model) // метод для создания доп полей req
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
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

                           
                            
                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    // Перенаправление на другую страницу или отображение успешного сообщения
                    _logHelper.LogAction((int)userId, "CreateReq", $"user with id:{userId} creating req values", 1);
                    return Json(new { success = true, message = "Category created successfully" });
                    // Например, перенаправление на страницу со списком запросов
                    // return RedirectToAction("Requests");
                }
                catch (Exception ex)
                {
                    _logHelper.LogAction((int)userId, "Failed CreateReq", $"user with id:{userId} can not creating req", 2);
                    return Json(new { success = false, message = ex.Message });
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }


        [HttpPost]
        public IActionResult UpdateRequest([FromBody] Request model) // метод для изменения req
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

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
                            
                        }
                    }
                    _logHelper.LogAction((int)userId, "EditReq", $"user with id:{userId} edit his req values", 1);
                    // Успешное обновление
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logHelper.LogAction((int)userId, "Failed EditReq", $"user with id:{userId} can not edit his req values", 2);
                    // Логируем ошибку (по возможности используйте более продвинутое логирование)
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return View(model);
        }
       
        
        public IActionResult DeleteRequest(int RequestId) // метод для блокирования пользователя
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    oracleConnection.Open();

                    using (var oracleCommand = oracleConnection.CreateCommand())
                    {
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        oracleCommand.CommandText = "DELETE_REQ";
                        oracleCommand.Parameters.Add("P_REQUEST_ID", OracleDbType.Int32).Value = RequestId;

                        oracleCommand.ExecuteNonQuery();
                    }
                }
                _logHelper.LogAction((int)userId, "DeleteRequest", $"user with id:{userId} deleting Req", 1);
                // Редирект на страницу, например, список пользователей
                return RedirectToAction("RequestsList"); // Или на другую страницу
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "Failed DeleteRequest", $"user with id:{userId} can not deleting Req", 2);
                // В случае ошибки можно вернуть представление с ошибкой или другую логику
                return View("Error"); // Показываем страницу с ошибкой
            }
        }


        public IActionResult DetailRequest()
        {
            return View("Views/User/DetailRequest.cshtml");
        }
        public IActionResult EditRequest()
        {
            return View("Views/User/EditRequest.cshtml");
        }
        
        public IActionResult Privacy() => View();

        public IActionResult AddRequest()
        {
            return View("Views/User/CreateRequest.cshtml");
        }
        public IActionResult RequestsList()
        {
            return View("Views/User/RequestsList.cshtml");
        }

        public IActionResult AddReqValues()
        {
            return View("Views/User/CreateReqVal.cshtml");
        }

        [HttpGet]
        public JsonResult GetFormData() //выборка данных для dropdown
        {
            var formData = new FormDataViewModel();
            using (var oracleConnection = new OracleConnection(_connectionString)) { 

           
            oracleConnection.Open(); //открываем наше соеденение

            using (var command = new OracleCommand("USER_GET_FORM_DATA", oracleConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметр user_id
                    var userIdParam = command.Parameters.Add("p_user_id", OracleDbType.Int32);
                    userIdParam.Value = HttpContext.Session.GetInt32("UserId");

                    // Добавляем параметр user_id
                    var CategoryParam = command.Parameters.Add("p_category_id", OracleDbType.Int32);
                    CategoryParam.Value = 2;
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
                }

            return Json(formData);
        }



    }

}
