using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Business.Services
{
    /// <summary>
    /// Service for generating reports.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Gets the daily sales report.
        /// </summary>
        /// <param name="from">The from date of the sales.</param>
        /// <param name="to">The to date of the sales.</param>
        /// <exception cref="SaleExceptions.FromDateRequired">From date is required. It cannot be default(DateTime).</exception>
        /// <exception cref="SaleExceptions.ToDateRequired">To date is required. It cannot be default(DateTime).</exception>
        /// <exception cref="SaleExceptions.ToDateMustBeAfterFromDate">The "to" date must be after the "from" date.</exception>
        /// <exception cref="SaleExceptions.DateRangeMustNoMoreThanAYear">The search range must be no more than a year.</exception>
        /// <returns>The aggregate details of the sales recorded in the range.</returns>
        Task<DailySalesReportDetails> GetDailySalesReportAsync(DateTime from, DateTime to);
    }
}
