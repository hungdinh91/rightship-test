using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.ConsoleApp.Consoles;
using OrderSystem.Application.Shared.Services;
using OrderSystem.Shared.Policies;

namespace OrderService.ConsoleApp
{
    class Program
    {
        private static IConfiguration Configuration = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile("appsettings.json").Build();

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<OrderConsole, OrderConsole>();

                    services.AddHttpClient($"ProductServiceClient", client =>
                    {
                        var apiHost = Configuration.GetConnectionString("ProductApiHost") ?? throw new NotSupportedException();
                        client.BaseAddress = new Uri(apiHost);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        // Add other headers if needed, like auth tokens
                    }).AddPolicyHandler(RetryPolicy.GetPolicy());

                    services.AddHttpClient($"OrderServiceClient", client =>
                    {
                        var apiHost = Configuration.GetConnectionString("OrderApiHost") ?? throw new NotSupportedException();
                        client.BaseAddress = new Uri(apiHost);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        // Add other headers if needed, like auth tokens
                    }).AddPolicyHandler(RetryPolicy.GetPolicy());

                    services.AddSingleton<IHttpClientService, HttpClientService>();
                    services.AddSingleton<IProductService, ProductService>();
                    services.AddSingleton<IOrderService, OrderSystem.Application.Shared.Services.OrderService>();
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
