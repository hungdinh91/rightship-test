namespace OrderService.Domain.Models
{
    public class Order : BaseEntity
    {
        public string CustomerName { get; set; }
        public double Total { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; }

        private Order() { }
        public Order(string customerName, List<OrderItem> orderItems)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;

            var dateNow = DateTimeOffset.Now;

            foreach (var item in orderItems)
            {
                item.OrderId = Id;
                CreatedAt = dateNow;
            }

            OrderItems = orderItems;
            Total = orderItems.Sum(x => x.Quantity * x.Price);
        }
    }
}
