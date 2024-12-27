namespace HomePage.Utils.Logging{
public static class Logger
    {
        private static readonly string logFilePath = @"Utils/Logging/BookingLogs.txt"; // Path to the log file
        public static void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("LogInformation:");
                    writer.WriteLine($"{DateTime.Now}: {message}");
                    writer.WriteLine("--------------------------------------------------------------");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging information: {ex.Message}");
            }
        }
    }
}