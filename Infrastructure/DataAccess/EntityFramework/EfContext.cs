using Domain.Entities.BankAccounts;
using Domain.Entities.Banks;
using Domain.Entities.BankTransactions;
using Domain.Entities.FeeTransactions;
using Domain.Entities.ModerationActions;
using Domain.Entities.Users;
using Domain.Entities.WalletTransferTransactions;
using Infrastructure.DataAccess.EntityFramework.Entities;
using Infrastructure.DataAccess.EntityFramework.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace Infrastructure.DataAccess.EntityFramework
{
    public class EfContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        // Properties
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<FeeTransaction> FeeTransactions { get; set; }
        public DbSet<ModerationAction> ModerationActions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> DomainUsers { get; set; }
        public DbSet<WalletTransferTransaction> UserTransactions { get; set; }

        
        // Constructors
        public EfContext(DbContextOptions<EfContext> dbContextOptions) : base(dbContextOptions) { }

        // Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new BankConfiguration());
            modelBuilder.ApplyConfiguration(new ModerationActionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WalletTransferTransactionConfiguration());
        }
    }
}
