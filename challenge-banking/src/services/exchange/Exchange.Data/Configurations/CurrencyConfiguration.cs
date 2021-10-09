using Exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currency", "Exchange");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Symbol).IsRequired().HasMaxLength(3);
            builder.Property(e => e.Code).IsRequired().HasMaxLength(3);
            builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(25);
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasMaxLength(25);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValueSql("((1))");
        }
    }
}
