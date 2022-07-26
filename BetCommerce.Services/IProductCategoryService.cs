using BetCommerce.Entity.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetCommerce.Services
{
    public interface IProductCategoryService
    {
        Task AddAsync(ProductCategory productCategory);
        Task<IEnumerable<ProductCategory>> GetAllAsync(int page = -1, int size = -1);
        Task<ProductCategory> GetAsync(int productCategoryId);
        Task UpdateAsync(ProductCategory productCategory);
        Task RemoveAsync(int productCategoryId);
    }
}
