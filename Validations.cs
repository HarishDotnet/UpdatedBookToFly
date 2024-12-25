using ConsoleTextFormat;
using BookToFlyExceptions;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text.Json;
namespace HomePage
{
    public class Validations
    {
        public bool ValidateName(string name){
            Regex regex=new Regex(@"^(?!.*[0-9])^[a-zA-Z]*$");
            return regex.IsMatch(name)&&name.Length>=3;
        }

        public bool passwordValidation(string userName, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(newPassword))
                    throw new NullField($"{Fmt.bgRed}Password should not be Null{Fmt.bgWhi}");
                if (userName.Equals(newPassword))
                    throw new SameAsUserName($"{Fmt.fgRed}Password should be different from user name{Fmt.fgWhi}");
                if (newPassword.Length < 8 || newPassword.Length > 15)
                    throw new PasswordLength($"{Fmt.fgRed}Password length should be greater then or equal to 8 and less then or equal to 15{Fmt.fgWhi}");
                Regex test = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$");
                if (!test.IsMatch(newPassword))
                {
                    throw new PasswordConditions("\n\n");
                }
            }
            catch (NullField message)
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (SameAsUserName message)
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (PasswordLength message)
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (PasswordConditions message)
            {
                Console.Write($"{Fmt.fgRed}{message.Message}{Fmt.fgWhi}\n");
                Console.WriteLine($"{Fmt.fgRed}1. Atleast 1 Uppercase.\n2. Atleast 1 LowerCase\n3. Atleast 1 Special character\n4. Atleast 1 numeric value{Fmt.fgWhi}");
                return true;
            }
            return false;
        }
        public bool ValidateDate(string date, out DateTime validDate)
        {
            return DateTime.TryParseExact(
                date,
                "dd/MM/yyyy",
                new CultureInfo("en-GB"),
                DateTimeStyles.None,
                out validDate
            ) && validDate>DateTime.Now ;
        }
        public bool isValidBookingId(string bookingId)
        {
            string filePath = @"JsonFiles/BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            // Deserialize the JSON into a Dictionary
            Dictionary<string, Booking> bookings;
            if (string.IsNullOrWhiteSpace(json))
                bookings = new Dictionary<string, Booking>(); // If empty file, use empty dictionary
            else
                bookings = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);

            return bookings.ContainsKey(bookingId);

        }
    }
}