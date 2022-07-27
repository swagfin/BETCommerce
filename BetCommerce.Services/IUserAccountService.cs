using BetCommerce.Entity.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetCommerce.Services
{
    public interface IUserAccountService
    {
        Task AddAsync(UserAccount userAccount);
        Task<IEnumerable<UserAccount>> GetAllAsync(int page = -1, int size = -1);
        Task<UserAccount> GetAsync(string userAccountId);
        Task UpdateAsync(UserAccount userAccount);
        Task RemoveAsync(string userAccountId);
        Task<UserAccount> SignInAsync(string emailAddress, string password);
    }
}
