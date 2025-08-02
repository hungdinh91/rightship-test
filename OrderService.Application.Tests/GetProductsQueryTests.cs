using Moq;
using OrderService.Application.Queries;
using OrderService.Domain.Contracts;
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.SharedKernel.Constants;
using OrderService.SharedKernel.Dtos;

namespace OrderService.Application.Tests;

public class GetProductsQueryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Should_Get_Empty_Products_From_Cache_Test()
    {
        var mockRepo = new Mock<IProductRepository>();
        var product1 = new Product { Id = Guid.NewGuid(), ProductName = "Product 1", Price = 100 };
        mockRepo.Setup(repo => repo.GetProductsAsync()).ReturnsAsync([product1]);

        var mockRedisCache = new Mock<IRedisCache>();
        mockRedisCache.Setup(redis => redis.GetAsync<List<ProductDto>>(CacheKeys.Products)).ReturnsAsync([]);

        var query = new GetProductsQueryHandler(mockRepo.Object, mockRedisCache.Object);
        var result = query.Handle(new GetProductsQuery(), default).GetAwaiter().GetResult();

        Assert.IsNotNull(result);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Count, Is.EqualTo(0));
    }

    [Test]
    public void Should_Get_Products_From_Cache_Test()
    {
        var mockRepo = new Mock<IProductRepository>();
        var product1 = new Product { Id = Guid.NewGuid(), ProductName = "Product 1", Price = 100 };
        mockRepo.Setup(repo => repo.GetProductsAsync()).ReturnsAsync([product1]);

        var mockRedisCache = new Mock<IRedisCache>();
        var productDto2 = new ProductDto { Id = Guid.NewGuid(), ProductName = "Product 2", Price = 200 };
        var productDto3 = new ProductDto { Id = Guid.NewGuid(), ProductName = "Product 3", Price = 300 };
        mockRedisCache.Setup(redis => redis.GetAsync<List<ProductDto>>(CacheKeys.Products)).ReturnsAsync([productDto2, productDto3]);

        var query = new GetProductsQueryHandler(mockRepo.Object, mockRedisCache.Object);
        var result = query.Handle(new GetProductsQuery(), default).GetAwaiter().GetResult();

        Assert.IsNotNull(result);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Count, Is.EqualTo(2));

        Assert.That(result.Value[0].Id, Is.EqualTo(productDto2.Id).Or.EqualTo(productDto3.Id));
        if (result.Value[0].Id.Equals(productDto2.Id))
        {
            Assert.That(result.Value[0].ProductName, Is.EqualTo(productDto2.ProductName));
            Assert.That(result.Value[0].Price, Is.EqualTo(productDto2.Price));
            Assert.That(result.Value[1].Id, Is.EqualTo(productDto3.Id));
            Assert.That(result.Value[1].ProductName, Is.EqualTo(productDto3.ProductName));
            Assert.That(result.Value[1].Price, Is.EqualTo(productDto3.Price));
        }
        else
        {
            Assert.That(result.Value[0].ProductName, Is.EqualTo(productDto3.ProductName));
            Assert.That(result.Value[0].Price, Is.EqualTo(productDto3.Price));
            Assert.That(result.Value[1].Id, Is.EqualTo(productDto2.Id));
            Assert.That(result.Value[1].ProductName, Is.EqualTo(productDto2.ProductName));
            Assert.That(result.Value[1].Price, Is.EqualTo(productDto2.Price));
        }
    }

    [Test]
    public void Should_Get_Empty_Products_From_Db_Test()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetProductsAsync()).ReturnsAsync([]);

        var mockRedisCache = new Mock<IRedisCache>();
        List<ProductDto>? cachedResult = null;
        mockRedisCache.Setup(redis => redis.GetAsync<List<ProductDto>>(CacheKeys.Products)).ReturnsAsync(cachedResult);

        var query = new GetProductsQueryHandler(mockRepo.Object, mockRedisCache.Object);
        var result = query.Handle(new GetProductsQuery(), default).GetAwaiter().GetResult();

        Assert.IsNotNull(result);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Count, Is.EqualTo(0));
    }

    [Test]
    public void Should_Get_Products_From_Db_Test()
    {
        var mockRepo = new Mock<IProductRepository>();
        var product1 = new Product { Id = Guid.NewGuid(), ProductName = "Product 1", Price = 100 };
        mockRepo.Setup(repo => repo.GetProductsAsync()).ReturnsAsync([product1]);

        var mockRedisCache = new Mock<IRedisCache>();
        List<ProductDto>? cachedResult = null;
        mockRedisCache.Setup(redis => redis.GetAsync<List<ProductDto>>(CacheKeys.Products)).ReturnsAsync(cachedResult);

        var query = new GetProductsQueryHandler(mockRepo.Object, mockRedisCache.Object);
        var result = query.Handle(new GetProductsQuery(), default).GetAwaiter().GetResult();

        Assert.IsNotNull(result);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Count, Is.EqualTo(1));
        Assert.That(result.Value[0].Id, Is.EqualTo(product1.Id));
        Assert.That(result.Value[0].ProductName, Is.EqualTo(product1.ProductName));
        Assert.That(result.Value[0].Price, Is.EqualTo(product1.Price));
    }
}