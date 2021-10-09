using Exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Data
{
    public class ExchangeDbContext:DbContext
    {
        public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options)
            :base(options)
        {

        }

        public virtual DbSet<ExchangeRate> Currencies { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Database schema
            modelBuilder.HasDefaultSchema("Exchange");

            //Register
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExchangeDbContext).Assembly);

            //Singularize name tables
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }
        }
    }
}
