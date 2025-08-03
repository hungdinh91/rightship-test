using Microsoft.EntityFrameworkCore;
using OrderSystem.Infrastructure.Shared.Cache;
using OrderSystem.Shared.Contracts;
using ProductService.API.Middlewares;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.DbContexts;
using ProductService.Infrastructure.Repositories;
using StackExchange.Redis;

namespace ProductService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                dbContext.Database.Migrate();
                dbContext.SeedData();
            }

            app.MapControllers();

            app.Run();
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDb"));
            });

            var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new NotSupportedException("Redis has not been configured");
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));

            builder.Services.AddSingleton<IRedisCache, RedisCache>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
        }
    }
}
