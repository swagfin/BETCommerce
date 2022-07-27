using BetCommerce.Entity.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BetCommerce.Services
{
    public interface IOrderService
    {
        Task AddAsync(Order order);
        Task<IEnumerable<Order>> GetAllAsync(int page = -1, int size = -1);
        Task<IEnumerable<Order>> GetAllByUserIdAsync(string userAccountId, int page = -1, int size = -1);
        Task<Order> GetAsync(int orderId);
        Task UpdateAsync(Order order);
        Task RemoveAsync(int orderId);
    }
}
