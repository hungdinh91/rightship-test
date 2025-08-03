using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Infrastructure.DbContexts;

public static class DbMigration
{
    public static void MigrateDb(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ProductDbContext>();
        dbContext.Database.Migrate();
    }

    public static void SeedData(this ProductDbContext orderDbContext)
    {
        var products = orderDbContext.Products.ToList();

        if (!products.Any(p => string.Equals(p.ProductName, "Widget", StringComparison.OrdinalIgnoreCase)))
        {
            orderDbContext.Products.Add(new Domain.Models.Product
            {
                ProductName = "Widget",
                Price = 12.99,
                CreatedAt = DateTimeOffset.Now
            });
        }

        if (!products.Any(p => string.Equals(p.ProductName, "Gadget", StringComparison.OrdinalIgnoreCase)))
        {
            orderDbContext.Products.Add(new Domain.Models.Product
            {
                ProductName = "Gadget",
                Price = 15.49,
                CreatedAt = DateTimeOffset.Now
            });
        }

        if (!products.Any(p => string.Equals(p.ProductName, "Doohickey", StringComparison.OrdinalIgnoreCase)))
        {
            orderDbContext.Products.Add(new Domain.Models.Product
            {
                ProductName = "Doohickey",
                Price = 8.75,
                CreatedAt = DateTimeOffset.Now
            });
        }

        orderDbContext.SaveChanges();
    }
}
