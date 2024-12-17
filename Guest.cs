using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage
{
    public class Guest
    {
        public Guest()
        {
            UserOptions? option = new UserOptions();
            Input input = new Input();
            bool doAgain;
            do
            {
                doAgain = false;
                Console.WriteLine("\n Options are:");
                Console.WriteLine(" 1. Show Flight Details");
                Console.WriteLine(" 2. Search Flight");
                int choice = input.getValidChoice(1, 2);
                AbstractFlightDetails? FlightType;
                switch (choice)
                {
                    case 1:
                        FlightType = option.SelectFlightType();
                        option.ShowFlightDetails(FlightType);
                        break;
                    case 2:
                        FlightType = option.SelectFlightType();
                        option.SearchFlight(FlightType);
                        break;
                }
                doAgain = input.isContinuepage("Guest");
            } while (doAgain);
        }
    }
}