using BetCommerce.DataAccess;
using BetCommerce.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System;
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
        private void RunPreOrderChecksAndCalculations(Order order)
        {
            //Verify If order has Order Items
            if (order.OrderItems == null || order.OrderItems.Count() < 1)
                throw new Exception("No items added to Transaction, Add Order items then try again");
            //Re-do some order Recalculations (Don't trust what client is sending)
            order.TotalItems = order.OrderItems.Sum(x => x.Quantity);
            order.Tax = 0; //Will inject Tax Inclusive Taxation Service
            order.SubTotal = Math.Round(order.OrderItems.Sum(x => x.TotalCost), 4);
            if (order.Discount < 1) { order.Discount = 0; }
            if (order.DeliveryCost < 1) { order.DeliveryCost = 0; }
            //Final
            order.DueAmount = Math.Round(((order.SubTotal + order.DeliveryCost) - order.Discount), 4);
            order.SubTotal -= Math.Round(order.Tax, 4);
        }
        private async Task VerifyIfQuantityExistsAsync(List<OrderItem> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
                return;
            string exceptionMsg = string.Empty;
            foreach (OrderItem item in orderItems)
            {
                //Get Main Product
                var product = await _db.Products.AsQueryable().Select(x => new { x.ProductId, x.CurrentQuantity }).FirstOrDefaultAsync(x => x.ProductId.Equals(item.ProductId));
                if (product == null)
                    exceptionMsg = $"{exceptionMsg}No registered Product with this Product Id: {item.ProductId}\n";
                else if (item.Quantity > product.CurrentQuantity)
                    exceptionMsg = $"{exceptionMsg}{item.ProductName} is currently OUT OF STOCK!\n";
            }
        }

        public async Task AddAsync(Order order)
        {
            RunPreOrderChecksAndCalculations(order);
            await VerifyIfQuantityExistsAsync(order.OrderItems);
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
