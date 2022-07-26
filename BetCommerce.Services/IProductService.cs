﻿using BetCommerce.Entity.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BetCommerce.Services
{
    public interface IProductService
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync(int page = -1, int size = -1);
        Task<IEnumerable<ProductStockCard>> GetStockHistoryAsync(int productId, DateTime dateFromUtc, DateTime dateToUtc);
        Task<Product> GetAsync(int productId);
        Task UpdateAsync(Product product);
        Task RemoveAsync(int productId);
    }
}
