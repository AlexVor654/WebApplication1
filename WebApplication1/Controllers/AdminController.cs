using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Diagnostics;
using WebApplication1.Models;





namespace WebApplication1.Controllers
{
   
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;  
        private string _connectionString;

        public AdminController(IConfiguration config) //Конструктор
        {
            _config = config; //переменная _config для с appsetting.json
            _connectionString = config.GetConnectionString("Db_Connection"); //переменная _connectionString для хранннения строки подключения к бд
        }

        

        public string AllRequest()  // метод для отображение всех req
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "ALL_REQUEST";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;
                

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

        public string AllCategory() // метод для отображение всех categories
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "ALL_CATEGORY";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;


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
        public string AllUsers() // метод для отображение всех categories
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "ALL_USERS";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;


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

        public string AllCategoryVal() // метод для отображение всех доп полей categories
        {
            try
            {
                var oracleConnection = new OracleConnection(_connectionString); //создаем обьект с переменной _connectionString
                oracleConnection.Open(); //открываем наше соеденение

                var oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                oracleCommand.CommandText = "ALL_CATEGORY_VAL";
                oracleCommand.Parameters.Add("RETURN", OracleDbType.Clob).Direction = ParameterDirection.ReturnValue;


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
        public IActionResult EditCategoryVal([FromBody] CategoryVal model) // метод для отображение изменение доп поля category
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

        [HttpPost]
        public IActionResult EditCategory([FromBody] Category model) // метод для изменения req
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
                            oracleCommand.CommandText = "UPDATE_CATEGORY";

                            // Параметры процедуры
                            oracleCommand.Parameters.Add("P_CATEGORY_ID", OracleDbType.Int32).Value = model.CATEGORY_ID;
                            oracleCommand.Parameters.Add("P_CATEGORY_NAME", OracleDbType.Varchar2).Value = model.CATEGORY_NAME;
                            oracleCommand.Parameters.Add("P_CATEGORY_ICON", OracleDbType.Varchar2).Value = model.CATEGORY_ICON;
                            oracleCommand.Parameters.Add("P_PROCEDURE_NAME", OracleDbType.Varchar2).Value = model.PROCEDURE_NAME;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;

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

        public IActionResult ProcedureBlockUser(int id) // метод для блокирования пользователя
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    oracleConnection.Open();

                    using (var oracleCommand = oracleConnection.CreateCommand())
                    {
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        oracleCommand.CommandText = "BLOCK_USER";
                        oracleCommand.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = id;

                        oracleCommand.ExecuteNonQuery();
                    }
                }

                // Редирект на страницу, например, список пользователей
                return RedirectToAction("AllRequests"); // Или на другую страницу
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // В случае ошибки можно вернуть представление с ошибкой или другую логику
                return View("Error"); // Показываем страницу с ошибкой
            }
        }
        [HttpGet] 
        
        [HttpPost]
        public IActionResult UserInsert(UserModel model) // метод для доьавления пользователя
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
                            oracleCommand.CommandText = "NEW_USER";

                            oracleCommand.Parameters.Add("p_username", OracleDbType.Varchar2).Value = model.RoleId;
                            oracleCommand.Parameters.Add("p_email", OracleDbType.Varchar2).Value = model.Username;
                            oracleCommand.Parameters.Add("p_password", OracleDbType.Varchar2).Value = model.FIRST_NAME;
                            oracleCommand.Parameters.Add("p_username", OracleDbType.Varchar2).Value = model.LAST_NAME;
                            oracleCommand.Parameters.Add("p_email", OracleDbType.Varchar2).Value = model.Email;
                            oracleCommand.Parameters.Add("p_password", OracleDbType.Varchar2).Value = model.PHONE;
                            oracleCommand.Parameters.Add("p_password", OracleDbType.Varchar2).Value = model.PASSWD;

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
                            oracleCommand.Parameters.Add("P_CATEGORY_NAME", OracleDbType.Varchar2).Value = model.CATEGORY_NAME;
                            oracleCommand.Parameters.Add("P_CATEGORY_ICON", OracleDbType.Varchar2).Value = model.CATEGORY_ICON;
                            oracleCommand.Parameters.Add("P_PROCEDURE_NAME", OracleDbType.Varchar2).Value = model.PROCEDURE_NAME;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;

                            foreach (OracleParameter param in oracleCommand.Parameters)
                            {
                                Debug.WriteLine($"Parameter Name: {param.ParameterName}, Value: {param.Value}");
                            }


                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    
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
                            oracleCommand.CommandText = "CREATE_REQ_VAL";
                            oracleCommand.Parameters.Add("P_CATEGORY_ID", OracleDbType.Int32).Value = model.CATEGORY_ID;
                            oracleCommand.Parameters.Add("P_FIELD_TYPE_ID", OracleDbType.Int32).Value = model.FIELD_TYPE_ID;
                            oracleCommand.Parameters.Add("P_FIELD_NAME", OracleDbType.Varchar2).Value = model.FIELD_NAME;
                            oracleCommand.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = model.DESCRIPTION;

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


        public IActionResult UserInsert()
        {
            return View();
        }

        // Обработка данных из формы и вставка в базу данных
        public IActionResult EditCategorieVal()
        {
            return View("Views/Admin/EditCategoriesVal.cshtml");
        }

        public IActionResult AllRequests()
        {
            return View("Views/Admin/AllRequests.cshtml");
        }
        public IActionResult AllUser()
        {
            return View("Views/Admin/AllUsers.cshtml");
        }

        public IActionResult AllCategoryVals()
        {
            return View("Views/Admin/AllCategoryVal.cshtml");
        }

        
        public IActionResult AllCategorys()
        {
            return View("Views/Admin/AllCategory.cshtml");
        }
        public IActionResult EditCategories()
        {
            return View();
        }
        public IActionResult CreateCategorie()
        {
            return View("Views/Admin/CreateCategories.cshtml");
        }

        public IActionResult CreatesCategorieField()
        {
            return View("Views/Admin/CreateCategoriesVal.cshtml");
        }
        public IActionResult Privacy() => View();

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
