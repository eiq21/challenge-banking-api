using Exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.Data.Configurations
{
    class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction", "Exchange");

            builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.CreatedBy).HasMaxLength(25);
            builder.Property(e => e.FixingExchangeRate).HasColumnType("decimal(5, 4)");
            builder.Property(e => e.SourceCurrency).IsRequired().HasMaxLength(3);
            builder.Property(e => e.TargetCurrency).IsRequired().HasMaxLength(3);
            builder.Property(e => e.Status).IsRequired().HasMaxLength(12).HasDefaultValueSql("('PENDIENTE')");
            builder.Property(e => e.SourceAmount).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.TargetAmount).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.TransactionAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasMaxLength(25);
            builder.HasOne(d => d.ExchangeRate).WithMany(p => p.Transaction).HasForeignKey(d => d.ExchangeRateId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
