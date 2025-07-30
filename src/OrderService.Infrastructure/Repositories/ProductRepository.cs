// Data/ProductRepository.cs
using OrderService.Domain.Contracts.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OrderService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderDbContext _dbContext;

        public ProductRepository(OrderDbContext orderDbContext)
        {
            _dbContext = orderDbContext;
        }

        public Product? GetProductByName(string? productName)
        {
            return _dbContext.Products.FirstOrDefault(p => p.ProductName == productName);
        }

        public List<string> GetProductNames()
        {
            return _dbContext.Products.Select(x => x.ProductName).ToList();
        }
    }
}
