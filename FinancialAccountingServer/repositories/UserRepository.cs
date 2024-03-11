using FinancialAccountingServer.Data;
using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountingServer.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinancialAppContext _dbContext;

        public UserRepository(FinancialAppContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Save()
        {
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> AddAvatarToUser(int userId, string imagePath)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.AvatarPath = imagePath;

            return await Save();
        }

        public async Task<bool> AddUser(User user)
        {
            await _dbContext.AddAsync(user);

            return await Save();
        }

        public async Task<bool> CheckUsernameExists(string userName)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);

            return existingUser != null;
        }

        public async Task<bool> DeleteAvatarOfUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            
            if (user == null)
            {
                return false;
            }

            user.AvatarPath = null;

            return await Save();
        }

        public async Task<string> GetAvatarOfUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user.AvatarPath;
        }

        public async Task<User> GetUserByUsername(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);

            return user;
        }

        public async Task<string> GetUserName(int userId)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return existingUser.Username;
        }

        public async Task<string> GetRoleByUserName(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);

            return user.Role;
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
    }
}
