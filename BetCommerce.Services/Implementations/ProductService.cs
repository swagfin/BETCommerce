using BetCommerce.DataAccess;
using BetCommerce.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int page = -1, int size = -1)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? int.MaxValue : size;
            return await _db.Products.AsQueryable().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetAsync(int productId)
        {
            return await _db.Products.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.ProductId.Equals(productId));
        }
        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int productId)
        {
            Product record = await GetAsync(productId);
            if (record != null)
            {
                _db.Products.Remove(record);
                await _db.SaveChangesAsync();
            }
        }

    }
}
