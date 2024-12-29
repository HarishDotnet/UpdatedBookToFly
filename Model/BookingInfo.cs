using HomePage.Model;
namespace HomePage
{
    public struct Booking
    {
        // Properties
        public string PassengerName { get; set; }
        public byte Age { get; set; }
        public string Date { get; set; }
        public Flight FlightDetails { get; set; }
        public string BookingId { get; set; }

        // Constructor to initialize the struct
        public Booking(string passengerName, byte age, string date, Flight flightDetails, string bookingId)
        {
            PassengerName = passengerName;
            Age = age;
            Date = date;
            FlightDetails = flightDetails;
            BookingId = bookingId;
        }

        // Override ToString() for display purposes
        public override string ToString()
        {
            return $"Booking ID: {BookingId}, Passenger: {PassengerName}, Age: {Age}, Date: {Date}";
        }
    }

}