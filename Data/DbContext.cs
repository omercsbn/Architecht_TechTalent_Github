using Microsoft.EntityFrameworkCore;
using FinansalYonetimWebApi.Models;

namespace FinansalYonetimWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<DepositOption> DepositOptions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Altin> altins { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<Firm> Firms { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DepositOption>()
            .Property(d => d.InterestRate)
            .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transfer>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)");



            base.OnModelCreating(modelBuilder);
        }
    }
}
