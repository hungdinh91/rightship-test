using OrderService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Contracts.Repositories;

public interface IProductRepository
{
    List<string> GetProductNames();
    Product? GetProductByName(string? productName);
}
