using Moq;
using OrderService.Application.Commands;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Extensions;

namespace OrderService.Application.Tests;

public class SubmitOrderCommandTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Should_Return_Product_Does_Not_Exist_Fail_Result_Test()
    {
        var mockProductRepo = new Mock<IProductRepository>();
        var mockOrderRepo = new Mock<IOrderRepository>();
        var orderedProductId1 = Guid.NewGuid();

        //var product1 = new Product { Id = Guid.NewGuid(), ProductName = "Product 1", Price = 100 };
        //var product2 = new Product { Id = Guid.NewGuid(), ProductName = "Product 2", Price = 200 };

        mockProductRepo.Setup(x => x.GetByManyIdsAsync(new List<Guid> { orderedProductId1 }))
            .ReturnsAsync([]);

        var commandHandler = new SubmitOrderCommandHandler(mockOrderRepo.Object, mockProductRepo.Object);

        var command = new SubmitOrderCommand(new SubmittedOrderDto
        {
            CustomerName = "ABC",
            OrderItems = new List<SubmittedOrderItemDto>
            {
                new SubmittedOrderItemDto { ProductId = orderedProductId1, Quantity = 10 }
            }
        });

        var result = commandHandler.Handle(command, default).GetAwaiter().GetResult();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsSuccess, Is.EqualTo(false));
        Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ProductDoesNotExist));
        Assert.That(result.ErrorMessage,
            Is.EqualTo(string.Format(ErrorCode.ProductDoesNotExist.GetDescription(), orderedProductId1)));
    }

    [Test]
    public void Should_Return_Product_Does_Not_Exist_Fail_Result_When_Submit_Many_Test()
    {
        var mockProductRepo = new Mock<IProductRepository>();
        var mockOrderRepo = new Mock<IOrderRepository>();
        var orderedProductId1 = Guid.NewGuid();
        var orderedProductId2 = Guid.NewGuid();

        var product1 = new Product { Id = Guid.NewGuid(), ProductName = "Product 1", Price = 100 };
        //var product2 = new Product { Id = Guid.NewGuid(), ProductName = "Product 2", Price = 200 };

        mockProductRepo.Setup(x => x.GetByManyIdsAsync(new List<Guid> { orderedProductId1, orderedProductId2, product1.Id }))
            .ReturnsAsync([product1]);

        var commandHandler = new SubmitOrderCommandHandler(mockOrderRepo.Object, mockProductRepo.Object);

        var command = new SubmitOrderCommand(new SubmittedOrderDto
        {
            CustomerName = "ABC",
            OrderItems = new List<SubmittedOrderItemDto>
            {
                new SubmittedOrderItemDto { ProductId = orderedProductId1, Quantity = 10 },
                new SubmittedOrderItemDto { ProductId = orderedProductId2, Quantity = 10 },
                new SubmittedOrderItemDto { ProductId = product1.Id, Quantity = 10 },
            }
        });

        var result = commandHandler.Handle(command, default).GetAwaiter().GetResult();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsSuccess, Is.EqualTo(false));
        Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ProductDoesNotExist));

        var orderedIds = new List<string> { orderedProductId1.ToString(), orderedProductId2.ToString() }
        .OrderBy(x => x).ToList();
        var expectedMsg = string.Format(ErrorCode.ProductDoesNotExist.GetDescription(), string.Join(", ", orderedIds));

        Assert.That(result.ErrorMessage, Is.EqualTo(expectedMsg));
    }

    [Test]
    public void Should_Return_OK_Result_Test()
    {
        var mockProductRepo = new Mock<IProductRepository>();
        var mockOrderRepo = new Mock<IOrderRepository>();

        var product1 = new Product { Id = Guid.NewGuid(), ProductName = "Product 1", Price = 100 };
        var product2 = new Product { Id = Guid.NewGuid(), ProductName = "Product 2", Price = 200 };

        mockProductRepo.Setup(x => x.GetByManyIdsAsync(new List<Guid> { product1.Id, product2.Id }))
            .ReturnsAsync([product1, product2]);

        var commandHandler = new SubmitOrderCommandHandler(mockOrderRepo.Object, mockProductRepo.Object);

        var command = new SubmitOrderCommand(new SubmittedOrderDto
        {
            CustomerName = "ABC",
            OrderItems = new List<SubmittedOrderItemDto>
            {
                new SubmittedOrderItemDto { ProductId = product1.Id, Quantity = 10 },
                new SubmittedOrderItemDto { ProductId = product2.Id, Quantity = 5 },
            }
        });

        var result = commandHandler.Handle(command, default).GetAwaiter().GetResult();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsSuccess, Is.EqualTo(true));
        Assert.That(result.ErrorCode, Is.Null);
        Assert.That(result.ErrorMessage, Is.Null);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.CustomerName, Is.EqualTo("ABC"));
        Assert.That(result.Value.Total, Is.EqualTo(2000));
        Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Value.OrderItems, Is.Not.Null);
        Assert.That(result.Value.OrderItems.Count, Is.EqualTo(2));
        Assert.That(result.Value.OrderItems[0].ProductId, Is.EqualTo(product1.Id).Or.EqualTo(product2.Id));
        Assert.That(result.Value.OrderItems[1].ProductId, Is.EqualTo(product1.Id).Or.EqualTo(product2.Id));
        if (result.Value.OrderItems[0].ProductId.Equals(product1.Id))
        {
            Assert.That(result.Value.OrderItems[0].Price, Is.EqualTo(product1.Price));
            Assert.That(result.Value.OrderItems[0].Quantity, Is.EqualTo(10));
            Assert.That(result.Value.OrderItems[1].ProductId, Is.EqualTo(product2.Id));
            Assert.That(result.Value.OrderItems[1].Price, Is.EqualTo(product2.Price));
            Assert.That(result.Value.OrderItems[1].Quantity, Is.EqualTo(5));
        }
        else
        {
            Assert.That(result.Value.OrderItems[0].Price, Is.EqualTo(product2.Price));
            Assert.That(result.Value.OrderItems[0].Quantity, Is.EqualTo(5));
            Assert.That(result.Value.OrderItems[1].ProductId, Is.EqualTo(product1.Id));
            Assert.That(result.Value.OrderItems[1].Price, Is.EqualTo(product1.Price));
            Assert.That(result.Value.OrderItems[1].Quantity, Is.EqualTo(10));
        }
    }
}
