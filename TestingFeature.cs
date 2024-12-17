using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BookToFlyExceptions;

namespace HomePage
{
    public class TestingFeature
    {
        
        static void Main(string [] agrs){
            UserOptions u=new UserOptions();
            AbstractFlightDetails ft=u.SelectFlightType();
            u.BookTicket(ft);
            
        //     try{
        //         throw new NullField("Confirm Password should not be Null");
        //     }
        //     catch(NullField message){
        //         Console.Write(message.Message);
        //     }
        //     while(true){
        //     string password=Console.ReadLine();;
        //      Regex test=new Regex(@"(?=.*[A-Z])(?=.*\W)(?=.*\d).{8,15}");
        //      if(test.IsMatch(password))
        //         Console.WriteLine("Valid password");
        //     else  Console.WriteLine("INValid password");
        //     }
            

        //     Console.WriteLine($"\nWelcome (hairsh)");
        //     // List <string> l1=new List<string>();

        //     /*
        //     IFlightDetails lf=new LocalFlights();
        //     lf.ShowFlightDetails();
        //     lf=new InternationalFlights();
        //     lf.ShowFlightDetails();
        //     */
        }
    }
}