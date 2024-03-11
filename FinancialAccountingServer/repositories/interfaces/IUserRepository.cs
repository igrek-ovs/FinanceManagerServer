using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;

namespace FinancialAccountingServer.repositories.interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUser(User user);

        Task<User> GetUserByUsername(string userName);

        Task<bool> CheckUsernameExists(string userName);

        Task<bool> AddAvatarToUser(int userId, string imagePath);

        Task<string> GetAvatarOfUser(int userId);

        Task<bool> DeleteAvatarOfUser(int userId);

        Task<string> GetUserName(int userId);

        Task<bool> Save();

        Task<string> GetRoleByUserName(string userName);

        Task<User> GetUserById(int userId);
    }
}
