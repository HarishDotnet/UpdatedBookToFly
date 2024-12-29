using ConsoleTextFormat;
using HomePage.Utils;
using HomePage.Model;
using Microsoft.Extensions.Logging;

namespace HomePage.Service
{
    public class Guest
    {
        private readonly UserOptions _userOptions; // Reference to UserOptions to perform operations related to flight booking
        Input input = new Input(); // Input instance to handle user input

        // Constructor to initialize UserOptions and welcome the guest
        public Guest(UserOptions userOptions)
        {
            _userOptions = userOptions ?? throw new ArgumentNullException(nameof(userOptions)); // Ensure UserOptions is not null
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly Guest Page :) {Fmt.fgWhi}{Fmt._b}"); // Welcome message for the guest
        }

        // Method to start the guest interaction
        public void Start()
        {
            bool doAgain; // Flag to check if the guest wants to continue or not
            do
            {
                doAgain = false; // Reset flag at the start of each loop
                Console.WriteLine("\n Options are:"); // Display the available options to the guest
                Console.WriteLine(" 1. Show Flight Details");
                Console.WriteLine(" 2. Search Flight");
                
                // Get valid choice from the user (either option 1 or 2)
                int choice = input.getValidChoice(1, 2);
                AbstractFlightDetails FlightType; // Variable to hold flight type (domestic or international)
                
                // Handle the user's choice
                switch (choice)
                {
                    case 1: // Option 1 - Show Flight Details
                        FlightType = _userOptions.SelectFlightType(); // Select flight type (domestic or international)
                        _userOptions.ShowFlightDetails(FlightType); // Show the details of the selected flights
                        break;
                    case 2: // Option 2 - Search for a Flight
                        Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}"); // Indicate search option
                        FlightType = _userOptions.SelectFlightType(); // Select flight type
                        _userOptions.SearchFlight(FlightType); // Perform the flight search based on user input
                        break;
                }

                // Ask the user if they want to continue to another page
                doAgain = input.isContinuepage($"{Fmt.fgMag}Guest{Fmt.fgWhi}");
            } while (doAgain); // Continue if user chooses 'Yes'
        }
    }
}
