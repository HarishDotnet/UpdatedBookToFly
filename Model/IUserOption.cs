namespace HomePage.Model
{
    // Interface for user operations related to flight booking and ticket management
    public interface IUserOption 
    {
        // Method to book a ticket and return booking confirmation or ID
        public string BookTicket(AbstractFlightDetails FlightType);

        // Method to allow the user to select a flight type and return it
        public AbstractFlightDetails SelectFlightType();

        // Method to search for flights based on criteria, returns true if found
        public bool SearchFlight(AbstractFlightDetails FlightType);

        // Method to display flight details for the given flight type
        public void ShowFlightDetails(AbstractFlightDetails FlightType);

        // Method to display ticket details using the booking ID, returns true if found
        public bool showTicket(string bookingId);
    }
}
