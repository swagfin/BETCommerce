using BetCommerce.DataAccess;
using BetCommerce.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.Services.Implementations
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ApplicationDbContext _db;

        public ProductCategoryService(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddAsync(ProductCategory productCategory)
        {
            await _db.ProductCategories.AddAsync(productCategory);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync(int page = -1, int size = -1)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? int.MaxValue : size;
            return await _db.ProductCategories.AsQueryable().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<ProductCategory> GetAsync(int productCategoryId)
        {
            return await _db.ProductCategories.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(productCategoryId));
        }
        public async Task UpdateAsync(ProductCategory productCategory)
        {
            _db.ProductCategories.Update(productCategory);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int productCategoryId)
        {
            ProductCategory record = await GetAsync(productCategoryId);
            if (record != null)
            {
                _db.ProductCategories.Remove(record);
                await _db.SaveChangesAsync();
            }
        }

    }
}
