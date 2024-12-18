using Authenticator;
using HomePage;
using System.Text.Json;
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

           