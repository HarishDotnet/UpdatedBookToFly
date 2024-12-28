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
        // Constructor now requires the logger
        public UserAuthentication(ILogger<UserAuthentication> logger,FlightBookingConnection flightBookingConnection,UserOptions userOptions) 
            : base(logger,flightBookingConnection)  // Passing the logger to the base class
        {
            _option = userOptions ?? throw new ArgumentNullException(nameof(userOptions));
            _flightBookingConnection=flightBookingConnection;
        }

        public void go()
        {
            _logger.LogInformation("UserAuthentication process started.");

            Input input = new Input();
            bool isUser = false;
            try
            {
                isUser = this.Begin();
                _logger.LogInformation("User authentication status: {IsUser}", isUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user authentication.");
                Console.WriteLine($"{Fmt.fgRed}An error occurred during authentication. Please try again.{Fmt.fgWhi}");
                return;
            }

            if (!isUser)
            {
                if (input.isContinuepage($"{Fmt.fgMag}User{Fmt.fgWhi}"))
                {
                    go();
                }
                return;
            }

            bool doAgain;
            AbstractFlightDetails flightType;

            do
            {
                doAgain = false;
                try
                {
                    Console.WriteLine("\n Enter Your Choice:");
                    Console.WriteLine(" 1. Show Flight Details");
                    Console.WriteLine(" 2. Search Flight");
                    Console.WriteLine(" 3. Book Ticket");
                    Console.WriteLine(" 4. Preview Ticket");
                    Console.WriteLine(" 5. Log out");

                    int choice = input.getValidChoice(1, 5);
                    _logger.LogInformation("User selected option: {Choice}", choice);

                    switch (choice)
                    {
                        case 1:
                            flightType = _option.SelectFlightType();
                            _logger.LogInformation("User selected flight type: {FlightType}", flightType.GetType().Name);
                            _option.ShowFlightDetails(flightType);
                            break;

                        case 2:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                            flightType = _option.SelectFlightType();
                            _logger.LogInformation("User initiated flight search for type: {FlightType}", flightType.GetType().Name);
                            _option.SearchFlight(flightType);
                            break;

                        case 3:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Book Ticket{Fmt.fgWhi}");
                            flightType = _option.SelectFlightType();
                            _logger.LogInformation("User initiated booking for flight type: {FlightType}", flightType.GetType().Name);
                            _option.BookTicket(flightType);
                            break;

                        case 4:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show Ticket{Fmt.fgWhi}");
                            Console.Write("Enter Booking ID to View: ");
                            string bookingId = Console.ReadLine();
                            _logger.LogInformation("User requested ticket preview for Booking ID: {BookingId}", bookingId);
                            _option.showTicket(bookingId);
                            break;

                        case 5:
                            Console.WriteLine($"\n{Fmt.fgGre}User Page Logged out successfully...!{Fmt.fgWhi}");
                            _logger.LogInformation("User logged out successfully.");
                            return;

                        default:
                            _logger.LogWarning("Invalid choice selected by user: {Choice}", choice);
                            break;
                    }

                    doAgain = input.isContinuepage($"{Fmt.fgMag}User Menu{Fmt.fgWhi}");
                    _logger.LogInformation("User chose to continue: {DoAgain}", doAgain);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing user input.");
                    Console.WriteLine($"{Fmt.fgRed}An error occurred. Please try again.{Fmt.fgWhi}");
                    doAgain = false;
                }
            } while (doAgain);

            _logger.LogInformation("UserAuthentication process ended.");
        }
    }
}
