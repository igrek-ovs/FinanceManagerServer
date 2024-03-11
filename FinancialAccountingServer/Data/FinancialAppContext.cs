using FinancialAccountingServer.models;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountingServer.Data
{
    public class FinancialAppContext : DbContext
    {
        public FinancialAppContext(DbContextOptions<FinancialAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
