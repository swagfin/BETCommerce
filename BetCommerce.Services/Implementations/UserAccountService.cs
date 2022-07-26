using BetCommerce.DataAccess;
using BetCommerce.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly ApplicationDbContext _db;

        public UserAccountService(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddAsync(UserAccount userAccount)
        {
            await _db.UserAccounts.AddAsync(userAccount);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAccount>> GetAllAsync(int page = -1, int size = -1)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? int.MaxValue : size;
            return await _db.UserAccounts.AsQueryable().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<UserAccount> GetAsync(string userAccountId)
        {
            return await _db.UserAccounts.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(userAccountId));
        }
        public async Task UpdateAsync(UserAccount userAccount)
        {
            _db.UserAccounts.Update(userAccount);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(string userAccountId)
        {
            UserAccount record = await GetAsync(userAccountId);
            if (record != null)
            {
                _db.UserAccounts.Remove(record);
                await _db.SaveChangesAsync();
            }
        }
    }
}
