using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.ConsoleApp.Consoles;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Infrastructure.DbContexts;
using OrderService.Infrastructure.Repositories;

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
                    services.AddDbContext<OrderDbContext>(options =>
                    {
                        var connectionString = Configuration.GetConnectionString("OrderDb");
                        options.UseSqlServer(connectionString);
                    }, ServiceLifetime.Singleton);
                    services.AddSingleton<IProductRepository, ProductRepository>();
                    services.AddSingleton<IOrderRepository, OrderRepository>();
                    services.AddSingleton<OrderConsole, OrderConsole>();
                });
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder().Build();
            var dbContext = host.Services.GetRequiredService<OrderDbContext>();
            dbContext.Database.Migrate();
            dbContext.SeedData();

            var orderConsole = host.Services.GetRequiredService<OrderConsole>();
            orderConsole.Start();

            Console.WriteLine("Done.");
        }
    }
}
