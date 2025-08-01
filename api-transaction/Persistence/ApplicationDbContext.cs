using api_transaction.Entities;
using Microsoft.EntityFrameworkCore;

namespace api_transaction.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<TransactionDetail>()
                .Property(td => td.TransactionId)
                .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<TransactionDetail>()
                .Property(td => td.ChangedAt)
                .HasDefaultValueSql("now()");


        }

    }
}
