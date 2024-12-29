using ConsoleTextFormat;
using Utils.BookToFlyExceptions;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text.Json;

namespace HomePage
{
    public class Validations
    {
        // Validates that the name does not contain numbers or special characters and is at least 3 characters long
        public bool ValidateName(string name)
        {
            Regex regex = new Regex(@"^(?!.*[0-9])^[a-zA-Z]*$");  // Regular expression to ensure name contains only letters
            return regex.IsMatch(name) && name.Length >= 3;  // Return true if name is valid
        }

        // Validates the new password entered by the user based on various criteria
        public bool passwordValidation(string userName, string newPassword)
        {
            try
            {
                // Check if the password is null or empty
                if (string.IsNullOrEmpty(newPassword))
                    throw new NullField($"{Fmt.bgRed}Password should not be Null{Fmt.bgWhi}");

                // Check if the password is the same as the username
                if (userName.Equals(newPassword))
                    throw new SameAsUserName($"{Fmt.fgRed}Password should be different from user name{Fmt.fgWhi}");

                // Check if the password length is within the valid range (8 to 15 characters)
                if (newPassword.Length < 8 || newPassword.Length > 15)
                    throw new PasswordLength($"{Fmt.fgRed}Password length should be greater than or equal to 8 and less than or equal to 15{Fmt.fgWhi}");

                // Check if the password contains at least one lowercase letter, one uppercase letter, one digit, and one special character
                Regex test = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$");
                if (!test.IsMatch(newPassword))
                {
                    throw new PasswordConditions("\n\n");
                }
            }
            catch (NullField message)  // Exception handling for null password
            {
                Console.WriteLine(message.Message);
                return true;  // Return true to indicate the validation failed
            }
            catch (SameAsUserName message)  // Exception handling for password being the same as username
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (PasswordLength message)  // Exception handling for incorrect password length
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (PasswordConditions message)  // Exception handling for failing password conditions
            {
                Console.Write($"{Fmt.fgRed}{message.Message}{Fmt.fgWhi}\n");
                Console.WriteLine($"{Fmt.fgRed}1. At least 1 Uppercase.\n2. At least 1 LowerCase\n3. At least 1 Special character\n4. At least 1 numeric value{Fmt.fgWhi}");
                return true;
            }
            return false;  // Return false if the password meets all criteria
        }

        // Validates the date input by checking if it matches the format "dd/MM/yyyy" and is in the future
        public bool ValidateDate(string date, out DateTime validDate)
        {
            // Try to parse the date in "dd/MM/yyyy" format using the British culture settings
            return DateTime.TryParseExact(
                date,
                "dd/MM/yyyy",  // Specify the date format
                new CultureInfo("en-GB"),  // Use British culture for date format (day/month/year)
                DateTimeStyles.None,
                out validDate
            ) && validDate > DateTime.Now;  // Ensure the date is in the future
        }

        // Validates if the booking ID already exists in the booking records (from a JSON file)
        public bool isValidBookingId(string bookingId)
        {
            string filePath = @"Model/JsonFiles/BookingDetails.json";  // Path to the JSON file containing booking data
            string json = File.ReadAllText(filePath);  // Read the JSON content

            // Deserialize the JSON into a Dictionary where the key is the booking ID
            Dictionary<string, Booking> bookings;
            if (string.IsNullOrWhiteSpace(json))
                bookings = new Dictionary<string, Booking>();  // If the file is empty, initialize an empty dictionary
            else
                bookings = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);  // Deserialize the JSON into a dictionary

            // Check if the provided booking ID already exists in the dictionary
            return bookings.ContainsKey(bookingId);
        }
    }
}
