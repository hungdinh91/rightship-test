namespace OrderService.Domain.Models
{
    public class Order : BaseEntity
    {
        public string CustomerName { get; set; }
        public double Total { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
