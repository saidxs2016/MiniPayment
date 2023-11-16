using Microsoft.EntityFrameworkCore;
using MiniPayment.Domain.Entities;

namespace MiniPayment.Infrastructure.Persistence.Context;

public partial class PaymentDbContext : DbContext
{

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }

    public PaymentDbContext()
    {
    }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your entities and relationships here
        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<TransactionDetail>()
            .HasKey(td => td.Id);

        // Define the relationship between Transaction and TransactionDetail
        modelBuilder.Entity<TransactionDetail>()
            .HasOne(td => td.Transaction)
            .WithMany(t => t.TransactionDetails)
            .HasForeignKey(td => td.TransactionId);

        base.OnModelCreating(modelBuilder);
    }

}
