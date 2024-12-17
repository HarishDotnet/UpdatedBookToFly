// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Text.RegularExpressions;
// using System.Threading.Tasks;
// using Admin;

// namespace HomePage
// {
//     internal class Ticket
//     {
//         // User user;
//         UserAuthentication userAuthentication;
//         static int bookingIdCounter = 1;
//         public Ticket(UserAuthentication userAuthentication){
//             this.userAuthentication=userAuthentication;
//         }
//         public void GetPassengerDetails(){
//             Console.WriteLine("Enter passenger Name :");
//             string? name=Console.ReadLine();
//             Console.WriteLine("Enter passenger Age:");
//             byte age;
//             while(!Byte.TryParse(Console.ReadLine(),out age)){
//                 Console.WriteLine("Enter correct Passenger Age:");
//             }
//             Console.WriteLine("Enter Date of Travel(dd/mm/yyy):");
//             string? date=Console.ReadLine();
//             while(!isValidDate(date)){
//                 Console.WriteLine("Enter valid date formate Ex:(dd/mm/yyy):");
//                 date=Console.ReadLine();
//             }
//             Console.WriteLine("Enter Passanger State:");
//             string? state=Console.ReadLine();

//             Console.WriteLine("Enter State:");
//             string? state=Console.ReadLine();

//             bookingIdCounter++;
//         }
//         public bool isValidDate(string date){
//             DateTime dateValue;
//             bool isValid = DateTime.TryParseExact(
//                 date,
//                 "dd/MM/yyyy",
//                 CultureInfo.InvariantCulture,
//                 DateTimeStyles.None,
//                 out dateValue
//             );
//             return isValid;
//         }
//     }
// }