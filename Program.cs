using System;
using LegacyOrderService.Models;
using LegacyOrderService.Data;

namespace LegacyOrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");

            var productRepo = new ProductRepository();
            var productNames = productRepo.GetProductNames().OrderBy(x => x).ToArray();

            if (productNames.Any())
            {
                Console.WriteLine("Enter customer name:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter product name:");
                string product = Console.ReadLine();

                var productNamesText = string.Join(", ", productNames);

                // Input product name to order
                while (string.IsNullOrWhiteSpace(product) || !productRepo.IsProductAvailable(product))
                {
                    Console.WriteLine($"Product name is required and have one of these values: {productNamesText}.");
                    Console.WriteLine("Enter product name:");
                    product = Console.ReadLine();
                }

                double price = productRepo.GetPrice(product);

                Console.WriteLine("Enter quantity:");
                int qty = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Processing order...");

                Order order = new Order();
                order.CustomerName = name;
                order.ProductName = product;
                order.Quantity = qty;
                order.Price = 10.0;

                double total = order.Quantity * order.Price;

                Console.WriteLine("Order complete!");
                Console.WriteLine("Customer: " + order.CustomerName);
                Console.WriteLine("Product: " + order.ProductName);
                Console.WriteLine("Quantity: " + order.Quantity);
                Console.WriteLine("Total: $" + price);

                Console.WriteLine("Saving order to database...");
                var repo = new OrderRepository();
                repo.Save(order);
            }
            else
            {
                Console.WriteLine("No products are currently available. Please check back again soon.");
            }

            Console.WriteLine("Done.");
        }
    }
}
