using BetCommerce.DataAccess;
using BetCommerce.Entity.Core;
using BetCommerce.Entity.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
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
            //Change Password to MDF
            string existEmail = await _db.UserAccounts.AsQueryable().Select(x => x.EmailAddress).AsNoTracking().FirstOrDefaultAsync(x => x.ToLower().Equals(userAccount.EmailAddress.ToLower()));
            if (!string.IsNullOrWhiteSpace(existEmail))
                throw new Exception($"The Email Address: {userAccount.EmailAddress} is already registered");
            userAccount.PasswordHash = userAccount.PasswordHash.ToMD5String();
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

        public async Task<UserAccount> SignInAsync(string emailAddress, string password)
        {
            string encryptedHash = password.ToMD5String();
            UserAccount user = await _db.UserAccounts.AsQueryable().FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress));
            if (user == null)
                throw new Exception($"No registered account with the provided email address: {emailAddress} , re-check inputs then try again.");
            else if (!user.PasswordHash.Equals(encryptedHash))
            {
                user.InvalidLogins += 1;
                int attemptsLeft = 6 - user.InvalidLogins;
                await _db.SaveChangesAsync();
                if (attemptsLeft < 1)
                {
                    attemptsLeft = 0;
                    user.IsActive = false;
                    await _db.SaveChangesAsync();
                    throw new Exception($"Invalid Password Provided, this Account has now been SUSPENDED because of too many Wrong Attempts. Please Contact your Administrator to Reset your Password. You can also Try Resetting your Password.");
                }
                throw new Exception($"Invalid Password Provided, Enter valid Password. Attempts Left ({attemptsLeft}).");
            }
            else if (!user.IsActive)
                throw new Exception($"Your account is currently SUSPENDED. Please contact the Administrator to reset your account or try reseting your password");
            else
            {
                user.InvalidLogins = 0;
                user.LastLogin = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
            return user;
        }
    }
}
