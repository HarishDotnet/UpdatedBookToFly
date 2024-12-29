namespace HomePage.Model
{
    // Abstract class representing flight details, containing a list of flights
    public abstract class AbstractFlightDetails
    {
        // List of flights both domestic and international flight can use this common list
        internal List<Flight> flights;
    }
}
