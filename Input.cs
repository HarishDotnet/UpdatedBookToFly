using System.Text.Json;
using BookToFlyExceptions;
using ConsoleTextFormat;
namespace HomePage
{
    class Input
    {
        Validations validate = new Validations();
        static int bookingIdCounter = 1;
        public int getValidChoice(int start, int end)
        {
            Console.WriteLine($"\n{Fmt.fgGre}Enter your choice:{Fmt.fgWhi}");
            int choice;
            while (!Int32.TryParse(Console.ReadLine(), out choice) || choice < start || choice > end)
            {
                try { throw new InvalidChoice($"{Fmt.fgRed}Invalid choice. Please enter with in [{start}-{end}] :"); }
                catch (InvalidChoice e) { Console.WriteLine(e.Message); Console.WriteLine($"\n{Fmt.fgGre}Enter your choice Again:{Fmt.fgWhi}"); }
            }
            return choice;
        }
        public bool isContinuepage(string pageName)
        {
            bool result;
            Console.WriteLine($"\nDo You want to Continue {pageName} page? y or n : ");
            result = Console.ReadLine().ToLower().Equals("y") ? true : false;
            return result;
        }

        internal Flight getFlightInputs(AbstractFlightDetails FlightType)
        {
            Flight flight = new Flight();
            Console.WriteLine("Enter new Flight Name:");
            flight.FlightName = Console.ReadLine();
            Console.WriteLine("Enter new Flight Number:");
            flight.FlightNumber = Console.ReadLine();
            while (!getFlightDetails(flight.FlightNumber, FlightType).FlightNumber.Equals("notFound"))
            {
                Console.WriteLine("Flight number already Exist..! Try Different Flight Number:");
                flight.FlightNumber = Console.ReadLine();
            }
            Console.WriteLine("Enter new Flight Source:");
            flight.From = Console.ReadLine()!;
            Console.WriteLine("Enter new Flight Destination:");
            flight.To = Console.ReadLine()!;
            Console.WriteLine("Enter new Flight Time:");
            flight.Time = Console.ReadLine()!;
            Console.WriteLine("Enter new Flight Price:");
            flight.Price = Convert.ToInt32(Console.ReadLine()!);
            while (flight.SeatAvailability < 0)
            {
                Console.WriteLine("Price should be greater then 0");
                flight.Price = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("Enter new Flight SeatAvailability:");
            flight.SeatAvailability = Convert.ToInt32(Console.ReadLine()!);
            while (flight.SeatAvailability < 0)
            {
                Console.WriteLine("SeatAvailability should be greater then 0");
                flight.SeatAvailability = Convert.ToInt32(Console.ReadLine());
            }
            return flight;
        }
        public string getDate()
        {
            string date = Console.ReadLine();
            DateTime DateInput;
            while (!validate.ValidateDate(date, out DateInput))
            {
                Console.WriteLine("Enter valid date (dd/MM/yyyy)[02/12/2025]:");
                date = Console.ReadLine();
            }
            return DateInput.ToString().Substring(0, 10);
        }

        public byte getAge()
        {
            byte age;
            while (!byte.TryParse(Console.ReadLine(), out age) || age <= 0)
            {
                Console.WriteLine("Enter valid age: ");
            }
            return age;
        }
        static void PrintPasswordCriterias()
        {
            Console.WriteLine("Password Criterias:");
            Console.WriteLine("\t1. Password should not be Null.");
            Console.WriteLine("\t2. Password length should be greater then or equal to 8 and less then or equal to 15.");
            Console.WriteLine("\t3. Password should have Atleast 1 Uppercase.");
            Console.WriteLine("\t4. Password should have Atleast 1 LowerCase.");
            Console.WriteLine("\t5. Password should have Atleast 1 Special character.");
            Console.WriteLine("\t6. Password should have Atleast 1 numeric value.");
        }
        public string getValidPassword(string userName)
        {
            string newPassword, confirmPassword;
            PrintPasswordCriterias();
            Console.WriteLine("Enter Your new password (Example@123): ");
            newPassword = getMaskedPassword();
            while (validate.passwordValidation(userName, newPassword))
            {
                Console.WriteLine("Enter Your new password (Example@123): ");
                newPassword = getMaskedPassword();
            }
        retry_confirmPassword:
            Console.WriteLine("Confirm Password: ");
            confirmPassword = getMaskedPassword();
            while (!newPassword.Equals(confirmPassword))
            {
                Console.WriteLine("Password Not same as new password :( , Please try again!");
                Console.WriteLine("You want to Try again confirm password? Type : Yes or No");
                confirmPassword = Console.ReadLine();
                if (confirmPassword.ToLower().Equals("yes"))
                    goto retry_confirmPassword;
                else
                    return getValidPassword(userName);
            }
            return newPassword;
        }
        public string getMaskedPassword()
        {
            string password = string.Empty;
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true); // will return ConsoleKeyInfo object
                if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length != 0)
                    {
                        Console.Write("\b \b");
                        password = password.Substring(0, password.Length - 1);
                    }
                }
                else if (pressedKey.Key == ConsoleKey.Enter)
                    break;
                else
                {
                    password += pressedKey.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return password;
        }

        public String getUserName()
        {
        retry_FirstName:
            Console.WriteLine("Enter Your Firstname: ");
            string firstName = Console.ReadLine();
            if (!validate.ValidateName(firstName))
            {
                Console.WriteLine($"{Fmt.fgRed}Name should not have number or special characters and Lenght should be more then 3{Fmt.fgWhi}");
                goto retry_FirstName;
            }
            Console.WriteLine("Enter Your Lastname: ");
            string lastName = Console.ReadLine();

            /*  validation for last name
            if(!validate.ValidateName(lastName)){
                Console.WriteLine($"{Fmt.fgRed}Name should not have number or special characters and Lenght should be more then 3{Fmt.fgWhi}");
                goto retry_LastName;
            }
            */

            char[] symbols = { '@', '_', '.', '$', '&' };
            Random random = new Random();
            // Generate a random index to select a symbol
            int index = random.Next(symbols.Length);
            char randomSymbol = symbols[index];

            return firstName + randomSymbol + lastName;
        }
        public Flight getFlightDetails(string FlightNumber, AbstractFlightDetails FlightType)
        {
            List<Flight> flights = FlightType.flights;

            foreach (var flight in flights)
            {
                if (flight.FlightNumber.Equals(FlightNumber))
                {
                    // FlightType.flights.Remove(flight);
                    return flight;
                }
            }
            return new Flight { FlightNumber = "notFound" };
        }

        public string getName()
        {
            string name = Console.ReadLine();
            while (!validate.ValidateName(name))
            {
                Console.WriteLine("Name should not have number or special character and Lenght should be more then 3.\nEnter name again:");
                name = Console.ReadLine();
            }
            return name;
        }
        public string getFlightNumber(AbstractFlightDetails FlightType)
        {
            string FlightNumber = Console.ReadLine();
            Flight flight = getFlightDetails(FlightNumber, FlightType);
            while (flight.FlightNumber.Equals("notFound"))
            {
                Console.WriteLine($"{Fmt.fgRed}Flight Not Found..!,\nEnter valid FlightNumber:{Fmt.fgWhi}");
                FlightNumber = Console.ReadLine();
                flight = getFlightDetails(FlightNumber, FlightType);
            }
            return FlightNumber;
        }
        internal string getBookingid()
        {
            string bookingId;
            bookingId = "BTF0" + bookingIdCounter++;
            while (validate.isValidBookingId(bookingId))
            {
                bookingId = "BTF0" + bookingIdCounter++;
            }
            return bookingId;
        }


    }
}