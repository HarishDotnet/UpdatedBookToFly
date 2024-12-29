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
        Input input = new Input();
       
        // Constructor to inject the logger
        public UserOptions(ILogger<UserOptions> logger)
        {
           _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
        }

        // Method to allow the user to select a flight type (Domestic or International)
        public AbstractFlightDetails SelectFlightType()
        {
            Console.WriteLine("\n--- Select Flight Type ---");
            Console.WriteLine("1. Domestic Flights");
            Console.WriteLine("2. International Flights");
            int choice = input.getValidChoice(1, 2);

            // Handle user selection
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

        // Method to display available flight details
        public void ShowFlightDetails(AbstractFlightDetails FlightType)
        {
            Console.WriteLine($"\n\t\t\t\t\t\t{Fmt.fgMag}--- Available Flights ---{Fmt.fgGre}\n");
            for (int i = 0; i < 110; i++) { Console.Write("-"); Thread.Sleep(1); } // Printing a separator line
            Console.WriteLine("\n|  Flight No   |    Flight Name    |        From        |        To        |     Time     | Price Rs | Seats |");
            for (int i = 0; i < 110; i++) { Console.Write("-"); Thread.Sleep(10); } // Printing another separator line
            Console.WriteLine(Fmt.fgWhi); // Reset color formatting

            // Loop through flights and display their details
            foreach (var flight in FlightType.flights)
            {
                Console.WriteLine(flight); // Display flight info
                Console.WriteLine(new string('-', 110)); // Print a separator line
                Thread.Sleep(100); // Delay for a smooth user experience
            }
        }

        // Method to find a flight based on user input (source and destination)
        private bool GetFlight(string source, string destination, AbstractFlightDetails FlightType)
        {
            bool found = false;
            foreach (var flight in FlightType.flights)
            {
                string flightSource = flight.From.ToLower();
                string flightDestination = flight.To.ToLower();
                // If a matching flight is found, display it
                if (flightSource.Equals(source) && flightDestination.Equals(destination))
                {
                    Console.WriteLine(flight);
                    found = true;
                    _logger.LogInformation($"Flight found: {flight.FlightNumber} from {flight.From} to {flight.To}");
                }
                Thread.Sleep(50); // Delay between each flight check
            }
            if (!found)
            {
                _logger.LogWarning($"No flights available from {source} to {destination}."); 
                Console.WriteLine($"{Fmt.fgRed}{source} to {destination} Flight not Available. Sorry for inconvenience{Fmt.fgWhi}");
            }
            return found;
        }

        // Method to search for a flight by source and destination
        public bool SearchFlight(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter source: ");
            string source = Console.ReadLine().ToLower(); // Get source from user input
            Console.Write("Enter destination: ");
            string destination = Console.ReadLine().ToLower(); // Get destination from user input
            return this.GetFlight(source, destination, FlightType); // Call GetFlight to search for matching flights
        }

        // Method to book a ticket for a selected flight
        public string BookTicket(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter Passenger Name: ");
            string passengerName = input.getName(); // Get passenger's name
            Console.Write("Enter Passenger Age: ");
            byte age = input.getAge(); // Get passenger's age

            Console.WriteLine("Enter your date of journey (dd/MM/yyyy)[02/12/2025]:");
            string date = input.getDate(); // Get journey date

            ShowFlightDetails(FlightType); // Display available flight details
            Console.WriteLine("Enter Flight Number that you want to travel:");
            string flightNumber = input.getFlightNumber(FlightType); // Get flight number from user

            Flight flight = input.getFlightDetails(flightNumber, FlightType); // Get the flight details
            FlightType.flights.Remove(flight); // Remove the selected flight from the available list
            Console.Write("Checking Seat Availability. Please wait");

            bool isAvailable = flight.SeatAvailability > 0; // Check if seats are available
            for (int i = 0; i < 3; i++) { Console.Write("."); Thread.Sleep(1000); } // Simulate waiting time

            if (isAvailable)
            {
                flight.SeatAvailability -= 1; // Decrease the seat availability by 1
                flight.addFlight(flight, FlightType); // Update the flight details
                _logger.LogInformation($"Booking successful for flight {flightNumber}. Seats left: {flight.SeatAvailability}");
            }
            else
            {
                flight.addFlight(flight, FlightType); // Add the flight back if not available
                Console.WriteLine("Seat not available. Please try later.");
                _logger.LogWarning($"Seat unavailable for flight {flightNumber}. Booking failed.");
                return null;
            }

            string bookingId = input.getBookingid(); // Get the booking ID
            Console.WriteLine($"{Fmt.fgGre}Your ticket has been booked successfully.");
            Console.WriteLine($"\t\t\t\t{Fmt.b}Your Booking ID is: {bookingId}{Fmt._b}{Fmt.fgWhi}");
            Booking bookingDetails = new Booking(passengerName, age, date, flight, bookingId);
            addBookingInfo(bookingId, bookingDetails); // Save booking details

            // Log the booking details for auditing
            string logMessage = $"Booking ID: {bookingId}, Passenger: {passengerName}, Age: {age}, Date: {date}, Flight Number: {flightNumber}";
            _logger.LogInformation(logMessage);
            return bookingId; // Return the booking ID
        }

        // Method to add booking information to a JSON file
        public void addBookingInfo(string bookingId, Booking bookingInfo)
        {
            string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath); // Read the existing data
            Dictionary<string, Booking> temp = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json); // Deserialize to dictionary
            temp.Add(bookingId, bookingInfo); // Add the new booking information
            json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true }); // Serialize back to JSON
            File.WriteAllText(filePath, json); // Write to the file
        }

        // Method to show the ticket details for a specific booking ID
        public bool showTicket(string bookingId)
        {
            bool found = false;
            string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath); // Read the file
            Dictionary<string, Booking> bookings = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json); // Deserialize the file

            if (bookings != null && bookings.ContainsKey(bookingId)) // Check if booking exists
            {
                Booking booking = bookings[bookingId]; // Retrieve the booking
                Console.WriteLine($"\n\t\t{Fmt.fgYel}Ticket Details:");
                Console.WriteLine($"{Fmt.fgGre}Passenger Name:{Fmt.fgWhi}{Fmt.b} {booking.PassengerName}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Age: {Fmt.fgWhi}{Fmt.b}{booking.Age}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Date: {Fmt.fgWhi}{Fmt.b}{booking.Date}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Booking ID: {Fmt.fgWhi}{Fmt.b}{booking.BookingId}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgRed}Flight Details:");
                Console.WriteLine($"{Fmt.fgGre}Fight Name:{Fmt.fgWhi}{Fmt.b} {booking.FlightDetails.FlightName}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Fight Number: {Fmt.fgWhi}{Fmt.b}{booking.FlightDetails.FlightNumber}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Source: {Fmt.fgWhi}{Fmt.b}{booking.FlightDetails.From}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Destination: {Fmt.fgWhi}{Fmt.b}{booking.FlightDetails.To}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Depature Time: {Fmt.fgWhi}{Fmt.b}{booking.FlightDetails.Time}{Fmt._b}");
                Console.WriteLine($"{Fmt.fgGre}Ticket Price: {Fmt.fgWhi}{Fmt.b}{booking.FlightDetails.Price}{Fmt._b}");
            }
            else
            {
                Console.WriteLine("\t\tTicket not available.");
                _logger.LogWarning($"No ticket found for Booking ID: {bookingId}.");
            }
            return found;
        }

        // Method to show all tickets in the system
        public void ShowAllTickets()
        {
            try
            {
                string filePath = @"Model/JSONFiles/BookingDetails.json"; // Path to your JSON file
                string json = File.ReadAllText(filePath); // Read the JSON file
                var bookingDetails = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json); // Deserialize the data

                // Loop through all booking records and display their details
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
                _logger.LogError($"Error reading or parsing the file: {ex.Message}"); // Log any error
                Console.WriteLine($"Error reading or parsing the file: {ex.Message}"); // Display the error to the user
            }
        }
    }
}
