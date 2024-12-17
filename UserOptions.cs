using System.Text.Json;
using ConsoleTextFormat;
using HomePage.FlightBooking;

namespace HomePage
{
    public class UserOptions
    {
        static int bookingIdCounter = 1;
        Input input = new Input();
        public AbstractFlightDetails SelectFlightType()
        {
            Console.WriteLine("\n--- Select Flight Type ---");
            Console.WriteLine("1. Local Flights");
            Console.WriteLine("2. International Flights");
            int choice = new Input().getValidChoice(1, 2);
            if (choice == 1)
            {
                // Create an instance of LocalFlights
                AbstractFlightDetails localFlights = new LocalFlights();
                return localFlights;
                // localFlights.ShowFlightDetails();
            }
            else
            {
                // Create an instance of InternationalFlights
                AbstractFlightDetails internationalFlights = new InternationalFlights();
                return internationalFlights;
                // internationalFlights.ShowFlightDetails();
            }

        }

        public void ShowFlightDetails(AbstractFlightDetails FlightType)
        {
            


            Console.WriteLine($"\n\t\t\t\t\t\t{Fmt.fgMag}--- Available Flights ---{Fmt.fgGre}\n");
            for(int i=0;i<110;i++){ Console.Write("-");Thread.Sleep(10);};
            Console.WriteLine("\n|  Flight No   |    Flight Name    |        From        |        To        |     Time     | Price Rs | Seats |");
            for(int i=0;i<110;i++){ Console.Write("-");Thread.Sleep(10);};
            Console.WriteLine(Fmt.fgWhi);
            foreach (var flight in FlightType.flights!)
            {
                Console.WriteLine(flight);
                Console.WriteLine(new string('-',110));
                Thread.Sleep(100);
            }
        }
        private bool GetFlight(string source, string destination, AbstractFlightDetails FlightType)
        {
            bool found = false;
            foreach (var flight in FlightType.flights!)
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
            string? source = Console.ReadLine()?.ToLower();
            Console.Write("Enter destination: ");
            string? destination = Console.ReadLine()?.ToLower();
            return this.GetFlight(source!, destination!, FlightType);
        }

        public void BookTicket(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter Passenger Name: ");
            string? passengerName = Console.ReadLine();

            Console.Write("Enter Passenger Age: ");
            byte age = input.getAge();

            Console.WriteLine("Enter your date of journey (dd/MM/yyyy):");
            string date = input.getDate();
            bool doAgain = false;
            do
            {
                doAgain = false;
                this.SearchFlight(FlightType);
                Console.WriteLine($"{Fmt.fgYel}\nAre You want to try diff source and destination? yes or no{Fmt.fgWhi}");
                string? opt=Console.ReadLine()?.ToLower();
                doAgain = opt.Equals("yes") ? true : false;
            } while (doAgain);
            Console.WriteLine("Enter Flight Number:");
            string flightNumber = input.getFlightNumber(FlightType);

            Flight flight = input.getFlightDetails(flightNumber, FlightType);
            Console.Write("Checking Seat Availablity Please wait");

            bool isAvailable = checkSeatAvailability(flight);
            //add thread later
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            if (isAvailable)
            {

            }
            else
            {
                Console.WriteLine("Seat not Available Please Try Later...");
            }
            string bookingId = "BTF0" + bookingIdCounter;
            Console.WriteLine($"{Fmt.fgGre}Your Ticket has been booked successfully!");
            // bookedTickets[bookingIdCounter] = $"Booking ID: {bookingIdCounter} | Passenger: {passengerName} | Type: Local Ticket";
            Console.WriteLine($"\t\t\t\t{Fmt.b}Your Booking ID is: {bookingId}{Fmt._b}{Fmt.fgWhi}");
            addBookingInfo(bookingId, new Booking(passengerName!, age, flightNumber, date, bookingId));
        }
        static private void addBookingInfo(string bookingId, Booking bookingInfo)
        {
            string filePath = "BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            Dictionary<string, Booking>? temp = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);
            temp!.Add(bookingId, bookingInfo);
            json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        internal void showTicket()
        {
            string filePath = "BookingDetails.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON into a Dictionary
            Dictionary<string, Booking>? bookings;
            if (string.IsNullOrWhiteSpace(json))
                bookings = new Dictionary<string, Booking>(); // If empty file, use empty dictionary
            else
                bookings = JsonSerializer.Deserialize<Dictionary<string, Booking>>(json);

            // Ask user to input the Booking ID to search
            Console.Write("Enter Booking ID to View: ");
            string? bookingId = Console.ReadLine();
            if (bookings != null && bookings.ContainsKey(bookingId!))
            {
                Booking booking = bookings[bookingId!];
                // Print the booking details
                Console.WriteLine("\nTicket Details:");
                Console.WriteLine($"Passenger Name: {booking.PassengerName}");
                Console.WriteLine($"Age: {booking.Age}");
                Console.WriteLine($"Date: {booking.Date}");
                Console.WriteLine($"Flight Number: {booking.FlightNumber}");
                Console.WriteLine($"Booking ID: {booking.BookingId}");
            }
            else
            {
                Console.WriteLine("Ticket not available.");
            }
        }
    
        public bool checkSeatAvailability(Flight flight)
        {
            if (flight.SeatAvailability <= 0)
            {
                return false;
            }
            return true;
        }

    }
}