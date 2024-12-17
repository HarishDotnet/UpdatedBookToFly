using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
namespace HomePage
{
    public struct Flight
    {
        public string FlightNumber { get; set; }
        public string FlightName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Time { get; set; }
        public int Price { get; set; }
        public int SeatAvailability { get; set; }
    }

    internal class InternationalFlights : AbstractFlightDetails
    {
        public InternationalFlights()
        {
            // Create a list to store flight instances
            // this.flights = new List<Flight>();
            string filePath = "InternationalFlights.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            this.flights = JsonSerializer.Deserialize<List<Flight>>(json);

            // foreach (var flight in this.flights)
            // {
            //     Console.WriteLine($"{flight.FlightNumber}, {flight.FlightName}, {flight.From}, {flight.To}, {flight.Time}, {flight.Price}");
            // }
        }
       
    }


    class LocalFlights : AbstractFlightDetails
    {
        public LocalFlights()
        {
            string filePath = "LocalFlights.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            this.flights = JsonSerializer.Deserialize<List<Flight>>(json);
        }

        

    }

}