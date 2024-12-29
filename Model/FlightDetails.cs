using System.Text.Json;
using ConsoleTextFormat;

namespace HomePage.Model
{
    // Flight structure to store the details of a flight
    public struct Flight
    {
        public string FlightNumber { get; set; } 
        public string FlightName { get; set; }  
        public string From { get; set; }   
        public string To { get; set; }          
        public string Time { get; set; }        
        public int Price { get; set; }           
        public int SeatAvailability { get; set; } 

        // Overriding ToString() method to display flight details in a formatted manner
        public override string ToString()
        {
            return $"|  {Fmt.fgMag}{FlightNumber,-12}{Fmt.fgWhi}|    {FlightName,-15}|    {From,-15}|    {To,-15}|    {Time,-10}| {Price,8} | {SeatAvailability,5} |";
        }

        // Method to add a flight to the respective flight list (either local or international)
        public void addFlight(Flight flight, AbstractFlightDetails FlightType)
        {
            // Add the flight to the corresponding list of flights
            List<Flight> temp = FlightType.flights;
            temp.Add(flight); // Add the current flight to the list

            // Serialize the updated list of flights to JSON format with indentation
            string json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true });

            // Save the updated list to the appropriate JSON file based on the flight type (local or international)
            if (FlightType is LocalFlights)
                File.WriteAllText(@"Model/JSONFiles/LocalFlights.json", json);
            else
                File.WriteAllText(@"Model/JSONFiles/InternationalFlights.json", json);
        }
    }

    // Class to represent International Flights, inheriting from AbstractFlightDetails
    class InternationalFlights : AbstractFlightDetails
    {
        // Constructor to initialize the list of flights from the corresponding JSON file
        public InternationalFlights()
        {
            string filePath = @"Model/JSONFiles/InternationalFlights.json"; // Path to the JSON file
            string json = File.ReadAllText(filePath); // Read the JSON content
            this.flights = JsonSerializer.Deserialize<List<Flight>>(json); // Deserialize the JSON into a list of flights
        }
    }

    // Class to represent Local Flights, inheriting from AbstractFlightDetails
    class LocalFlights : AbstractFlightDetails
    {
        // Constructor to initialize the list of flights from the corresponding JSON file
        public LocalFlights()
        {
            string filePath = @"Model/JSONFiles/LocalFlights.json"; // Path to the JSON file
            string json = File.ReadAllText(filePath); // Read the JSON content
            this.flights = JsonSerializer.Deserialize<List<Flight>>(json); // Deserialize the JSON into a list of flights
        }
    }
}
