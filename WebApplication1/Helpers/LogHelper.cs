using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace WebApplication1.Helpers
{
    public class LogHelper
    {
        private readonly string _connectionString;

        public LogHelper(IConfiguration config) //Конструктор
        {
            _connectionString = config.GetConnectionString("Db_Connection"); //переменная _connectionString для хранения строки подключения к бд
        }

        public void LogAction(int userId, string action, string actionData, int actionStatus)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                using (var command = new OracleCommand("LOG_ACTION", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_user_id", OracleDbType.Int32).Value = userId;
                    command.Parameters.Add("p_log_action", OracleDbType.Varchar2).Value = action;
                    command.Parameters.Add("p_action_data", OracleDbType.Varchar2).Value = actionData;
                    command.Parameters.Add("p_action_status", OracleDbType.Int32).Value = actionStatus;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
