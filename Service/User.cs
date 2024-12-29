using HomePage.Utils;
using HomePage.Model;
using ConsoleTextFormat;
using HomePage.Service.FlightBookingDB;
using Microsoft.Extensions.Logging;
using System;

namespace HomePage.Service
{
    internal class UserAuthentication : LoginAndSignupPage
    {
        private readonly UserOptions _option;
        private readonly FlightBookingConnection _flightBookingConnection;

        // Constructor requires logger, flight booking connection, and user options
        public UserAuthentication(ILogger<UserAuthentication> logger, FlightBookingConnection flightBookingConnection, UserOptions userOptions) 
            : base(logger, flightBookingConnection)  // Pass logger to base class
        {
            _option = userOptions ?? throw new ArgumentNullException(nameof(userOptions));
            _flightBookingConnection = flightBookingConnection;
        }

        // Main method to handle the user authentication process
        public void go()
        {
            _logger.LogInformation("UserAuthentication process started.");

            Input input = new Input();
            bool isUser = false;
            try
            {
                // Begin the login/signup process
                isUser = this.Begin();
                _logger.LogInformation("User authentication status: {IsUser}", isUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user authentication.");
                Console.WriteLine($"{Fmt.fgRed}An error occurred during authentication. Please try again.{Fmt.fgWhi}");
                return; // Stop further processing if an error occurs during authentication
            }

            // If the user is not authenticated, prompt to try again
            if (!isUser)
            {
                if (input.isContinuepage($"{Fmt.fgMag}User{Fmt.fgWhi}"))
                {
                    go(); // Recurse to retry the authentication
                }
                return;
            }

            bool doAgain;
            AbstractFlightDetails flightType;

            // Main menu loop for authenticated users
            do
            {
                doAgain = false;
                try
                {
                    // Present menu options for the user
                    Console.WriteLine("\n Enter Your Choice:");
                    Console.WriteLine(" 1. Show Flight Details");
                    Console.WriteLine(" 2. Search Flight");
                    Console.WriteLine(" 3. Book Ticket");
                    Console.WriteLine(" 4. Preview Ticket");
                    Console.WriteLine(" 5. Log out");

                    // Get the user's choice
                    int choice = input.getValidChoice(1, 5);
                    _logger.LogInformation("User selected option: {Choice}", choice);

                    // Handle each menu option
                    switch (choice)
                    {
                        case 1:
                            flightType = _option.SelectFlightType(); // Select flight type
                            _logger.LogInformation("User selected flight type: {FlightType}", flightType.GetType().Name);
                            _option.ShowFlightDetails(flightType); // Show flight details
                            break;

                        case 2:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                            flightType = _option.SelectFlightType(); // Select flight type
                            _logger.LogInformation("User initiated flight search for type: {FlightType}", flightType.GetType().Name);
                            _option.SearchFlight(flightType); // Search for flights
                            break;

                        case 3:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Book Ticket{Fmt.fgWhi}");
                            flightType = _option.SelectFlightType(); // Select flight type
                            _logger.LogInformation("User initiated booking for flight type: {FlightType}", flightType.GetType().Name);
                            _option.BookTicket(flightType); // Book a ticket
                            break;

                        case 4:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show Ticket{Fmt.fgWhi}");
                            Console.Write("Enter Booking ID to View: ");
                            string bookingId = Console.ReadLine();
                            _logger.LogInformation("User requested ticket preview for Booking ID: {BookingId}", bookingId);
                            _option.showTicket(bookingId); // Show ticket details
                            break;

                        case 5:
                            Console.WriteLine($"\n{Fmt.fgGre}User Page Logged out successfully...!{Fmt.fgWhi}");
                            _logger.LogInformation("User logged out successfully."); // Log logout event
                            return; // Exit the user authentication process

                        default:
                            _logger.LogWarning("Invalid choice selected by user: {Choice}", choice); // Log invalid choice
                            break;
                    }

                    // Ask the user if they want to continue in the menu
                    doAgain = input.isContinuepage($"{Fmt.fgMag}User Menu{Fmt.fgWhi}");
                    _logger.LogInformation("User chose to continue: {DoAgain}", doAgain);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing user input."); // Log any errors that occur during menu processing
                    Console.WriteLine($"{Fmt.fgRed}An error occurred. Please try again.{Fmt.fgWhi}");
                    doAgain = false; // Do not continue if there's an error
                }
            } while (doAgain); // Repeat the loop if the user wants to continue

            _logger.LogInformation("UserAuthentication process ended.");
        }
    }
}
