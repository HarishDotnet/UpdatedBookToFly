using HomePage.Utils;
using ConsoleTextFormat;
using System.Text.Json;
using HomePage.Model;
using HomePage.Service.FlightBookingDB;
namespace HomePage.Service
{
    class AdminAuthentication : LoginAndSignupPage
    {
        Input input;
        UserOptions option = new UserOptions();
        FlightBookingConnection flightBookingConnection;
        public AdminAuthentication() 
        {
            flightBookingConnection=new FlightBookingConnection();
            input = new Input();
        }
        public void go()
        {
            Console.WriteLine($"\n\t\t{Fmt.b}{Fmt.fgGre}(: Welcome to BookToFly Admin Page :) {Fmt.fgWhi}{Fmt._b}");
            bool isAdmin = this.Begin();
            bool doAgain;
            AbstractFlightDetails FlightType;
            if (isAdmin)
            {
                do
                {
                    doAgain = false;
                    Console.WriteLine("\n Enter Your Choice:");
                    Console.WriteLine(" 1. Add Flight");
                    Console.WriteLine(" 2. Update Flight");
                    Console.WriteLine(" 3. Remove Flight");
                    Console.WriteLine(" 4. Show Flight");
                    Console.WriteLine(" 5. Show All Booked Tickets");
                    Console.WriteLine(" 6. Search Flight");
                    Console.WriteLine(" 7. Display User List");
                    Console.WriteLine(" 8. Log out");
                    int choice = input.getValidChoice(1, 8
                    );
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Add Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            Flight flight = input.getFlightInputs(FlightType);
                            Console.WriteLine("....Confirming admin....");
                            if (this.Login())
                            {
                                flight.addFlight(flight, FlightType);
                                Console.WriteLine($"\n\t\t\t{Fmt.fgGre}Flight added successfully...!{Fmt.fgWhi}");
                            }
                            else
                            {
                                Console.WriteLine($"{Fmt.fgRed}Sorry,you don't have access to Add Flight Details.{Fmt.fgWhi}");
                            }
                            break;
                        case 2:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Update Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            this.updateFlightdetail(FlightType);
                            break;
                        case 3:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Remove Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            this.removeFlight(FlightType);
                            break;
                        case 4:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show Flights{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            option.ShowFlightDetails(FlightType);
                            break;
                        case 5:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Show All Tickets{Fmt.fgWhi}");
                            option.ShowAllTickets();
                            break;
                        case 6:
                            Console.WriteLine($"\t\t\tYou have selected {Fmt.fgMag}Search Flight{Fmt.fgWhi}");
                            FlightType = option.SelectFlightType();
                            option.SearchFlight(FlightType);
                            break;
                        case 7:
                            Console.WriteLine($"\n{Fmt.fgGre}User List are:{Fmt.fgWhi}");
                            flightBookingConnection.DisplayUserName();
                            break;
                        case 8:
                            Console.WriteLine($"\n{Fmt.fgGre}Admin Page Logged out successfully...!{Fmt.fgWhi}");
                            return;
                    }
                    doAgain = input.isContinuepage($"{Fmt.fgMag}Admin Menu{Fmt.fgWhi}");
                } while (doAgain);

            }
            else
            {
                if (input.isContinuepage($"{Fmt.fgMag}Admin{Fmt.fgWhi}"))
                    this.go();
            }
        }

        private void removeFlight(AbstractFlightDetails FlightType)
        {
            Console.WriteLine("Enter Flight Number To remove Flight: ");
            string flightNumber = input.getFlightNumber(FlightType);
            Flight flight = input.getFlightDetails(flightNumber, FlightType);
            Console.WriteLine("Are you sure you want to Remove this Flight: (y/n)");
            string key = Console.ReadLine();
            if (key.ToLower().Equals("y"))
            {
                Console.WriteLine("....Confirming admin....");
                if (this.Login())
                {
                    FlightType.flights!.Remove(flight);
                    string json = JsonSerializer.Serialize(FlightType.flights, new JsonSerializerOptions { WriteIndented = true });
                    if (FlightType is LocalFlights)
                        File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json);
                    else
                        File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json);
                    Console.WriteLine($"\n\t\t\t{Fmt.fgGre}Flight Detail Removed successfully...!{Fmt.fgWhi}");
                }
                else
                {
                    Console.WriteLine($"{Fmt.fgRed}Sorry,you don't have access to Remove Flight Details.{Fmt.fgWhi}");
                }
            }

        }
        private void updateFlightdetail(AbstractFlightDetails FlightType)
        {
            Console.WriteLine("Enter Flight Number To Update Details: ");
            string flightNumber = input.getFlightNumber(FlightType);
            Flight flight = input.getFlightDetails(flightNumber, FlightType);
            FlightType.flights.Remove(flight);
            Console.WriteLine("Which detail you want to update :");
            Console.WriteLine("1.Flight Name");
            Console.WriteLine("2.Flight Number");
            Console.WriteLine("3.Source");
            Console.WriteLine("4.Destination");
            Console.WriteLine("5.Time");
            Console.WriteLine("6.Price");
            Console.WriteLine("7.Seat Availability");
            int choice = input.getValidChoice(1, 7);
            switch (choice)
            {
                case 1:
                    Console.WriteLine($"Previous Flight name: {flight.FlightName}");
                    Console.WriteLine("Enter new Flight Name:");
                    flight.FlightName = Console.ReadLine()!;
                    break;
                case 2:
                    Console.WriteLine($"Previous Flight Number: {flight.FlightNumber}");
                    Console.WriteLine("Enter new Flight Number:");
                    flight.FlightNumber = Console.ReadLine()!;
                    break;
                case 3:
                    Console.WriteLine($"Previous Flight Source: {flight.From}");
                    Console.WriteLine("Enter new Flight Source:");
                    flight.From = Console.ReadLine()!;
                    break;
                case 4:
                    Console.WriteLine($"Previous Flight Destination: {flight.To}");
                    Console.WriteLine("Enter new Flight Destination:");
                    flight.To = Console.ReadLine()!;
                    break;
                case 5:
                    Console.WriteLine($"Previous Flight Time: {flight.Time}");
                    Console.WriteLine("Enter new Flight Time:");
                    flight.Time = Console.ReadLine()!;
                    break;
                case 6:
                    Console.WriteLine($"Previous Flight Price: {flight.Price}");
                    Console.WriteLine("Enter new Flight Price:");
                    flight.Price = Convert.ToInt32(Console.ReadLine()!);
                    break;
                case 7:
                    Console.WriteLine($"Previous Flight SeatAvailability: {flight.SeatAvailability}");
                    Console.WriteLine("Enter new Flight SeatAvailability:");
                    flight.SeatAvailability = Convert.ToInt32(Console.ReadLine()!);
                    break;

            }
            Console.WriteLine("Are you sure you want to update this detail: (y/n)");
            var key = Console.ReadLine();
            if (key.ToLower().Equals("y"))
            {
                Console.WriteLine("....Confirming admin....");
                if (this.Login())
                {
                    FlightType.flights!.Add(flight);
                    string json = JsonSerializer.Serialize(FlightType.flights, new JsonSerializerOptions { WriteIndented = true });
                    if (FlightType is LocalFlights)
                        File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json);
                    else
                        File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json);
                    Console.WriteLine($"\n\t\t\t{Fmt.fgGre}Flight Details Updated successfully...!{Fmt.fgWhi}");
                }
                else
                {
                    Console.WriteLine("Sorry,you don't have access to update.");
                }
            }
        }

    }
}

