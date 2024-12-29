using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HomePage.Service;
using System.Data;

namespace HomePage.Service.FlightBookingDB
{
    public class FlightBookingConnection
    {
        private string connectionString;
        private SqlConnection connection;
        private readonly ILogger<FlightBookingConnection> _logger;

        // Constructor with ILogger injection
        public FlightBookingConnection(ILogger<FlightBookingConnection> logger)
        {
            _logger = logger;
            try
            {
                var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile(@"Model/JSONFiles/AppSettings.json", reloadOnChange: true, optional: false)
                 .Build();

                connectionString = configuration.GetConnectionString("Connection");
                connection = new SqlConnection(connectionString);
                connection.Open();
                _logger.LogInformation("Database connection established successfully.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while establishing database connection.");
                Console.WriteLine("Error to get connection: " + exception.Message);
            }
        }
             
        public void AddDetails(string username, string password, object authObject)
        {
            string tableName = authObject is AdminAuthentication ? "Admins" : "Users";
            string query = "SetDetail";
            try
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                    _logger.LogInformation("User details added successfully for {Username}.", username);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding user details for {Username}.", username);
            }
        }

        public void DisplayUserName()
        {
            string query = "GetUserNames"; // Adjust the table and column names as needed
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType=CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                int i = 1;
                while (reader.Read())
                {
                    Console.WriteLine(i++ + ". " + reader["Username"].ToString());
                }
                reader.Close();
                _logger.LogInformation("Displayed user names.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while displaying user names.");
            }
        }

        public bool isDuplicate(string username, object authObject)
        {
            string tableName = authObject is AdminAuthentication ? "Admins" : "Users";
            string query = "GetCountOfUser";
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType=CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TableName",tableName);
                    command.Parameters.AddWithValue("@Username", username);
                    int count = (int)command.ExecuteScalar();
                    bool isDuplicate = count > 0;
                    _logger.LogInformation("Checked for duplicate username: {Username}, Found: {IsDuplicate}", username, isDuplicate);
                    return isDuplicate;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking for duplicate username: {Username}", username);
                return false;
            }
        }

        public Dictionary<string, string> getDataFromDB(string tableName)
        {
            Dictionary<string, string> userData = new Dictionary<string, string>();
            string query = "GetDetails";

            try
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType=CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TableName", tableName);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string username = reader["username"].ToString();
                        string password = reader["password"].ToString();
                        userData.Add(username, password);
                    }
                    reader.Close();
                    _logger.LogInformation("Fetched data from {TableName} table.", tableName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching data from {TableName} table.", tableName);
            }

            return userData;
        }

        public bool CheckAuthentication(string username, string password, object authObject)
        {
            string authType = authObject is AdminAuthentication ?"Admins" : "Users";
            Dictionary<string, string> datas = this.getDataFromDB(authType);

            bool isAuthenticated = datas.ContainsKey(username) && datas[username].Equals(password);
            _logger.LogInformation("Checked authentication for user: {Username}, Success: {IsAuthenticated}", username, isAuthenticated);
            return isAuthenticated;
        }
    }
}
