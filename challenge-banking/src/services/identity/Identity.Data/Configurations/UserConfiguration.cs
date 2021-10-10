using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Identity", "User");
            builder.Property(e => e.Email).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PasswordHash).IsRequired();
            builder.Property(e => e.PasswordSalt).IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValueSql("((1))");
        }
    }
}
