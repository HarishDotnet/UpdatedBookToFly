using Authenticator;
using HomePage;
namespace Admin
{
    public struct User
    {
        public string username;
        public string password;
        public User(string name, string pass)
        {
            username = name;
            password = pass;
        }
    }
    class AdminAuthentication : LoginAndSignupPage
    {
        public AdminAuthentication() : base("Admin")
        {
            // Console.WriteLine("Admin Page selected.");
        }

        public void go()
        {
            bool isAdmin = this.Begin();
            Input input = new Input();
            if (isAdmin)
            {
                Console.WriteLine("\nI am in devolpment stage come back later... :)");
            }
            else
            {
                Console.WriteLine("Do you want to continue to SignUp Page? yes : no");
                if (input.isContinuepage("Admin Profile"))
                    this.go();
            }
        }

    }
}

// for adding Flight Details

            // List<Flight> temp=this.flights;
            // temp.Add(new Flight{FlightNumber="LF111" ,FlightName="HarishFly",From = "Erode", To = "Rameshwaram", Time = "8:45 PM", Price = 1000 });
            // string json = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            // File.WriteAllText("LocalFlights.json",json);