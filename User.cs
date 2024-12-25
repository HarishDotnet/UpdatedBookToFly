using Authenticator;
using ConsoleTextFormat;
namespace HomePage
{
    internal class UserAuthentication : LoginAndSignupPage
    {
        UserOptions option;
        public UserAuthentication()
        {
            option=new UserOptions();
        }
        public void go()
        {
            Input input = new Input();
            bool isUser = this.Begin();
            bool doAgain;
            AbstractFlightDetails FlightType;
            if (isUser)
            {
                do
                {
                    doAgain = false;
                    Console.WriteLine("\n Enter Your Choice:");
                    Console.WriteLine(" 1. Show Flight Details");
                    Console.WriteLine(" 2. Search Flight");
                    Console.WriteLine(" 3. BookTicket");
                    Console.WriteLine(" 4. Preview Ticket");
                    Console.WriteLine(" 5. Log out");
                    int choice = input.getValidChoice(1, 5);
                    switch (choice)
                    {
                        case 1:
                            FlightType=option.SelectFlightType();
                            option.ShowFlightDetails(FlightType);
                            break;
                        case 2:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                            FlightType=option.SelectFlightType();
                            option.SearchFlight(FlightType);
                            break;
                        case 3:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Book Ticket{Fmt.fgWhi}");
                            FlightType=option.SelectFlightType();
                            option.BookTicket(FlightType);
                            break;
                        case 4:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show Ticket{Fmt.fgWhi}");
                            Console.Write("Enter Booking ID to View: ");
                            string bookingId = Console.ReadLine();
                            option.showTicket(bookingId);
                            break;
                        case 5:
                            Console.WriteLine($"\n{Fmt.fgGre}User Page Logged out successfully...!{Fmt.fgWhi}");
                            return;
                    }
                    doAgain = input.isContinuepage($"{Fmt.fgMag}User Menu{Fmt.fgWhi}");
                } while (doAgain);

            }
            else
            {
                if (input.isContinuepage($"{Fmt.fgMag}User{Fmt.fgWhi}"))
                    this.go();
            }
        }

    }
}