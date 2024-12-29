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
        
        // Constructor now takes ILogger<AdminAuthentication>, FlightBookingConnection, Input, and UserOptions
        public AdminAuthentication(ILogger<AdminAuthentication> logger, 
                                    FlightBookingConnection flightBookingConnection,
                                    Input input, 
                                    UserOptions option)
            : base(logger, flightBookingConnection) // Pass the logger to the base class constructor
        {
            _flightBookingConnection = flightBookingConnection;
            this.input = input;
            this.option = option;
        }

        // Main method to handle Admin Authentication and Admin functionalities
        public void go()
        {
            _logger.LogInformation("AdminAuthentication process started.");
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly Admin Page :) {Fmt.fgWhi}{Fmt._b}");
            bool isAdmin = this.Begin();  // Start the login process for the admin user
            bool doAgain;
            AbstractFlightDetails FlightType;

            // If the user is authenticated as admin, proceed with admin options
            if (isAdmin)
            {
                do
                {
                    doAgain = false;
                    // Admin Menu options
                    Console.WriteLine("\n Enter Your Choice:");
                    Console.WriteLine(" 1. Add Flight");
                    Console.WriteLine(" 2. Update Flight");
                    Console.WriteLine(" 3. Remove Flight");
                    Console.WriteLine(" 4. Show Flight");
                    Console.WriteLine(" 5. Show All Booked Tickets");
                    Console.WriteLine(" 6. Search Flight");
                    Console.WriteLine(" 7. Display User List");
                    Console.WriteLine(" 8. Log out");

                    // Capture the choice and log it
                    int choice = input.getValidChoice(1, 8);
                    _logger.LogInformation("Admin selected option: {Choice}", choice);

                    // Switch based on admin's selected option
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Add Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            Flight flight = input.getFlightInputs(FlightType);
                            Console.WriteLine("....Confirming admin....");

                            // Check if admin credentials are correct before adding the flight
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
                            this.updateFlightdetail(FlightType); // Call to update flight details
                            break;

                        case 3:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Remove Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            this.removeFlight(FlightType); // Call to remove flight details
                            break;

                        case 4:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show Flights{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            option.ShowFlightDetails(FlightType); // Display flight details
                            break;

                        case 5:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show All Tickets{Fmt.fgWhi}");
                            option.ShowAllTickets(); // Show all booked tickets
                            break;

                        case 6:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            option.SearchFlight(FlightType); // Search for flights based on user input
                            break;

                        case 7:
                            Console.WriteLine($"\n{Fmt.fgGre}User List are:{Fmt.fgWhi}");
                            _flightBookingConnection.DisplayUserName(); // Display the list of users
                            break;

                        case 8:
                            Console.WriteLine($"\n{Fmt.fgGre}Admin Page Logged out successfully...!{Fmt.fgWhi}");
                            _logger.LogInformation("Admin logged out successfully.");
                            return; // Logout admin and exit
                    }

                    // Option to continue or logout after performing an action
                    doAgain = input.isContinuepage($"{Fmt.fgMag}Admin Menu{Fmt.fgWhi}");
                    _logger.LogInformation("Admin chose to continue: {DoAgain}", doAgain);

                } while (doAgain); // Repeat the menu options if the admin wishes to continue
            }
            else
            {
                if (input.isContinuepage($"{Fmt.fgMag}Admin{Fmt.fgWhi}"))
                    this.go(); // If not authenticated, prompt again for admin credentials
            }
            _logger.LogInformation("AdminAuthentication process ended.");
        }

        // Method to remove a flight from the flight list
        private void removeFlight(AbstractFlightDetails FlightType)
        {
            _logger.LogInformation("Admin initiated flight removal.");
            Console.WriteLine("Enter Flight Number To remove Flight: ");
            string flightNumber = input.getFlightNumber(FlightType); // Get the flight number to remove
            Flight flight = input.getFlightDetails(flightNumber, FlightType); // Retrieve flight details

            Console.WriteLine("Are you sure you want to Remove this Flight: (y/n)");
            string key = Console.ReadLine();
            if (key.ToLower().Equals("y"))
            {
                Console.WriteLine("....Confirming admin....");
                // Check admin credentials before removing the flight
                if (this.Login())
                {
                    FlightType.flights!.Remove(flight); // Remove flight from the list
                    string json = JsonSerializer.Serialize(FlightType.flights, new JsonSerializerOptions { WriteIndented = true });
                    if (FlightType is LocalFlights)
                        File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json); // Update the JSON file
                    else
                        File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json); // Update the JSON file
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

        // Method to update flight details
        private void updateFlightdetail(AbstractFlightDetails FlightType)
        {
            _logger.LogInformation("Admin initiated flight update.");
            Console.WriteLine("Enter Flight Number To Update Details: ");
            string flightNumber = input.getFlightNumber(FlightType); // Get flight number for update
            Flight flight = input.getFlightDetails(flightNumber, FlightType); // Retrieve flight details
            FlightType.flights.Remove(flight); // Remove old flight details

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

            // Confirm and update the flight details
            Console.WriteLine("Are you sure you want to update this detail: (y/n)");
            var key = Console.ReadLine();
            if (key.ToLower().Equals("y"))
            {
                Console.WriteLine("....Confirming admin....");
                // Check admin credentials before saving the updated flight
                if (this.Login())
                {
                    FlightType.flights!.Add(flight); // Add updated flight back to the list
                    string json = JsonSerializer.Serialize(FlightType.flights, new JsonSerializerOptions { WriteIndented = true });
                    if (FlightType is LocalFlights)
                        File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json); // Update the LocalFlights JSON file
                    else
                        File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json); // Update the InternationalFlights JSON file
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
