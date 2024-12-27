using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConsoleTextFormat;
using Serilog;
using HomePage.Utils;
using HomePage.Utils.Logging;
using HomePage.Service.FlightBookingDB;

namespace HomePage.Service
{
    public class Program
    {
        private static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            // Start the logger configuration and service provider
            var serviceProvider = StartLoggerConfiguration();

            // Resolve dependencies
            _logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var input = serviceProvider.GetRequiredService<Input>();
            var user = serviceProvider.GetRequiredService<UserAuthentication>();
            var admin = serviceProvider.GetRequiredService<AdminAuthentication>();

            _logger.LogInformation("Application Started");

            bool doagain = false;

            do
            {
                for (int i = 0; i < 100; i++) { Console.Write($"{Fmt.fgblu}-"); Thread.Sleep(5); }
                Console.WriteLine();
                Console.WriteLine($"\n\t\t{Fmt.fgCya}Welcome to the BookToFly Console Application{Fmt.fgWhi}"); Thread.Sleep(100);
                Console.WriteLine("Please select your profile by pressing (1/2/3):"); Thread.Sleep(100);
                Console.WriteLine("1. User"); Thread.Sleep(100);
                Console.WriteLine("2. Admin"); Thread.Sleep(100);
                Console.WriteLine("3. GuestMode"); Thread.Sleep(100);
                Console.WriteLine("4. Exit"); Thread.Sleep(100);

                int choice = input.getValidChoice(1, 4);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine($"\t\t\t{Fmt.fgGre}You have selected User Page{Fmt.fgWhi}");
                        user.go();
                        doagain = input.isContinuepage($"{Fmt.fgMag}Home{Fmt.fgWhi}");
                        break;
                    case 2:
                        Console.WriteLine($"\t\t\t{Fmt.fgGre}You have selected Admin Page{Fmt.fgWhi}");
                        admin.go();
                        doagain = input.isContinuepage($"{Fmt.fgMag}Home{Fmt.fgWhi}");
                        break;
                    case 3:
                        Console.WriteLine($"\t\t\t{Fmt.fgGre}You have selected Guest Page{Fmt.fgWhi}");
                        var guest = serviceProvider.GetRequiredService<Guest>(); // Resolving Guest from DI
                        doagain = input.isContinuepage($"{Fmt.fgMag}Home{Fmt.fgWhi}");
                        break;
                    case 4:
                        doagain = false;
                        break;
                }

            } while (doagain);

            Console.Write($"\n{Fmt.fgCya}Exiting from the application.\n{Fmt.fgGre}Loading Please Wait.");
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Console.Write($"{Fmt.fgGre}.");
            }
            Console.WriteLine($"\n\t\t\t\t{Fmt.fgGre}Thank you for Choosing BookToFly{Fmt.fgWhi}");
            _logger.LogInformation($"Application Ended.\n {new string('-', 100)}");
        }

        private static ServiceProvider StartLoggerConfiguration()
        {
            // Set up a service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the service provider
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Utils/Logging/log-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add logging services
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            // Register custom services
            services.AddTransient<Input>();
            services.AddTransient<Guest>(); // Ensure Guest is registered here
            services.AddTransient<FlightBookingConnection>(); 

            // Register the dependencies for AdminAuthentication and UserAuthentication
            services.AddTransient<UserAuthentication>(); // No need to manually instantiate
            services.AddTransient<AdminAuthentication>();

            // Register UserOptions service
            services.AddTransient<UserOptions>(); // Ensure UserOptions is added to the DI container
        }
    }
}
