using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;

namespace FinancialAccountingServer.Services.interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(UserDTO userDTO);

        Task<bool> Login(UserDTO userDTO);

        Task<string> GenerateAccessToken(UserDTO userDTO);

        Task<RefreshToken> GenerateRefreshToken(UserDTO userDto);

        Task<string> RefreshAccessToken(RefreshTokenDto refreshToken);

        Task<string> GetUserName(int userId);

        Task<bool> Save();

        Task<bool> AddAvatarToUser(int userId, string imagePath);

        Task<string> GetAvatarOfUser(int userId);

        Task<bool> DeleteAvatarOfUser(int userId);
    }
}
