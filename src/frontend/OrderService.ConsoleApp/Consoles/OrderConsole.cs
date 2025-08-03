using OrderSystem.Application.Shared.Services;
using OrderSystem.Domain.Shared.Dtos;

namespace OrderService.ConsoleApp.Consoles;

public class OrderConsole
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public OrderConsole(IProductService productService, IOrderService orderService)
    {
        _productService = productService;
        _orderService = orderService;
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
        var productsResult = _productService.GetProductsAsync().GetAwaiter().GetResult();
        if (!productsResult.IsSuccess || productsResult.Value == null)
        {
            Console.WriteLine("Can not get list of products. Please try again later.");
            return false;
        }

        if (productsResult.Value.Count == 0)
        {
            Console.WriteLine("We don't have any products to serve. Please try again later.");
            return false;
        }

        var products = productsResult.Value;
        var productNames = products.Select(x => x.ProductName).ToList();

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
        string? productName = string.Empty;
        ProductDto? product = null;
        bool isFirstTime = true;

        while (string.IsNullOrWhiteSpace(productName) || !productNames.Any(x => string.Equals(x, productName, StringComparison.OrdinalIgnoreCase)) || product == null)
        {
            Console.WriteLine();
            if (!isFirstTime)
            {
                Console.WriteLine("Product name is not correct. Please try enter again.");
            }

            Console.WriteLine($"Enter product name (required and need to be one of these values [{allProductNamesText}]):");
            productName = Console.ReadLine();
            isFirstTime = false;
            product = products.FirstOrDefault(x => string.Equals(x.ProductName, productName, StringComparison.OrdinalIgnoreCase));
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

        var orderItem = new SubmittedOrderItemDto();
        orderItem.ProductId = product.Id;
        orderItem.Quantity = quantity;

        var submittedOrder = new SubmittedOrderDto { CustomerName = customerName, OrderItems = [orderItem] };

        double total = orderItem.Quantity * product.Price;

        Console.WriteLine("Order complete!");
        Console.WriteLine("Customer: " + customerName);
        Console.WriteLine("Product: " + product.ProductName);
        Console.WriteLine("Price: $" + product.Price);
        Console.WriteLine("Quantity: " + orderItem.Quantity);
        Console.WriteLine("Total: $" + total);

        Console.WriteLine("Saving order to database...");
        var result = _orderService.SubmitOrder(submittedOrder).GetAwaiter().GetResult();
        if (!result.IsSuccess)
        {
            Console.WriteLine("We have an error when submit order: " + result.ErrorMessage);
            Console.WriteLine("Please try submit order again.");
            return false;
        }

        return true;
    }
}
