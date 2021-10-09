using Exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Data.Configurations
{
    public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.ExchangeRate> builder)
        {
            builder.ToTable("TypeChange", "Exchange");
            builder.Property(e => e.Pair).IsRequired().HasMaxLength(7);
            builder.Property(e => e.Offer).IsRequired().HasColumnType("decimal(5,4)");
            builder.Property(e => e.Demand).IsRequired().HasColumnType("decimal(5,4)");
            builder.Property(e => e.ExchangeRateAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(25);
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasMaxLength(25);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValueSql("((1))");
        }
    }
}
