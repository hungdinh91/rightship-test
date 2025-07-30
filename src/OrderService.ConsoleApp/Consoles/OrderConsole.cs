using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;

namespace OrderService.ConsoleApp.Consoles;

public class OrderConsole
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository; 

    public OrderConsole(IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public void Start()
    {
        bool isStopped = false;
        while (!isStopped)
        {
            var canOrder = OrderProducts();
            if (!canOrder) isStopped = true;
            else
            {
                var keyChar = ' ';
                while (keyChar != 'Y' && keyChar != 'N' && keyChar != 'y' && keyChar != 'n')
                {
                    Console.WriteLine();
                    Console.WriteLine("Do you want to continue (Y/N): ");
                    var key = Console.ReadKey();
                    keyChar = key.KeyChar;
                }

                if (keyChar == 'N' || keyChar == 'n')
                {
                    isStopped = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine();
                }
            }
        }
    }

    private bool OrderProducts()
    {
        Console.WriteLine("Welcome to Order Processor!");
        var productNames = _productRepository.GetProductNames().OrderBy(x => x).ToArray();

        if (productNames.Any())
        {
            // Input customer name
            Console.WriteLine("Enter customer name (required):");
            string? customerName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(customerName))
            {
                Console.WriteLine();
                Console.WriteLine("Customer name is required.");
                Console.WriteLine("Please input customer name again.");
                Console.WriteLine("Enter customer name:");
                customerName = Console.ReadLine();
            }

            // Input product name to order
            var allProductNamesText = string.Join(", ", productNames);
            Console.WriteLine();
            Console.WriteLine($"Enter product name (required and need to be one of these values [{allProductNamesText}]):");
            string? productName = Console.ReadLine();
            Product? product = productNames.Contains(productName) ? _productRepository.GetProductByName(productName) : null;

            while (string.IsNullOrWhiteSpace(productName) || !productNames.Contains(productName) || product == null) 
            {
                Console.WriteLine();
                Console.WriteLine($"Product name is required and need to be one of these values: {allProductNamesText}.");
                Console.WriteLine("Please input product again.");
                Console.WriteLine("Enter product name:");
                productName = Console.ReadLine();
                product = _productRepository.GetProductByName(productName);
            }

            // Input quantity want to order
            Console.WriteLine();
            Console.WriteLine("Enter quantity (required and need to be a positive integer):");
            var inputQuantityString = Console.ReadLine();
            int quantity = 0;

            while (string.IsNullOrWhiteSpace(inputQuantityString) || !int.TryParse(inputQuantityString, out quantity) || quantity <= 0)
            {
                Console.WriteLine();
                Console.WriteLine("Quantity is required and must be a positive integer.");
                Console.WriteLine("Please input quantity again.");
                Console.WriteLine("Enter quantity:");
                inputQuantityString = Console.ReadLine();
            }

            // Process order
            Console.WriteLine();
            Console.WriteLine("Processing order...");

            Order order = new Order();
            order.CustomerName = customerName;
            
            OrderItem orderItem = new OrderItem();
            orderItem.OrderId = order.Id;
            orderItem.ProductId = product.Id;
            orderItem.Quantity = quantity;
            orderItem.Price = product.Price;

            order.OrderItems = new List<OrderItem>() { orderItem };

            double total = orderItem.Quantity * orderItem.Price;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + product.ProductName);
            Console.WriteLine("Price: $" + orderItem.Price);
            Console.WriteLine("Quantity: " + orderItem.Quantity);
            Console.WriteLine("Total: $" + total);

            Console.WriteLine("Saving order to database...");
            _orderRepository.Create(order);

            return true;
        }
        else
        {
            Console.WriteLine("No products are currently available. Please check back again soon.");
            return false;
        }
    }
}
