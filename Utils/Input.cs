using HomePage.Model;
using Utils.BookToFlyExceptions;
using ConsoleTextFormat;

namespace HomePage.Utils
{
    class Input
    {
        Validations validate = new Validations();
        static int bookingIdCounter = 1; // Counter for generating unique booking IDs

        // Method to get a valid choice within a specified range (start, end)
        public int getValidChoice(int start, int end)
        {
            Console.WriteLine($"\n{Fmt.fgGre}Enter your choice:{Fmt.fgWhi}");
            int choice;
            while (!Int32.TryParse(Console.ReadLine(), out choice) || choice < start || choice > end)
            {
                try 
                {
                    throw new InvalidChoice($"{Fmt.fgRed}Invalid choice. Please enter with in [{start}-{end}] :");
                }
                catch (InvalidChoice e) 
                { 
                    Console.WriteLine(e.Message); 
                    Console.WriteLine($"\n{Fmt.fgGre}Enter your choice Again:{Fmt.fgWhi}"); 
                }
            }
            return choice;
        }

        // Method to ask the user if they want to continue on the current page (yes or no)
        public bool isContinuepage(string pageName)
        {
            bool result;
            Console.WriteLine($"\nDo You want to Continue {pageName} page? y or n : ");
            result = Console.ReadLine().ToLower().Equals("y") ? true : false;
            return result;
        }

        // Method to collect all necessary flight information (inputs) from the user
        internal Flight getFlightInputs(AbstractFlightDetails FlightType)
        {
            Flight flight = new Flight();
            Console.WriteLine("Enter new Flight Name:");
            flight.FlightName = Console.ReadLine();
            Console.WriteLine("Enter new Flight Number:");
            flight.FlightNumber = Console.ReadLine();
            // Ensure the flight number is unique by checking against the existing flight list
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

            // Validate that price is greater than 0
            while (flight.Price < 0)
            {
                Console.WriteLine("Price should be greater than 0");
                flight.Price = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("Enter new Flight Seat Availability:");
            flight.SeatAvailability = Convert.ToInt32(Console.ReadLine()!);

            // Validate that seat availability is greater than 0
            while (flight.SeatAvailability < 0)
            {
                Console.WriteLine("SeatAvailability should be greater than 0");
                flight.SeatAvailability = Convert.ToInt32(Console.ReadLine());
            }

            return flight;
        }

        // Method to prompt the user to enter a valid date in the format dd/MM/yyyy
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

        // Method to get a valid age (must be a positive byte value)
        public byte getAge()
        {
            byte age;
            while (!byte.TryParse(Console.ReadLine(), out age) || age <= 0)
            {
                Console.WriteLine("Enter valid age: ");
            }
            return age;
        }

        // Static method to print password criteria to the console
        static void PrintPasswordCriterias()
        {
            Console.WriteLine("Password Criterias:");
            Console.WriteLine("\t1. Password should not be Null.");
            Console.WriteLine("\t2. Password length should be greater than or equal to 8 and less than or equal to 15.");
            Console.WriteLine("\t3. Password should have Atleast 1 Uppercase.");
            Console.WriteLine("\t4. Password should have Atleast 1 LowerCase.");
            Console.WriteLine("\t5. Password should have Atleast 1 Special character.");
            Console.WriteLine("\t6. Password should have Atleast 1 numeric value.");
        }

        // Method to get a valid password from the user, ensuring it meets all password criteria
        public string getValidPassword(string userName)
        {
            string newPassword, confirmPassword;
            PrintPasswordCriterias();  // Show password criteria to the user
            Console.WriteLine("Enter Your new password (Example@123): ");
            newPassword = getMaskedPassword(); // Get the masked password input

            // Keep prompting the user until the password meets all conditions
            while (validate.passwordValidation(userName, newPassword))
            {
                Console.WriteLine("Enter Your new password (Example@123): ");
                newPassword = getMaskedPassword();
            }

        retry_confirmPassword:
            Console.WriteLine("Confirm Password: ");
            confirmPassword = getMaskedPassword();
            // Ensure the passwords match
            while (!newPassword.Equals(confirmPassword))
            {
                Console.WriteLine("Password Not same as new password :( , Please try again!");
                Console.WriteLine("You want to Try again confirm password? Type : Yes or No");
                confirmPassword = Console.ReadLine();
                if (confirmPassword.ToLower().Equals("yes"))
                    goto retry_confirmPassword;
                else
                    return getValidPassword(userName);  // Retry if user doesn't confirm
            }

            return newPassword;
        }

        // Method to get a masked password (input is hidden while typing)
        public string getMaskedPassword()
        {
            string password = string.Empty;
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true); // Returns ConsoleKeyInfo object
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
                    password += pressedKey.KeyChar; // Append key char to password string
                    Console.Write("*");  // Mask the character with an asterisk (*)
                }
            }
            Console.WriteLine();
            return password;
        }

        // Method to get a valid username, combining first and last name with a random symbol
        public String getUserName()
        {
        retry_FirstName:
            Console.WriteLine("Enter Your Firstname: ");
            string firstName = Console.ReadLine();
            if (!validate.ValidateName(firstName))
            {
                Console.WriteLine($"{Fmt.fgRed}Name should not have number or special characters and Length should be more than 3{Fmt.fgWhi}");
                goto retry_FirstName;
            }

            Console.WriteLine("Enter Your Lastname: ");
            string lastName = Console.ReadLine();

            // Generate a random symbol to combine with the name
            char[] symbols = { '@', '_', '.', '$', '&' };
            Random random = new Random();
            int index = random.Next(symbols.Length);
            char randomSymbol = symbols[index];

            return firstName + randomSymbol + lastName;  // Return combined username
        }

        // Method to get the flight details based on the flight number, returns "notFound" if no flight matches
        public Flight getFlightDetails(string FlightNumber, AbstractFlightDetails FlightType)
        {
            List<Flight> flights = FlightType.flights;

            foreach (var flight in flights)
            {
                if (flight.FlightNumber.Equals(FlightNumber))
                {
                    return flight;  // Return the flight details if a match is found
                }
            }
            return new Flight { FlightNumber = "notFound" };  // Return a "not found" flight if no match
        }

        // Method to get a valid name input (checks for length and no special characters/numbers)
        public string getName()
        {
            string name = Console.ReadLine();
            while (!validate.ValidateName(name))
            {
                Console.WriteLine("Name should not have number or special character and Length should be more than 3.\nEnter name again:");
                name = Console.ReadLine();
            }
            return name;
        }

        // Method to get a valid flight number (checks if the flight number exists in the flight list)
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

        // Method to generate and return a unique booking ID
        internal string getBookingid()
        {
            string bookingId;
            bookingId = "BTF0" + bookingIdCounter++;  // Generate a new booking ID using the counter
            while (validate.isValidBookingId(bookingId))
            {
                bookingId = "BTF0" + bookingIdCounter++;  // Ensure the booking ID is valid
            }
            return bookingId;
        }
    }
}
