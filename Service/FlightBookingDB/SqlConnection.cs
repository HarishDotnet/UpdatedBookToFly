using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace HomePage.Service.FlightBookingDB
{
    public class FlightBookingConnection
    {
        private string connectionString;
        SqlConnection connection;
        public FlightBookingConnection()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile(@"Model/JSONFiles/AppSettings.json", reloadOnChange: true, optional: false).Build();
                connectionString = configuration.GetConnectionString("Connection");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error...to get connection" + exception.Message);
            }
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        public void AddDetails(string username, string password, object authObject)
        {
            string tableName = authObject is AdminAuthentication ? "Admins" : "Users";
            string query = $"INSERT INTO {tableName} (Username, Password) VALUES (@Username, @Password)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.ExecuteNonQuery();
            }
        }
        public void DisplayUserName()
        {

            string query = "SELECT Username FROM Users"; // Adjust the table and column names as needed
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                Console.WriteLine(i++ + ". " + reader["Username"].ToString());
            }
        }
        public bool isDuplicate(string username, object authObject)
        {
            string tableName = authObject is AdminAuthentication ? "Admins" : "Users";
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE Username = @Username";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public Dictionary<string, string> getDataFromDB(object authObject)
        {
            Dictionary<string, string> userData = new Dictionary<string, string>();
            string tableName = authObject is AdminAuthentication ? "Admins" : "Users";

            string query = $"SELECT * FROM {tableName}";
            using (var command = new SqlCommand(query, connection))
            {

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string username = reader["username"].ToString();
                    string someOtherColumn = reader["password"].ToString();
                    userData.Add(username, someOtherColumn);
                }
                reader.Close();
                return userData;
            }
        }

        public bool CheckAuthentication(string username, string password, object authObject)
        {
            object authType = authObject is AdminAuthentication ? new AdminAuthentication() : new UserAuthentication();
            Dictionary<string, string> datas = this.getDataFromDB(authType);

            if (datas.ContainsKey(username) && datas[username].Equals(password))
            {
                return true;
            }
            return false;
        }
    }
}