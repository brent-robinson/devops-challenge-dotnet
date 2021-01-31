using DevOpsChallenge.SalesApi.Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Database.Repositories
{
    /// <summary>
    /// Repository for sales.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Find a sale by its transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID of the sale.</param>
        /// <returns>The sale if found, else NULL.</returns>
        Task<SaleEntity> FindAsync(string transactionId);

        /// <summary>
        /// Get all sales in a date range (inclusive).
        /// </summary>
        /// <param name="from">The inclusive "from" date to include sales.</param>
        /// <param name="to">The inclusive "to" date to include sales.</param>
        /// <returns>The sales.</returns>
        Task<IEnumerable<SaleEntity>> GetAsync(DateTime from, DateTime to);

        /// <summary>
        /// Add an sale to the repository.
        /// </summary>
        /// <param name="entity">The sale to add.</param>
        void Add(SaleEntity entity);

        /// <summary>
        /// Commit any add/update/delete operations to the repository.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Get the entries of the daily sales report.
        /// </summary>
        /// <remarks>
        /// Aggregates sales by date.
        /// </remarks>
        /// <param name="from">The inclusive "from" date to include sales.</param>
        /// <param name="to">The inclusive "to" date to include sales.</param>
        /// <returns>The entries for the report.</returns>
        Task<Dictionary<DateTime, DailySalesReportEntryEntity>> GenerateDailySalesReportAsync(DateTime from, DateTime to);
    }
}
