using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Database.Models;
using DevOpsChallenge.SalesApi.Database.Repositories;
using EnsureThat;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Business.Services
{
    /// <summary>
    /// Service for generating reports.
    /// </summary>
    public class ReportService : IReportService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        /// <param name="repository">The sale repository.</param>
        /// <param name="logger">The logger.</param>
        public ReportService(
            ISaleRepository repository,
            ILogger<ReportService> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the sale repository.
        /// </summary>
        protected ISaleRepository Repository { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger<ReportService> Logger { get; }

        /// <inheritdoc />
        public async Task<DailySalesReportDetails> GetDailySalesReportAsync(DateTime from, DateTime to)
        {
            // Validate
            Ensure.Any.IsNotDefault(from, optsFn: o => o.WithException(new SaleExceptions.FromDateRequired()));
            Ensure.Any.IsNotDefault(to, optsFn: o => o.WithException(new SaleExceptions.ToDateRequired()));

            Ensure.Comparable.IsGte(to, from, optsFn: o => o.WithException(new SaleExceptions.ToDateMustBeAfterFromDate()));
            Ensure.Comparable.IsLte(to - from, TimeSpan.FromDays(366), optsFn: o => o.WithException(new SaleExceptions.DateRangeMustNoMoreThanAYear()));

            // Ensure only date component is used
            from = from.Date;
            to = to.Date;

            // Query database (this only returns entries for dates with sales)
            Dictionary<DateTime, DailySalesReportEntryEntity> report = await this.Repository.GenerateDailySalesReportAsync(from, to).ConfigureAwait(false);

            // Convert to business model (all dates inclusive should have an entry)
            List<DailySalesReportEntryDetails> entries = new List<DailySalesReportEntryDetails>();
            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                entries.Add(new DailySalesReportEntryDetails()
                {
                    Date = date,
                    Amount = report.ContainsKey(date) ? report[date].Amount : 0,
                });
            }

            return new DailySalesReportDetails()
            {
                StartDate = from,
                EndDate = to,
                Entries = entries.ToArray(),
            };
        }
    }
}
