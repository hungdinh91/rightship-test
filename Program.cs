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

                while(string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine();
                    Console.WriteLine("Customer name is required.");
                    Console.WriteLine("Please input customer name again.");
                    Console.WriteLine("Enter customer name:");
                    name = Console.ReadLine();
                }

                Console.WriteLine();
                Console.WriteLine("Enter product name:");
                string product = Console.ReadLine();

                var productNamesText = string.Join(", ", productNames);

                // Input product name to order
                while (string.IsNullOrWhiteSpace(product) || !productRepo.IsProductAvailable(product))
                {
                    Console.WriteLine();
                    Console.WriteLine($"Product name is required and have one of these values: {productNamesText}.");
                    Console.WriteLine("Please input product again.");
                    Console.WriteLine("Enter product name:");
                    product = Console.ReadLine();
                }

                double price = productRepo.GetPrice(product);

                Console.WriteLine();
                Console.WriteLine("Enter quantity:");
                var inputQuantityString = Console.ReadLine();
                int quantity = 0;

                while (string.IsNullOrWhiteSpace(inputQuantityString) || !int.TryParse(inputQuantityString, out quantity))
                {
                    Console.WriteLine();
                    Console.WriteLine("Quantity is required and must be a positive integer.");
                    Console.WriteLine("Please input quantity again.");
                    Console.WriteLine("Enter quantity:");
                    inputQuantityString = Console.ReadLine();
                }

                Console.WriteLine();
                Console.WriteLine("Processing order...");

                Order order = new Order();
                order.CustomerName = name;
                order.ProductName = product;
                order.Quantity = quantity;
                order.Price = price;

                double total = order.Quantity * order.Price;

                Console.WriteLine("Order complete!");
                Console.WriteLine("Customer: " + order.CustomerName);
                Console.WriteLine("Product: " + order.ProductName);
                Console.WriteLine("Price: $" + order.Price);
                Console.WriteLine("Quantity: " + order.Quantity);
                Console.WriteLine("Total: $" + total);

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
