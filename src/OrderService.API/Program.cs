using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using OrderService.API.Middlewares;
using OrderService.Domain.Contracts;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Infrastructure.Cache;
using OrderService.Infrastructure.DbContexts;
using OrderService.Infrastructure.Repositories;
using StackExchange.Redis;

namespace OrderService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb"));
            });

            var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new NotSupportedException("Redis has not been configured");
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));

            builder.Services.AddSingleton<IRedisCache, RedisCache>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

            // Add Application Insights logging
            builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                    config.ConnectionString = builder.Configuration.GetConnectionString("ApplicationInsights"),
                configureApplicationInsightsLoggerOptions: (options) => { }
            );

            // Optional: Set log level filter
            builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("OrderService", LogLevel.Debug);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionHandling>();
            app.UseAuthorization();

            // Migrate the database
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                dbContext.Database.Migrate();
                dbContext.SeedData();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
