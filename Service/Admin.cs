using HomePage.Utils;
using ConsoleTextFormat;
using System.Text.Json;
using HomePage.Model;
using HomePage.Service.FlightBookingDB;
using Microsoft.Extensions.Logging; // Ensure you include this namespace for ILogger

namespace HomePage.Service
{
    class AdminAuthentication : LoginAndSignupPage
    {
        private readonly Input input;
        private readonly UserOptions option;
        private readonly FlightBookingConnection _flightBookingConnection;
        private readonly ILogger<AdminAuthentication> _logger;  // Logger for the class

        // Constructor now takes ILogger<AdminAuthentication>, FlightBookingConnection, Input, and UserOptions
        public AdminAuthentication(ILogger<AdminAuthentication> logger, 
                                    FlightBookingConnection flightBookingConnection,
                                    Input input, 
                                    UserOptions option)
            : base(logger,flightBookingConnection) // Pass the logger to the base class constructor
        {
            _logger = logger;
            _flightBookingConnection = flightBookingConnection;
            this.input = input;
            this.option = option;
        }

        public void go()
        {
            _logger.LogInformation("AdminAuthentication process started.");
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly Admin Page :) {Fmt.fgWhi}{Fmt._b}");
            bool isAdmin = this.Begin();
            bool doAgain;
            AbstractFlightDetails FlightType;

            if (isAdmin)
            {
                do
                {
                    doAgain = false;
                    Console.WriteLine("\n Enter Your Choice:");
                    Console.WriteLine(" 1. Add Flight");
                    Console.WriteLine(" 2. Update Flight");
                    Console.WriteLine(" 3. Remove Flight");
                    Console.WriteLine(" 4. Show Flight");
                    Console.WriteLine(" 5. Show All Booked Tickets");
                    Console.WriteLine(" 6. Search Flight");
                    Console.WriteLine(" 7. Display User List");
                    Console.WriteLine(" 8. Log out");

                    int choice = input.getValidChoice(1, 8);
                    _logger.LogInformation("Admin selected option: {Choice}", choice);

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Add Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            Flight flight = input.getFlightInputs(FlightType);
                            Console.WriteLine("....Confirming admin....");
                            if (this.Login())
                            {
                                flight.addFlight(flight, FlightType);
                                Console.WriteLine($"\n\t\t\t{Fmt.fgGre}Flight added successfully...!{Fmt.fgWhi}");
                                _logger.LogInformation("Flight added successfully: \n{Flight}", flight);
                            }
                            else
                            {
                                Console.WriteLine($"{Fmt.fgRed}Sorry, you don't have access to Add Flight Details.{Fmt.fgWhi}");
                                _logger.LogWarning("Unauthorized access attempt to add flight.");
                            }
                            break;

                        case 2:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Update Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            this.updateFlightdetail(FlightType);
                            break;

                        case 3:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Remove Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            this.removeFlight(FlightType);
                            break;

                        case 4:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show Flights{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            option.ShowFlightDetails(FlightType);
                            break;

                        case 5:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show All Tickets{Fmt.fgWhi}");
                            option.ShowAllTickets();
                            break;

                        case 6:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            option.SearchFlight(FlightType);
                            break;

                        case 7:
                            Console.WriteLine($"\n{Fmt.fgGre}User List are:{Fmt.fgWhi}");
                            _flightBookingConnection.DisplayUserName();
                            break;

                        case 8:
                            Console.WriteLine($"\n{Fmt.fgGre}Admin Page Logged out successfully...!{Fmt.fgWhi}");
                            _logger.LogInformation("Admin logged out successfully.");
                            return;
                    }

                    doAgain = input.isContinuepage($"{Fmt.fgMag}Admin Menu{Fmt.fgWhi}");
                    _logger.LogInformation("Admin chose to continue: {DoAgain}", doAgain);

                } while (doAgain);
            }
            else
            {
                if (input.isContinuepage($"{Fmt.fgMag}Admin{Fmt.fgWhi}"))
                    this.go();
            }
            _logger.LogInformation("AdminAuthentication process ended.");
        }

        private void removeFlight(AbstractFlightDetails FlightType)
        {
            _logger.LogInformation("Admin initiated flight removal.");
            Console.WriteLine("Enter Flight Number To remove Flight: ");
            string flightNumber = input.getFlightNumber(FlightType);
            Flight flight = input.getFlightDetails(flightNumber, FlightType);
            Console.WriteLine("Are you sure you want to Remove this Flight: (y/n)");
            string key = Console.ReadLine();
            if (key.ToLower().Equals("y"))
            {
                Console.WriteLine("....Confirming admin....");
                if (this.Login())
                {
                    FlightType.flights!.Remove(flight);
                    string json = JsonSerializer.Serialize(FlightType.flights, new JsonSerializerOptions { WriteIndented = true });
                    if (FlightType is LocalFlights)
                        File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json);
                    else
                        File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json);
                    Console.WriteLine($"\n\t\t\t{Fmt.fgGre}Flight Detail Removed successfully...!{Fmt.fgWhi}");
                    _logger.LogInformation("Flight removed: {FlightNumber}", flightNumber);
                }
                else
                {
                    Console.WriteLine($"{Fmt.fgRed}Sorry, you don't have access to Remove Flight Details.{Fmt.fgWhi}");
                    _logger.LogWarning("Unauthorized access attempt to remove flight.");
                }
            }
        }

        private void updateFlightdetail(AbstractFlightDetails FlightType)
        {
            _logger.LogInformation("Admin initiated flight update.");
            Console.WriteLine("Enter Flight Number To Update Details: ");
            string flightNumber = input.getFlightNumber(FlightType);
            Flight flight = input.getFlightDetails(flightNumber, FlightType);
            FlightType.flights.Remove(flight);
            Console.WriteLine("Which detail you want to update :");
            Console.WriteLine("1.Flight Name");
            Console.WriteLine("2.Flight Number");
            Console.WriteLine("3.Source");
            Console.WriteLine("4.Destination");
            Console.WriteLine("5.Time");
            Console.WriteLine("6.Price");
            Console.WriteLine("7.Seat Availability");
            int choice = input.getValidChoice(1, 7);
            switch (choice)
            {
                case 1:
                    Console.WriteLine($"Previous Flight name: {flight.FlightName}");
                    Console.WriteLine("Enter new Flight Name:");
                    flight.FlightName = Console.ReadLine()!;
                    break;
                case 2:
                    Console.WriteLine($"Previous Flight Number: {flight.FlightNumber}");
                    Console.WriteLine("Enter new Flight Number:");
                    flight.FlightNumber = Console.ReadLine()!;
                    break;
                case 3:
                    Console.WriteLine($"Previous Flight Source: {flight.From}");
                    Console.WriteLine("Enter new Flight Source:");
                    flight.From = Console.ReadLine()!;
                    break;
                case 4:
                    Console.WriteLine($"Previous Flight Destination: {flight.To}");
                    Console.WriteLine("Enter new Flight Destination:");
                    flight.To = Console.ReadLine()!;
                    break;
                case 5:
                    Console.WriteLine($"Previous Flight Time: {flight.Time}");
                    Console.WriteLine("Enter new Flight Time:");
                    flight.Time = Console.ReadLine()!;
                    break;
                case 6:
                    Console.WriteLine($"Previous Flight Price: {flight.Price}");
                    Console.WriteLine("Enter new Flight Price:");
                    flight.Price = Convert.ToInt32(Console.ReadLine()!);
                    break;
                case 7:
                    Console.WriteLine($"Previous Flight SeatAvailability: {flight.SeatAvailability}");
                    Console.WriteLine("Enter new Flight SeatAvailability:");
                    flight.SeatAvailability = Convert.ToInt32(Console.ReadLine()!);
                    break;
            }
            Console.WriteLine("Are you sure you want to update this detail: (y/n)");
            var key = Console.ReadLine();
            if (key.ToLower().Equals("y"))
            {
                Console.WriteLine("....Confirming admin....");
                if (this.Login())
                {
                    FlightType.flights!.Add(flight);
                    string json = JsonSerializer.Serialize(FlightType.flights, new JsonSerializerOptions { WriteIndented = true });
                    if (FlightType is LocalFlights)
                        File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json);
                    else
                        File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json);
                    Console.WriteLine($"\n\t\t\t{Fmt.fgGre}Flight Details Updated successfully...!{Fmt.fgWhi}");
                    _logger.LogInformation("Flight updated: {FlightNumber}", flightNumber);
                }
                else
                {
                    Console.WriteLine("Sorry, you don't have access to update.");
                    _logger.LogWarning("Unauthorized access attempt to update flight.");
                }
            }
        }
    }
}
