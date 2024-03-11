using FinancialAccountingServer.models;

namespace FinancialAccountingServer.repositories.interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshToken(RefreshToken refreshToken);

        Task RemoveRefreshToken(int userId);

        Task<RefreshToken> GetRefreshTokenByUserId(int userId);
    }
}
