using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.ConsoleApp.Consoles;
using OrderService.ConsoleApp.Settings;

namespace OrderService.ConsoleApp
{
    class Program
    {
        private static IConfiguration Configuration = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile("appsettings.json").Build();

        static IHostBuilder CreateHostBuilder()
        {
            var appSettings = new AppSettings
            {
                OrderApiHost = Configuration.GetConnectionString("OrderApiHost") ?? throw new NotSupportedException(nameof(AppSettings.OrderApiHost))
            };

            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<OrderConsole, OrderConsole>();
                    services.AddSingleton(appSettings);

                    services.AddHttpClient($"{appSettings.OrderApiHost}Client", client =>
                    {
                        client.BaseAddress = new Uri(appSettings.OrderApiHost);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        // Add other headers if needed, like auth tokens
                    });
                });
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder().Build();

            var orderConsole = host.Services.GetRequiredService<OrderConsole>();
            orderConsole.Start();

            Console.WriteLine("Done.");
        }
    }
}
