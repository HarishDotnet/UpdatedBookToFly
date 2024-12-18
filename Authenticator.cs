using System;
using System.Runtime.InteropServices;
using BookToFlyExceptions;
using HomePage;
using ConsoleTextFormat;
using System.Text.RegularExpressions;
namespace Authenticator
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

    public class LoginAndSignupPage
    {
        private List<User> userDetails = new List<User>();

        string Profile;
        public LoginAndSignupPage(string profile)
        {
            this.Profile = profile;
        }
        public bool Begin()
        {
            if (this.Profile.Equals("Admin"))
            {
                User user = new User("Admin", "Admin123");
                this.userDetails.Add(user);
                return this.Login();
            }
            
            Console.WriteLine($"\n\n\t\t\t-:BookToFly{Fmt.b} {Fmt.fgMag}{this.Profile} {Fmt._b}{Fmt.fgWhi}Authentication page:-");
            Console.WriteLine("\npress the following keys for action:");
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
        static string getMaskedPassword()
        {
            string password = string.Empty;
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true); // will return ConsoleKeyInfo object
                if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length != 0)
                    {
                        Console.Write("\b \b");
                        password = password.Substring(0, password.Length - 1);
                    }
                }
                else if (pressedKey.Key == ConsoleKey.Enter)
                    break;
                else
                {
                    password += pressedKey.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return password;
        }
        public bool passwordValidation(string userName, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(newPassword))
                    throw new NullField($"{Fmt.bgRed}Password should not be Null{Fmt.bgWhi}");
                if (userName.Equals(newPassword))
                    throw new SameAsUserName($"{Fmt.fgRed}Password should be different from user name{Fmt.fgWhi}");
                if (newPassword.Length < 8 || newPassword.Length > 15)
                    throw new PasswordLength($"{Fmt.fgRed}Password length should be greater then or equal to 8 and less then or equal to 15{Fmt.fgWhi}");
                Regex test = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$");
                if (!test.IsMatch(newPassword))
                {
                    throw new PasswordConditions("\n\n");
                }
            }
            catch (NullField message)
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (SameAsUserName message)
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (PasswordLength message)
            {
                Console.WriteLine(message.Message);
                return true;
            }
            catch (PasswordConditions message)
            {
                Console.Write($"{Fmt.fgRed}{message.Message}{Fmt.fgWhi}\n");
                Console.WriteLine($"{Fmt.fgRed}1. Atleast 1 Uppercase.\n2. Atleast 1 LowerCase\n3. Atleast 1 Special character\n4. Atleast 1 numeric value{Fmt.fgWhi}");
                return true;
            }
            return false;
        }
        private String getUserName()
        {
            Console.WriteLine("Enter Your Firstname: ");
            string? firstName = Console.ReadLine();
            Console.WriteLine("Enter Your Lastname: ");
            string? lastName = Console.ReadLine();
            return firstName + "." + lastName;
        }
        static void PrintPasswordCriterias()
        {
            Console.WriteLine("Password Criterias:");
            Console.WriteLine("\t1. Password should not be Null.");
            Console.WriteLine("\t2. Password length should be greater then or equal to 8 and less then or equal to 15.");
            Console.WriteLine("\t3. Password should have Atleast 1 Uppercase.");
            Console.WriteLine("\t4. Password should have Atleast 1 LowerCase.");
            Console.WriteLine("\t5. Password should have Atleast 1 Special character.");
            Console.WriteLine("\t6. Password should have Atleast 1 numeric value.");
        }
        private String getPassword(string userName)
        {
            string? newPassword, confirmPassword;
            PrintPasswordCriterias();
            Console.WriteLine("Enter Your new password: ");
            newPassword = getMaskedPassword();
            while (passwordValidation(userName, newPassword))
            {
                Console.WriteLine("Enter Your new password: ");
                newPassword = getMaskedPassword();
            }
        retry_confirmPassword:
            Console.WriteLine("Confirm Password: ");
            confirmPassword = getMaskedPassword();
            while (!newPassword.Equals(confirmPassword))
            {
                Console.WriteLine("Password Not same as new password :( , Please try again!");
                Console.WriteLine("You want to Try again confirm password? Type : Yes or No");
                confirmPassword = Console.ReadLine();
                if (confirmPassword.ToLower().Equals("yes"))
                    goto retry_confirmPassword;
                else
                {
                    Console.WriteLine("Enter Your new password: ");
                    newPassword = getMaskedPassword();
                    Console.WriteLine("Confirm Password: ");
                    confirmPassword = getMaskedPassword();
                }
            }
            return newPassword;
        }
        public bool SignUp()
        {
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly User SignUp Page :) {Fmt.fgWhi}{Fmt._b}");
            String userName = this.getUserName();
            string password = this.getPassword(userName);
            Console.WriteLine("Account Created successfull!\n");
            Thread.Sleep(1000);
            Console.WriteLine($"\n\t\t{Fmt.b}Your UserName is : {Fmt.fgGre}{userName}{Fmt.fgWhi}{Fmt._b}");
            Thread.Sleep(2000);
            User user = new User(userName, password);
            this.userDetails.Add(user);
            return this.Begin();
        }

        public bool ValidUser(string username, string password, User user)
        {
            bool isUser = false;
            isUser = user.username.Equals(username) && user.password.Equals(password);//Equals() is used to compare the content in the value
            return isUser;
        }
        public bool Login()
        {
            Console.WriteLine("Enter Your Username: ");
            string? username = Console.ReadLine();
            Console.WriteLine("Enter Your Password: ");
            string? password = getMaskedPassword();
            bool isUser = false;

            foreach (User user in this.userDetails)
            {
                isUser = this.ValidUser(username, password, user);
                if(isUser)
                {
                    Console.WriteLine($"\nWelcome ({user.username})");
                    return true;
                }
            }
            Console.WriteLine("\nIncorrect Credentials! \n Check your Username and Password and come back");
            if (new Input().isContinuepage("UserLogin"))
                return this.Login();

            return false;
        }
    }


}