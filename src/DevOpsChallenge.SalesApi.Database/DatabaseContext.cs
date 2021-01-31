using DevOpsChallenge.SalesApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevOpsChallenge.SalesApi.Database
{
    /// <summary>
    /// Entity Framework database context.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configure the database model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sale
            EntityTypeBuilder<SaleEntity> saleEntity = modelBuilder.Entity<SaleEntity>();
            saleEntity.ToTable("Sale");
            saleEntity.HasKey(x => x.Id);

            saleEntity.Property(x => x.TransactionId).IsRequired().HasMaxLength(32);
            saleEntity.Property(x => x.Date).IsRequired();
            saleEntity.Property(x => x.Amount).IsRequired();
            saleEntity.Property(x => x.Notes).HasMaxLength(256);
        }
    }
}
