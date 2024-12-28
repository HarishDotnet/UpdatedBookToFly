using System.Text.Json;
using HomePage.Model;
using ConsoleTextFormat;
using HomePage.Utils;
using Microsoft.Extensions.Logging;

namespace HomePage.Service
{
    public class UserOptions : IUserOption
    {
        private readonly ILogger<UserOptions> _logger;
        Input input=new Input();
       

        // public UserOptions(){}
        public UserOptions(ILogger<UserOptions> logger)
        {
           _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
        }

        public AbstractFlightDetails SelectFlightType()
        {
            Console.WriteLine("\n--- Select Flight Type ---");
            Console.WriteLine("1. Domestic Flights");
            Console.WriteLine("2. International Flights");
            int choice = input.getValidChoice(1, 2);
            if (choice == 1)
            {
                Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Domestic Flight {Fmt.fgWhi}");
                _logger.LogInformation("selected Domestic Flight.");
                return new LocalFlights();
            }
            else
            {
                Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}International Flight {Fmt.fgWhi}");
                _logger.LogInformation("selected International Flight.");
                return new InternationalFlights();
            }
        }

        public void ShowFlightDetails(AbstractFlightDetails FlightType)
        {
            Console.WriteLine($"\n\t\t\t\t\t\t{Fmt.fgMag}--- Available Flights ---{Fmt.fgGre}\n");
            for (int i = 0; i < 110; i++) { Console.Write("-"); Thread.Sleep(1); }
            Console.WriteLine("\n|  Flight No   |    Flight Name    |        From        |        To        |     Time     | Price Rs | Seats |");
            for (int i = 0; i < 110; i++) { Console.Write("-"); Thread.Sleep(10); }
            Console.WriteLine(Fmt.fgWhi);
            foreach (var flight in FlightType.flights)
            {
                Console.WriteLine(flight);
                Console.WriteLine(new string('-', 110));
                Thread.Sleep(100);
            }
        }

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
                    _logger.LogInformation($"Flight found: {flight.FlightNumber} from {flight.From} to {flight.To}");
                }
                Thread.Sleep(50);
            }
            if (!found)
            {
                _logger.LogWarning($"No flights available from {source} to {destination}."); 
                Console.WriteLine($"{Fmt.fgRed}{source} to {destination} Flight not Available. Sorry for inconvenience{Fmt.fgWhi}");
            }
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
                _logger.LogInformation($"Booking successful for flight {flightNumber}. Seats left: {flight.SeatAvailability}");
            }
            else
            {
                flight.addFlight(flight, FlightType);
                Console.WriteLine("Seat not available. Please try later.");
                _logger.LogWarning($"Seat unavailable for flight {flightNumber}. Booking failed.");
                return null;
            }

            string bookingId = input.getBookingid();
            Console.WriteLine($"{Fmt.fgGre}Your ticket has been booked successfully.");
            Console.WriteLine($"\t\t\t\t{Fmt.b}Your Booking ID is: {bookingId}{Fmt._b}{Fmt.fgWhi}");
            Booking bookingDetails = new Booking(passengerName, age, date, flightNumber, bookingId);
            addBookingInfo(bookingId, bookingDetails);

            // Log the booking details
            string logMessage = $"Booking ID: {bookingId}, Passenger: {passengerName}, Age: {age}, Date: {date}, Flight Number: {flightNumber}";
            _logger.LogInformation(logMessage);
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
            Dictionary<string, Booking> bookings = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);

            if (bookings != null && bookings.ContainsKey(bookingId))
            {
                Booking booking = bookings[bookingId];
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
                _logger.LogWarning($"No ticket found for Booking ID: {bookingId}.");
            }
            return found;
        }

        public void ShowAllTickets()
        {
            try
            {
                string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
                string json = File.ReadAllText(filePath);
                var bookingDetails = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);

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
                _logger.LogError($"Error reading or parsing the file: {ex.Message}");
                Console.WriteLine($"Error reading or parsing the file: {ex.Message}");
            }
        }
    }
}
