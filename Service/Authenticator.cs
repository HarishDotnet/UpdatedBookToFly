using ConsoleTextFormat;
using Microsoft.Extensions.Logging;
using HomePage.Service.FlightBookingDB;
using HomePage.Utils;

namespace HomePage.Service
{
    public class LoginAndSignupPage
    {
        private Dictionary<string ,string > adminDetails = new Dictionary<string ,string >();
        private readonly FlightBookingConnection _flightBookingConnection;
        private readonly Input input = new Input();
        private string Profile;

        protected readonly ILogger<LoginAndSignupPage> _logger;

        // Constructor accepting the ILogger
        public LoginAndSignupPage(ILogger<LoginAndSignupPage> logger,FlightBookingConnection flightBookingConnection)
        {
            _logger = logger;
            Profile = this is AdminAuthentication ? "Admin" : "User";
            _flightBookingConnection=flightBookingConnection;
        }


        public bool Begin()
        {
            if (this is AdminAuthentication)
            {
                // if(!flightBookingConnection.isDuplicate("Admin",this))
                //     flightBookingConnection.AddDetails("Admin","Admin@123",this);
                return this.Login();
            }
            
            Console.WriteLine($"\n\t\t-:Welcome to BookToFly{Fmt.b} {Fmt.fgMag}{this.Profile} {Fmt._b}{Fmt.fgWhi}Authentication page:-");
            Console.WriteLine("\nPress the following keys for action:");
            Console.WriteLine("Key\tAction To Be Perform");
            Console.WriteLine(" 1 \t     SignUP Page");
            Console.WriteLine(" 2 \t     Login Page");
            int choice = new Input().getValidChoice(1, 2);
            switch (choice)
            {
                case 1:
                    return this.SignUp();
                case 2:
                    return this.Login();
            }
            return false;
        }
       
        public bool SignUp()
        {
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly User SignUp Page :) {Fmt.fgWhi}{Fmt._b}");
            Re_enter_Username:
            string userName = input.getUserName();
            if(_flightBookingConnection.isDuplicate(userName,this)){
                Console.WriteLine($"{Fmt.fgRed}Username : {Fmt.fgGre}{userName}{Fmt.fgRed} already exist try enter different firstname and lastname.{Fmt.fgWhi}");
                goto Re_enter_Username;
            }
            string password = input.getValidPassword(userName);
            Console.WriteLine("Account Created successfully!\n");
            Thread.Sleep(1000);
            Console.WriteLine($"\n\t\t{Fmt.b}Your UserName is : {Fmt.fgGre}{userName}{Fmt.fgWhi}{Fmt._b}");
            Thread.Sleep(3000);
            _flightBookingConnection.AddDetails(userName,password,this);
            return this.Login();
        }

        public bool Login()
        {
            Console.WriteLine($"\t\t\t{Fmt.fgGre}Login Page{Fmt.fgWhi}");
            Console.WriteLine("Enter Your Username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Enter Your Password: ");
            string password = input.getMaskedPassword();
            bool isUser = false;
            isUser = _flightBookingConnection.CheckAuthentication(username,password,this);
            if(isUser)
            {
                Console.WriteLine($"\n\t\t{Fmt.fgGre}Welcome ({username}){Fmt.fgWhi}");
                return true;
            }
            Console.WriteLine($"\n{Fmt.fgRed}Incorrect Credentials! \n Check your Username and Password{Fmt.fgWhi}");
            if (new Input().isContinuepage($"{Fmt.fgMag}Login{Fmt.fgWhi}"))
                return this.Login();

            return false;
        }
    }
}
