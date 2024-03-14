using FinancialAccountingServer.models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FinancialAccountingServer.Data
{
    using Microsoft.EntityFrameworkCore;

    public class FinancialAppContext : DbContext
    {
        public FinancialAppContext(DbContextOptions<FinancialAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<ExpenseCategory> Categories { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupMembership> GroupMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure cascade delete behavior to NoAction for certain relationships
            modelBuilder.Entity<Group>()
                .HasMany(g => g.ExpenseCategories)
                .WithOne(ec => ec.Group)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Members)
                .WithOne(m => m.Group)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<ExpenseCategory>()
            //    .HasOne(ec => ec.User)
            //    .WithMany(u => u.ExpenseCategories)
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.Memberships)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .OnDelete(DeleteBehavior.NoAction);
        }



    }

}
