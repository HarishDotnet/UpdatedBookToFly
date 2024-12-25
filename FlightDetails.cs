using System.Text.Json;
using ConsoleTextFormat;
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

        public override string ToString()
        {
            return $"|  {Fmt.fgMag}{FlightNumber,-12}{Fmt.fgWhi}|    {FlightName,-15}|    {From,-15}|    {To,-15}|    {Time,-10}| {Price,8} | {SeatAvailability,5} |";
        }
        public void addFlight(Flight flight,AbstractFlightDetails FlightType){
             List<Flight> temp=FlightType.flights!;
             temp.Add(flight);
             string json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true });
             if(FlightType is LocalFlights)
                File.WriteAllText(@"JSONFiles/LocalFlights.json",json);
            else
                File.WriteAllText(@"JSONFiles/InternationalFlights.json",json);
        }
    }
    class InternationalFlights : AbstractFlightDetails
    {
        public InternationalFlights()
        {
            // Create a list to store flight instances
            string filePath = @"JSONFiles/InternationalFlights.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            this.flights = JsonSerializer.Deserialize<List<Flight>>(json);
        }
       
    }
    class LocalFlights : AbstractFlightDetails
    {
        public LocalFlights()
        {
            string filePath = @"JSONFiles/LocalFlights.json"; // Path to your JSON file
            string json = File.ReadAllText(filePath);
            this.flights = JsonSerializer.Deserialize<List<Flight>>(json);
        }

    }

}