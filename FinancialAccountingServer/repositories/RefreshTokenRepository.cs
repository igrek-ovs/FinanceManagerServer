using FinancialAccountingServer.Data;
using FinancialAccountingServer.models;
using FinancialAccountingServer.repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountingServer.repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly FinancialAppContext _dbContext;

        public RefreshTokenRepository(FinancialAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRefreshToken(int userId)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (token != null)
            {
                await Task.Run(() => _dbContext.RefreshTokens.Remove(token));

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken> GetRefreshTokenByUserId(int userId)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
        }
    }
}
