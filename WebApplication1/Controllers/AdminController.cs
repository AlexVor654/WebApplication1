using Microsoft.AspNetCore.Mvc;

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Helpers;




namespace WebApplication1.Controllers
{
    //[CustomAuthorize(2)]
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;  
        private string _connectionString;
        private LogHelper _logHelper; // Поле для LogHelper
        public AdminController(IConfiguration config) //Конструктор
        {
            _config = config; // Переменная для работы с appsettings.json
            _connectionString = config.GetConnectionString("Db_Connection"); // Переменная для строки подключения к БД
            _logHelper = new LogHelper(_config); // Создание LogHelper с помощью конфигурации
        }
        

        //+
        public string GetRequests()  // метод для отображение всех req
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString)) 
                {
                oracleConnection.Open(); //открываем наше соеденение
                    using (var oracleCommand = oracleConnection.CreateCommand()) { 
                
                    oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    oracleCommand.CommandText = "ALL_REQUEST";
                    oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                
                oracleCommand.ExecuteNonQuery();

                _logHelper.LogAction((int)userId, "Requests page", $"admin {userId} view AllReq page", 1);
                return ((Oracle.ManagedDataAccess.Types.OracleClob)(oracleCommand.Parameters["RETURN"].Value)).Value;
            }
            }
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "Failed Requests page", $"admin {userId} can not view AllCategory page", 2);
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        //+
        public string GetCategories() // метод для отображение всех categories
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using(var oracleConnection = new OracleConnection(_connectionString)) 
                { 
                oracleConnection.Open(); //открываем наше соеденение
                using (var oracleCommand = oracleConnection.CreateCommand()) 
                {                
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "ALL_CATEGORY";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;


                oracleCommand.ExecuteNonQuery();
                _logHelper.LogAction((int)userId, "Categories page", $"admin {userId} view AllCategory page", 1);
                return ((Oracle.ManagedDataAccess.Types.OracleClob)(oracleCommand.Parameters["RETURN"].Value)).Value;
                    }
                }    
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "Failed Categories page", $"admin {userId} can not view AllCategory page", 2);
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        //+
        public string GetUsers() // метод для отображение всех пользователей
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString)) 
                { 
                oracleConnection.Open(); //открываем наше соеденение
                    using (var oracleCommand = oracleConnection.CreateCommand())
                    {
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        oracleCommand.CommandText = "ALL_USERS";
                        oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;

                        oracleCommand.ExecuteNonQuery();
                        _logHelper.LogAction((int)userId, "Users page", $"admin {userId} view Users page", 1);

                        return ((Oracle.ManagedDataAccess.Types.OracleClob)(oracleCommand.Parameters["RETURN"].Value)).Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "Failed Users page", $"admin {userId} can not view Users page", 2);
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        //+
        public string GetCategoryValues() // метод для отображение всех доп полей categories
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString)) 
                { 
                oracleConnection.Open(); //открываем наше соеденение
                    using ( var oracleCommand = oracleConnection.CreateCommand()) 
                    {               
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "ALL_CATEGORY_VAL";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                oracleCommand.ExecuteNonQuery();

                _logHelper.LogAction((int)userId, "CategoryValues page", $"admin {userId} view CategoryValues page", 1);
                return ((Oracle.ManagedDataAccess.Types.OracleClob)(oracleCommand.Parameters["RETURN"].Value)).Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "Failed CategoryValues page", $"admin {userId} can not view CategoryValues page", 2);
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        
        
        //+
        [HttpPost]
        public IActionResult UpdateCategoryValues([FromBody] CategoryVal model) // метод для отображение изменение доп поля category
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
                            oracleCommand.CommandText = "UPDATE_CATEGORY_FIELD";

                            // Параметры процедуры
                            oracleCommand.Parameters.Add("P_FIELD_ID", OracleDbType.Int32).Value = model.FIELD_ID;
                            oracleCommand.Parameters.Add("P_CATEGORY_ID", OracleDbType.Varchar2).Value = model.CATEGORY_ID;
                            oracleCommand.Parameters.Add("P_FIELD_TYPE_ID", OracleDbType.Varchar2).Value = model.FIELD_TYPE_ID;
                            oracleCommand.Parameters.Add("P_FIELD_NAME", OracleDbType.Varchar2).Value = model.FIELD_NAME;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;

                            oracleCommand.Parameters.Add("P_DISPLAY_TEXT", OracleDbType.Varchar2).Value = model.DISPLAY_TEXT;
                            oracleCommand.Parameters.Add("P_FIELD_VALUE", OracleDbType.Varchar2).Value = model.FIELD_VALUE;
                            oracleCommand.Parameters.Add("P_SEARCH_FIELD_VALUE", OracleDbType.Varchar2).Value = model.SEARCH_FIELD_VALUE;

                            oracleCommand.ExecuteNonQuery();
                        }
                    }
                    _logHelper.LogAction((int)userId, "EditCategoryValues", $"admin {userId} edit EditCategoryValues", 1);
                    
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logHelper.LogAction((int)userId, "Failed EditCategoryValues", $"admin {userId} can not edit EditCategoryValues", 2);
                    
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult UpdateCategory([FromBody] Category model) // метод для изменения req
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
                            oracleCommand.CommandText = "UPDATE_CATEGORY";

                            // Параметры процедуры
                            oracleCommand.Parameters.Add("P_CATEGORY_ID", OracleDbType.Int32).Value = model.CategoryId;
                            oracleCommand.Parameters.Add("P_CATEGORY_NAME", OracleDbType.Varchar2).Value = model.CategoryName;
                            oracleCommand.Parameters.Add("P_CATEGORY_ICON", OracleDbType.Varchar2).Value = model.CategoryIcon;
                            oracleCommand.Parameters.Add("P_PROCEDURE_NAME", OracleDbType.Varchar2).Value = model.ProcedureName;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.Description;

                            oracleCommand.ExecuteNonQuery();
                            // model.CATEGORY_NAME = username; //TODO: Delete this
                        }
                    }
                    _logHelper.LogAction((int)userId, "EditCategory", $"admin {userId} edit Category", 1);
                    // Успешное обновление
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logHelper.LogAction((int)userId, "Failed EditCategory", $"admin {userId} can not edit Category", 2);
                    // Логируем ошибку (по возможности используйте более продвинутое логирование)
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return View(model);
        }

        public IActionResult BlockUser(int BlockId) // метод для блокирования пользователя
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
                        oracleCommand.CommandText = "BLOCK_USER";
                        oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = BlockId;

                        oracleCommand.ExecuteNonQuery();
                    }
                }
                _logHelper.LogAction((int)userId, "UserBlock", $"admin {userId} block a user", 1);
                // Редирект на страницу, например, список пользователей
                return RedirectToAction("AllUser"); // Или на другую страницу
            }
            catch (Exception ex)
            {
                _logHelper.LogAction((int)userId, "Failed UserBlock", $"admin {userId} block can not get", 2);
                // В случае ошибки можно вернуть представление с ошибкой или другую логику
                return View("Error"); // Показываем страницу с ошибкой
            }
        }
        [HttpGet]

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


        [HttpPost]
        public IActionResult CreateUser(UserModel model) // метод для доьавления пользователя
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
                            oracleCommand.CommandText = "NEW_USER";

                            oracleCommand.Parameters.Add("P_ROLE_ID", OracleDbType.Varchar2).Value =1;
                            oracleCommand.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = model.Username;
                            oracleCommand.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2).Value = model.FirstName;
                            oracleCommand.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2).Value = model.LastName;
                            oracleCommand.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = model.Email;
                            oracleCommand.Parameters.Add("P_PHONE", OracleDbType.Varchar2).Value = model.Phone;
                            oracleCommand.Parameters.Add("p_password", OracleDbType.Varchar2).Value = PasswordHelper.HashPassword(model.Password); 

                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    _logHelper.LogAction((int)userId, "CreateUser", $"admin {userId} create a new user", 1);
                    return Json(new { success = true, message = "User created successfully" });
                }
                catch (Exception ex)
                {
                    _logHelper.LogAction((int)userId, "Failed CreateUser", $"admin {userId} can not create a new user", 2);
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateCategory(Category model) // метод для создания req
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
                            oracleCommand.CommandText = "CREATE_CATEGORY";
                            oracleCommand.Parameters.Add("P_CATEGORY_NAME", OracleDbType.Varchar2).Value = model.CategoryName;
                            oracleCommand.Parameters.Add("P_CATEGORY_ICON", OracleDbType.Varchar2).Value = model.CategoryIcon;
                            oracleCommand.Parameters.Add("P_PROCEDURE_NAME", OracleDbType.Varchar2).Value = model.ProcedureName;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.Description;

                            oracleCommand.ExecuteNonQuery();

                        }
                    }
                    int? userId = HttpContext.Session.GetInt32("UserId");
                    var logHelper = new LogHelper(_config);
                    logHelper.LogAction(userId.Value, "Creating", $"user{userId} create a category", 1);
                    return Json(new { success = true, message = "Category created successfully" });

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateCategoryValues(CategoryVal model) // метод для создания req
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var logHelper = new LogHelper(_config);
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
                            oracleCommand.CommandText = "CREATE_CATEGORY_FIELD_VAL";
                            oracleCommand.Parameters.Add("P_FIELD_ID", OracleDbType.Varchar2).Value = model.FIELD_ID;
                            oracleCommand.Parameters.Add("P_DISPLAY_TEXT", OracleDbType.Varchar2).Value = model.DISPLAY_TEXT;
                            oracleCommand.Parameters.Add("P_FIELD_VALUE", OracleDbType.Varchar2).Value = model.FIELD_VALUE;

                            oracleCommand.ExecuteNonQuery();
                        }
                    }
                    
                    logHelper.LogAction(userId.Value, "Creating CategoryValues", $"user{userId} create a category value", 1);
                    return Json(new { success = true, message = "Category created successfully" });
                }
                catch (Exception ex)
                {
                    logHelper.LogAction(userId.Value, "Creating CategoryValues", $"user{userId} don't create a category value", 2);
                    return Json(new { success = false, message = ex.Message });
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateCategoryField(CategoryVal model) // метод для создания доп полей req
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
                            oracleCommand.CommandText = "CREATE_CATEGORY_FIELD";
                            oracleCommand.Parameters.Add("P_CATEGORY_ID", OracleDbType.Int32).Value = model.CATEGORY_ID;
                            oracleCommand.Parameters.Add("P_FIELD_TYPE_ID", OracleDbType.Int32).Value = model.FIELD_TYPE_ID;
                            oracleCommand.Parameters.Add("P_FIELD_NAME", OracleDbType.Varchar2).Value = model.FIELD_NAME;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;

                            
                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    int? userId = HttpContext.Session.GetInt32("UserId");
                    var logHelper = new LogHelper(_config);
                    logHelper.LogAction(userId.Value, "CreatingField", $"user{userId} create a category field", 1);
                    return Json(new { success = true, message = "Category Field created successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            // Если нужно отобразить сообщение об успешной вставке или ошибке, можно оставить текущий метод
            return View(model);
        }


        public IActionResult AddUser()
        {
            return View("Views/Admin/AddUser.cshtml");
        }

        // Обработка данных из формы и вставка в базу данных
        public IActionResult EditCategoryValues()
        {
            return View("Views/Admin/EditCategoryValues.cshtml");
        }

        public IActionResult Requests()
        {
            return View("Views/Admin/Requests.cshtml");
        }
        public IActionResult Users()
        {
            return View("Views/Admin/Users.cshtml");
        }

        public IActionResult CategoryValues()
        {
            return View("Views/Admin/CategoryValues.cshtml");
        }

        
        public IActionResult Categories()
        {
            return View("Views/Admin/Categories.cshtml");
        }
        public IActionResult EditCategories()
        {
            return View();
        }
        public IActionResult AddCategories()
        {
            return View("Views/Admin/AddCategories.cshtml");
        }
        public IActionResult AddCategoryField()
        {
            return View("Views/Admin/AddCategoryField.cshtml");
        }

        public IActionResult AddCategoryValues()
        {
            return View("Views/Admin/AddCategoryValues.cshtml");
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
                var categoryCursor = command.Parameters.Add("P_CATEGORIES", OracleDbType.RefCursor);
                categoryCursor.Direction = ParameterDirection.Output;

                // Параметры для пользователей
                var userCursor = command.Parameters.Add("P_USERS", OracleDbType.RefCursor);
                userCursor.Direction = ParameterDirection.Output;

                // Параметры для полей
                var fieldCursor = command.Parameters.Add("P_FIELDS", OracleDbType.RefCursor);
                fieldCursor.Direction = ParameterDirection.Output;

                // Параметры для типов полей
                var field_typeCursor = command.Parameters.Add("P_FIELD_TYPES", OracleDbType.RefCursor);
                field_typeCursor.Direction = ParameterDirection.Output;

                var requestCursor = command.Parameters.Add("P_REQUESTS", OracleDbType.RefCursor);
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
