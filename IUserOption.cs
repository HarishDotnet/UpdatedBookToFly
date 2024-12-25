using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage
{
    public interface IUserOption 
    {
        public string BookTicket(AbstractFlightDetails FlightType);

        public AbstractFlightDetails SelectFlightType();
        public bool SearchFlight(AbstractFlightDetails FlightType);
        public void ShowFlightDetails(AbstractFlightDetails FlightType);

        public bool showTicket(string bookingId);
    }

}