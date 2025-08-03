using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Infrastructure.DbContexts;

public static class DbMigration
{
    public static void MigrateDb(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<OrderDbContext>();
        dbContext.Database.Migrate();
    }

    public static void SeedData(this OrderDbContext orderDbContext)
    {
    }
}
