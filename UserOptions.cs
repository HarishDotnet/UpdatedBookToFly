using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ConsoleTextFormat;

namespace HomePage
{
    public class UserOptions
    {
        static int bookingIdCounter = 1;
        Input input=new Input();
        public AbstractFlightDetails SelectFlightType()
        {
            Console.WriteLine("\n--- Select Flight Type ---");
            Console.WriteLine("1. Local Flights");
            Console.WriteLine("2. International Flights");
            int choice = new Input().getValidChoice(1, 2);
            if (choice == 1)
            {
                // Create an instance of LocalFlights
                AbstractFlightDetails localFlights = new LocalFlights();
                return localFlights;
                // localFlights.ShowFlightDetails();
            }
            else
            {
                // Create an instance of InternationalFlights
                AbstractFlightDetails internationalFlights = new InternationalFlights();
                return internationalFlights;
                // internationalFlights.ShowFlightDetails();
            }

        }

        public void ShowFlightDetails(AbstractFlightDetails FlightType)
        {
            // for adding Flight Details

            // List<Flight> temp=this.flights;
            // temp.Add(new Flight{FlightNumber="LF111" ,FlightName="HarishFly",From = "Erode", To = "Rameshwaram", Time = "8:45 PM", Price = 1000 });
            // string json = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            // File.WriteAllText("LocalFlights.json",json);


            Console.WriteLine("\n--- Available LocalFlights ---");
            foreach (var flight in FlightType.flights)
            {
                Console.WriteLine($"Flight: {flight.FlightNumber} | Name: {flight.FlightName} | From: {flight.From} | To: {flight.To} | Time: {flight.Time} | Price: {flight.Price} Rs. | Seats Available: {flight.SeatAvailability}");
                Thread.Sleep(100);
            }
        }
        private void GetFlight(string source, string destination, AbstractFlightDetails FlightType)
        {
            bool notFound = true;
            foreach (var flight in FlightType.flights)
            {
                string flightSource = flight.From.ToLower();
                string flightDestination = flight.To.ToLower();
                if (flightSource.Equals(source) && flightDestination.Equals(destination))
                {
                    Console.WriteLine($"Flight: {flight.FlightNumber} | Name: {flight.FlightName} | From: {flight.From} | To: {flight.To} | Time: {flight.Time} | Price: {flight.Price} Rs. | Seats Available: {flight.SeatAvailability}");
                    notFound = false;
                }
                Thread.Sleep(100);
            }
            if (notFound)
                Console.WriteLine($"{Fmt.fgRed}{source} to {destination} Flight not Available. Sorry for inconvenience{Fmt.fgRed}");
        }
        public void SearchFlight(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter source: ");
            string source = Console.ReadLine().ToLower();
            Console.Write("Enter destination: ");
            string destination = Console.ReadLine().ToLower();
            this.GetFlight(source, destination, FlightType);
        }

        public void BookTicket(AbstractFlightDetails FlightType)
        {
            Console.Write("Enter Passenger Name: ");
            string passengerName = Console.ReadLine();

            Console.Write("Enter Passenger Age: ");
            byte age=input.getAge();
            
            Console.WriteLine("Enter your date of journey (dd/MM/yyyy):");
            string date=input.getDate();

            Console.WriteLine("Enter Flight Number:");
            string flightNumber=input.getFlightNumber(FlightType);

            Flight flight=input.getFlightDetails(flightNumber,FlightType);
            Console.Write("Checking Seat Availablity Please wait");

            bool isAvailable=checkSeatAvailability(flight);
            //add thread later
            for(int i=0;i<4;i++){
                Console.Write(".");
                Thread.Sleep(1000);
            }
            if(isAvailable){

            }
            else{
                Console.WriteLine("Seat not Available Please Try Later...");
            }
            string bookingId="BTF0"+bookingIdCounter;
            Console.WriteLine("Your Ticket has been booked successfully!");
            // bookedTickets[bookingIdCounter] = $"Booking ID: {bookingIdCounter} | Passenger: {passengerName} | Type: Local Ticket";
            Console.WriteLine($"Your Booking ID is: {bookingId}");
        }

        
        public bool checkSeatAvailability(Flight flight){
                if(flight.SeatAvailability<=0){
                    return false;
                }
                    return true;
            }

    }
}