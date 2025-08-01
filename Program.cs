using System;
using LegacyOrderService.Models;
using LegacyOrderService.Data;

namespace LegacyOrderService
{
    class Program
    {
        static void Main(string[] args)
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

            Console.WriteLine("Done.");
        }

        static bool OrderProducts()
        {
            Console.WriteLine("Welcome to Order Processor!");
            var productRepo = new ProductRepository();
            var productNames = productRepo.GetProductNames().OrderBy(x => x).ToArray();

            if (productNames.Any())
            {
                // Input customer name
                Console.WriteLine("Enter customer name (required):");
                string name = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine();
                    Console.WriteLine("Customer name is required.");
                    Console.WriteLine("Please input customer name again.");
                    Console.WriteLine("Enter customer name:");
                    name = Console.ReadLine();
                }

                // Input product name to order
                var productNamesText = string.Join(", ", productNames);
                Console.WriteLine();
                Console.WriteLine($"Enter product name (required and need to be one of these values [{productNamesText}]):");
                string product = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(product) || !productRepo.IsProductAvailable(product))
                {
                    Console.WriteLine();
                    Console.WriteLine($"Product name is required and need to be one of these values: {productNamesText}.");
                    Console.WriteLine("Please input product again.");
                    Console.WriteLine("Enter product name:");
                    product = Console.ReadLine();
                }

                double price = productRepo.GetPrice(product);

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

                return true;
            }
            else
            {
                Console.WriteLine("No products are currently available. Please check back again soon.");
                return false;
            }
        }
    }
}
