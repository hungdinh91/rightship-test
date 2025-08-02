using Microsoft.EntityFrameworkCore;
using OrderService.API.Middlewares;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Infrastructure.DbContexts;
using OrderService.Infrastructure.Repositories;

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

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
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
