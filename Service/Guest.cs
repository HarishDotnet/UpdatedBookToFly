using ConsoleTextFormat;
using HomePage.Utils;
using HomePage.Model;
namespace HomePage.Service
{
    public class Guest
    {
        public Guest()
        {
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly Guest Page :) {Fmt.fgWhi}{Fmt._b}");
            UserOptions option = new UserOptions();
            Input input = new Input();
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
                        FlightType = option.SelectFlightType();
                        option.ShowFlightDetails(FlightType);
                        break;
                    case 2:
                        Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                        FlightType = option.SelectFlightType();
                        option.SearchFlight(FlightType);
                        break;
                }
                doAgain = input.isContinuepage($"{Fmt.fgMag}Guest{Fmt.fgWhi}");
            } while (doAgain);
        }
    }
}