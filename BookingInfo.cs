namespace HomePage
{
    public struct Booking
    {
        // Properties
        public string PassengerName { get; set; }
        public byte Age { get; set; }
        public string Date { get; set; }
        public string FlightNumber { get; set; }
        public string BookingId { get; set; }

        // Constructor to initialize the struct
        public Booking(string passengerName, byte age, string date, string flightNumber, string bookingId)
        {
            PassengerName = passengerName;
            Age = age;
            Date = date;
            FlightNumber = flightNumber;
            BookingId = bookingId;
        }

        // Override ToString() for display purposes
        public override string ToString()
        {
            return $"Booking ID: {BookingId}, Passenger: {PassengerName}, Age: {Age}, Date: {Date}, " +
                   $"Flight Number: {FlightNumber}";
        }
    }

}