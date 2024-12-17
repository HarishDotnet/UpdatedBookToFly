using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage
{
    public interface IUserOption 
    {
        public IUserOption SelectFlightType(); 
        public void BookTicket(AbstractFlightDetails FlightType);

        public void SearchFlight(AbstractFlightDetails FlightType);
    }

}