using BetCommerce.DataAccess;
using BetCommerce.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync(int page = -1, int size = -1)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? int.MaxValue : size;
            return await _db.Orders.AsQueryable().OrderByDescending(x => x.Id).Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllByUserIdAsync(string userAccountId, int page = -1, int size = -1)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? int.MaxValue : size;
            return await _db.Orders.AsQueryable().Where(x => x.UserAccountId.Equals(userAccountId)).Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }
        public async Task<Order> GetAsync(int orderId)
        {
            return await _db.Orders.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(orderId));
        }
        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int orderId)
        {
            Order record = await GetAsync(orderId);
            if (record != null)
            {
                _db.Orders.Remove(record);
                await _db.SaveChangesAsync();
            }
        }

    }
}
