using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure.DbContexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext() { }
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.CustomerName).IsRequired();
                entity.Property(x => x.Total).IsRequired();
                entity.HasIndex(x => x.CustomerName);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.OrderId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.Price).IsRequired();
                entity.Property(x => x.Quantity).IsRequired();
                entity.HasOne(x => x.Order).WithMany(x => x.OrderItems).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
