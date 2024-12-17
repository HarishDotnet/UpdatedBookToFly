using System.Text.Json;
using BookToFlyExceptions;
using System.Globalization;
namespace HomePage
{

    class Input
    {
        public int getValidChoice(int start, int end)
        {
            Console.WriteLine("\nEnter your choice:");
            int choice;
            while (!Int32.TryParse(Console.ReadLine(), out choice) || choice < start || choice > end)
            {
                try { throw new InvalidChoice($"Invalid choice. Please enter with in [{start}-{end}] :"); }
                catch (InvalidChoice message) { }
            }
            return choice;
        }
        public bool isContinuepage(string pageName)
        {
            bool result;
            Console.WriteLine($"\nDo You want to go to {pageName} page? Yes or no : ");
            result = Console.ReadLine().ToLower().Equals("yes") ? true : false;
            return result;
        }
        static bool ValidateDate(string date, out DateTime validDate)
        {
            return DateTime.TryParseExact(
                date,
                "dd/MM/yyyy",
                new CultureInfo("en-GB"),
                DateTimeStyles.None,
                out validDate
            );
        }
        public string getDate(){
            string date = Console.ReadLine();
            DateTime DateInput;
            while(!ValidateDate(date, out DateInput)){
                Console.WriteLine("Enter valid date (dd/MM/yyyy) :");
                date = Console.ReadLine();
            } 
            return DateInput.ToString();
        }

        public byte getAge(){
            byte age;
            while (!byte.TryParse(Console.ReadLine(), out age) || age<=0 )
            {    
                Console.WriteLine("Enter valid age: ");
            }
            return age;
        }


        public Flight getFlightDetails(string FlightNumber,AbstractFlightDetails FlightType){
            List<Flight> flights=FlightType.flights;
            
            foreach(var flight in flights){
                if(flight.FlightNumber.Equals(FlightNumber)){
                    return flight;
                }
            }
            return new Flight{FlightNumber="notFound"};
        }
        public string getFlightNumber(AbstractFlightDetails FlightType){
            string FlightNumber=Console.ReadLine();
            Flight flight=getFlightDetails(FlightNumber,FlightType);
            while(flight.FlightNumber.Equals("notFound")){
                Console.WriteLine("Flight Not Found..!,\nEnter valid FlightNumber:");
                FlightNumber=Console.ReadLine();
                flight=getFlightDetails(FlightNumber,FlightType);
            }
            return FlightNumber;
        }
    }
}