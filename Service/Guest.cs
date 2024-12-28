using ConsoleTextFormat;
using HomePage.Utils;
using HomePage.Model;
using Microsoft.Extensions.Logging;

namespace HomePage.Service
{
    public class Guest
    {
        private readonly UserOptions _userOptions;
        Input input=new Input();

        public Guest(UserOptions userOptions)
        {
            _userOptions = userOptions ?? throw new ArgumentNullException(nameof(userOptions));
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly Guest Page :) {Fmt.fgWhi}{Fmt._b}");
        }

        public void Start()
        {
            bool doAgain;
            do
            {
                doAgain = false;
                Console.WriteLine("\n Options are:");
                Console.WriteLine(" 1. Show Flight Details");
                Console.WriteLine(" 2. Search Flight");
                int choice = input.getValidChoice(1, 2);
                AbstractFlightDetails FlightType;
                switch (choice)
                {
                    case 1:
                        FlightType = _userOptions.SelectFlightType();
                        _userOptions.ShowFlightDetails(FlightType);
                        break;
                    case 2:
                        Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                        FlightType = _userOptions.SelectFlightType();
                        _userOptions.SearchFlight(FlightType);
                        break;
                }
                doAgain = input.isContinuepage($"{Fmt.fgMag}Guest{Fmt.fgWhi}");
            } while (doAgain);
        }
    }
}
