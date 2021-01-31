using DevOpsChallenge.SalesApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Database.Repositories
{
    /// <summary>
    /// Repository for sales.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public SaleRepository(DatabaseContext dbContext)
        {
            // Store the context
            this.Context = dbContext;

            // Initialise the DbSet
            this.Entities = this.Context.Set<SaleEntity>();
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected DatabaseContext Context { get; }

        /// <summary>
        /// Gets the entity set.
        /// </summary>
        protected DbSet<SaleEntity> Entities { get; }

        /// <inheritdoc />
        public async Task<SaleEntity> FindAsync(string transactionId)
        {
            return await this.Entities
                .FirstOrDefaultAsync(x => x.TransactionId == transactionId)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SaleEntity>> GetAsync(DateTime from, DateTime to)
        {
            return await this.Entities
                .Where(x => x.Date >= from && x.Date <= to)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Add(SaleEntity entity)
        {
            this.Entities.Add(entity);
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            await this.Context
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Dictionary<DateTime, DailySalesReportEntryEntity>> GenerateDailySalesReportAsync(DateTime from, DateTime to)
        {
            return (await this.Entities
                .Where(x => x.Date >= from && x.Date <= to)
                .GroupBy(x => x.Date)
                .OrderBy(x => x.Key)
                .Select(x => new
                {
                    Date = x.Key,
                    Amount = x.Sum(x => x.Amount),
                })
                .ToListAsync()
                .ConfigureAwait(false))
                .ToDictionary(
                    x => x.Date,
                    x => new DailySalesReportEntryEntity()
                    {
                        Date = x.Date,
                        Amount = x.Amount,
                    });
        }
    }
}
