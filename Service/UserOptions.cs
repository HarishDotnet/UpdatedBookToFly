using System.Text.Json;
using HomePage.Model;
using ConsoleTextFormat;
using HomePage.Utils.Logging;
using HomePage.Utils;
namespace HomePage.Service
{
    public class UserOptions : IUserOption
    {
        Input input;
        public UserOptions()
        {
            input = new Input();
        }

        public AbstractFlightDetails SelectFlightType()
        {
            Console.WriteLine("\n--- Select Flight Type ---");
            Console.WriteLine("1. Domestic Flights");
            Console.WriteLine("2. International Flights");
            int choice = new Input().getValidChoice(1, 2);
            if (choice == 1)
            {
                // Create an instance of LocalFlights
                Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Domestic Flight {Fmt.fgWhi}");
                AbstractFlightDetails localFlights = new LocalFlights();
                return localFlights;
            }
            else
            {
                // Create an instance of InternationalFlights
                Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}International Flight {Fmt.fgWhi}");
                AbstractFlightDetails internationalFlights = new InternationalFlights();
                return internationalFlights;
            }

        }

        //will display all the flight details 
        public void ShowFlightDetails(AbstractFlightDetails FlightType)
        {
            Console.WriteLine($"\n\t\t\t\t\t\t{Fmt.fgMag}--- Available Flights ---{Fmt.fgGre}\n");
            for (int i = 0; i < 110; i++) { Console.Write("-"); Thread.Sleep(1); };
            Console.WriteLine("\n|  Flight No   |    Flight Name    |        From        |        To        |     Time     | Price Rs | Seats |");
            for (int i = 0; i < 110; i++) { Console.Write("-"); Thread.Sleep(10); };
            Console.WriteLine(Fmt.fgWhi);
            foreach (var flight in FlightType.flights)
            {
                Console.WriteLine(flight);
                Console.WriteLine(new string('-', 110));
                Thread.Sleep(100);
            }
        }

        //will return true if the source and destination exists and as well as it prints the list of flights
        private bool GetFlight(string source, string destination, AbstractFlightDetails FlightType)
        {
            bool found = false;
            foreach (var flight in FlightType.flights)
            {
                string flightSource = flight.From.ToLower();
                string flightDestination = flight.To.ToLower();
                if (flightSource.Equals(source) && flightDestination.Equals(destination))
                {
                    Console.WriteLine(flight);
                    found = true;
                }
                Thread.Sleep(50);
            }
            if (!found)
                Console.WriteLine($"{Fmt.fgRed}{source} to {destination} Flight not Available. Sorry for inconvenience{Fmt.fgWhi}");
            return found;
        }
        public bool SearchFlight(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter source: ");
            string source = Console.ReadLine().ToLower();
            Console.Write("Enter destination: ");
            string destination = Console.ReadLine().ToLower();
            return this.GetFlight(source, destination, FlightType);
        }
        public string BookTicket(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter Passenger Name: ");
            string passengerName = input.getName();
            Console.Write("Enter Passenger Age: ");
            byte age = input.getAge();

            Console.WriteLine("Enter your date of journey (dd/MM/yyyy)[02/12/2025]:");
            string date = input.getDate();
            // bool doAgain = false;
            // do
            // {
            //     doAgain = false;
            //     if (this.SearchFlight(FlightType))
            //         break;
            //     Console.WriteLine($"{Fmt.fgYel}\nAre you want to try a different source and destination? (y or n){Fmt.fgWhi}");
            //     string opt = Console.ReadLine().ToLower();
            //     doAgain = opt.Equals("y") ? true : false;
            // } while (doAgain);
            ShowFlightDetails(FlightType);
            Console.WriteLine("Enter Flight Number that you want to travel:");
            string flightNumber = input.getFlightNumber(FlightType);

            Flight flight = input.getFlightDetails(flightNumber, FlightType);
            FlightType.flights.Remove(flight);
            Console.Write("Checking Seat Availability. Please wait");

            bool isAvailable = flight.SeatAvailability > 0;
            for (int i = 0; i < 3; i++) { Console.Write("."); Thread.Sleep(1000); }
            if (isAvailable)
            {
                flight.SeatAvailability -= 1;
                flight.addFlight(flight, FlightType);
            }
            else
            {
                flight.addFlight(flight, FlightType);
                Console.WriteLine("Seat not available. Please try later.");
                return null;
            }

            string bookingId = input.getBookingid();
            Console.WriteLine($"{Fmt.fgGre}Your ticket has been booked successfully.");
            Console.WriteLine($"\t\t\t\t{Fmt.b}Your Booking ID is: {bookingId}{Fmt._b}{Fmt.fgWhi}");
            Booking bookingDetails = new Booking(passengerName, age, date, flightNumber, bookingId);
            addBookingInfo(bookingId, bookingDetails);

            // Log the booking details
            string logMessage = $"Booking ID: {bookingId}, Passenger: {passengerName}, Age: {age}, Date: {date}, Flight Number: {flightNumber}";
            Logger.Log(logMessage);
            return bookingId;
        }
        public void addBookingInfo(string bookingId, Booking bookingInfo)
        {
            string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            Dictionary<string, Booking> temp = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);
            temp.Add(bookingId, bookingInfo);
            json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        public bool showTicket(string bookingId)
        {
            bool found = false;
            string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            // Deserialize the JSON into a Dictionary
            Dictionary<string, Booking> bookings;
            if (string.IsNullOrWhiteSpace(json))
                bookings = new Dictionary<string, Booking>(); // If empty file, use empty dictionary
            else
                bookings = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);

            // Ask user to input the Booking ID to search
            if (bookings != null && bookings.ContainsKey(bookingId))
            {
                Booking booking = bookings[bookingId];
                // Print the booking details
                Console.WriteLine($"\n\t\t{Fmt.fgYel}Ticket Details:");
                Console.WriteLine($"{Fmt.fgGre}Passenger Name:{Fmt.fgWhi}{Fmt.b} {booking.PassengerName}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Age: {Fmt.fgWhi}{Fmt.b}{booking.Age}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Date: {Fmt.fgWhi}{Fmt.b}{booking.Date}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Flight Number: {Fmt.fgWhi}{Fmt.b}{booking.FlightNumber}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Booking ID: {Fmt.fgWhi}{Fmt.b}{booking.BookingId}{Fmt._b}");
            }
            else
            {
                Console.WriteLine("\t\tTicket not available.");
            }
            return found;
        }
        public void ShowAllTickets()
        {
            try
            {
                string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
                // Read JSON file
                string json = File.ReadAllText(filePath);

                // Deserialize JSON into a dictionary
                var bookingDetails = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);

                // Iterate and print the dictionary content
                foreach (var booking in bookingDetails)
                {
                    Console.WriteLine($"{Fmt.fgblu}Booking ID: {booking.Key}{Fmt.fgWhi}");
                    Console.WriteLine($"\n\t\t{booking.Value}");
                    Console.WriteLine("------------------------------------------------------------------------------------------------- ");
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., file not found, invalid JSON)
                Console.WriteLine($"Error reading or parsing the file: {ex.Message}");
            }
        }
    }
}