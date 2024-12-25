/*
@title          BookToFly - Flight Booking Application
@author         Harish M
@createdOn      10-12-2024
@modifiedOn     [11-12-2024, 15-12-2024, 18-12-2024, 23-12-2024, 25-12-2024]
@reviewedBy     -
@reviewedOn     -
*/
using Admin;
using ConsoleTextFormat;
namespace HomePage
{
    public class HomePage
    {
        static void Main(string[] args)
        {
            bool doagain = false;
            UserAuthentication user = new UserAuthentication();
            AdminAuthentication admin = new AdminAuthentication();
            Input input = new Input();
            do
            {
                for(int i=0;i<100;i++){Console.Write($"{Fmt.fgblu}-");Thread.Sleep(10);}
                Console.WriteLine();
                Console.WriteLine($"\n\t\t{Fmt.fgCya}Welcome to the BookToFly Console Application{Fmt.fgWhi}");Thread.Sleep(100);
                Console.WriteLine("Please select your profile by pressing (1/2/3):");Thread.Sleep(100);
                Console.WriteLine("1. User");Thread.Sleep(100);
                Console.WriteLine("2. Admin");Thread.Sleep(100);
                Console.WriteLine("3. GuestMode");Thread.Sleep(100);
                Console.WriteLine("4. Exit");Thread.Sleep(100);
                int choice = input.getValidChoice(1, 4);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine($"\t\t\t{Fmt.fgGre}You have selected User Page{Fmt.fgWhi}");
                        user.go();
                        doagain = input.isContinuepage($"{Fmt.fgMag}Home{Fmt.fgWhi}");
                        break;
                    case 2:
                        Console.WriteLine($"\t\t\t{Fmt.fgGre}You have selected Admin Page{Fmt.fgWhi}");
                        admin.go();
                        doagain = input.isContinuepage($"{Fmt.fgMag}Home{Fmt.fgWhi}");
                        break;
                    case 3:
                        Console.WriteLine($"\t\t\t{Fmt.fgGre}You have selected Guest Page{Fmt.fgWhi}");
                        Guest guest = new Guest();
                        doagain = input.isContinuepage($"{Fmt.fgMag}Home{Fmt.fgWhi}");
                        break;
                    case 4:
                        doagain = false;
                        break;
                }

            } while (doagain);
            Console.Write($"\n{Fmt.fgCya}Exiting from the application.\n{Fmt.fgGre}Loading Please Wait.");
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Console.Write($"{Fmt.fgGre}.");
            }
            Console.WriteLine($"\n\t\t\t\t{Fmt.fgGre}Thankyou for Choosing BookToFly{Fmt.fgWhi}");
        }
    
    }
   
}
