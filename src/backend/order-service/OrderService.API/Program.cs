using Microsoft.EntityFrameworkCore;
using OrderService.API.Middlewares;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Infrastructure.DbContexts;
using OrderService.Infrastructure.Repositories;
using OrderSystem.Application.Shared.Services;
using OrderSystem.Infrastructure.Shared.Cache;
using OrderSystem.Shared.Contracts;
using OrderSystem.Shared.Policies;
using StackExchange.Redis;

namespace OrderService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            RegisterServices(builder);

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

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb"));
            });

            var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new NotSupportedException("Redis has not been configured");
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));

            builder.Services.AddHttpClient($"ProductServiceClient", client =>
            {
                var apiHost = builder.Configuration.GetConnectionString("ProductApiHost") ?? throw new NotSupportedException();
                client.BaseAddress = new Uri(apiHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                // Add other headers if needed, like auth tokens
            }).AddPolicyHandler(RetryPolicy.GetPolicy());

            builder.Services.AddSingleton<IRedisCache, RedisCache>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IHttpClientService, HttpClientService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
        }
    }
}
