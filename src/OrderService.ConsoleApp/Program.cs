using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.ConsoleApp.Consoles;
using OrderService.ConsoleApp.Settings;
using Polly.Extensions.Http;
using Polly;
using OrderService.ConsoleApp.Helpers;
using OrderService.ConsoleApp.Services;
using Serilog;

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

                    services.AddHttpClient($"{appSettings.OrderApiHost}-Client", client =>
                    {
                        client.BaseAddress = new Uri(appSettings.OrderApiHost);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        // Add other headers if needed, like auth tokens
                    })
                    .AddPolicyHandler(GetRetryPolicy());

                    services.AddSingleton<IHttpClientService, HttpClientService>();
                    services.AddSingleton<IProductService, ProductService>();
                    services.AddSingleton<IOrderService, Services.OrderService>();
                });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // 5xx, 408
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder().Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application Starting Up");

            var orderConsole = host.Services.GetRequiredService<OrderConsole>();
            try
            {
                orderConsole.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Log.CloseAndFlush();
            Console.WriteLine("Done.");
        }
    }
}
