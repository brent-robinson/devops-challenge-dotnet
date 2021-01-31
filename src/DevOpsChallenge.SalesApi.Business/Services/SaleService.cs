using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Database.Models;
using DevOpsChallenge.SalesApi.Database.Repositories;
using EnsureThat;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Business.Services
{
    /// <summary>
    /// Service for managing sale records.
    /// </summary>
    public class SaleService : ISaleService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleService"/> class.
        /// </summary>
        /// <param name="repository">The sale repository.</param>
        /// <param name="logger">The logger.</param>
        public SaleService(
            ISaleRepository repository,
            ILogger<SaleService> logger)
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
        protected ILogger<SaleService> Logger { get; }

        /// <inheritdoc />
        public async Task<SaleDetails> AddSaleAsync(string transactionId, DateTime date, double amount, string notes)
        {
            // Validate
            Ensure.String.IsNotNullOrWhiteSpace(transactionId, optsFn: o => o.WithException(new SaleExceptions.TransactionIdRequired()));
            Ensure.String.HasLengthBetween(transactionId, 1, 32, optsFn: o => o.WithException(new SaleExceptions.TransactionIdInvalid()));
            Ensure.String.IsAllLettersOrDigits(transactionId, optsFn: o => o.WithException(new SaleExceptions.TransactionIdInvalid()));

            Ensure.Any.IsNotDefault(date, optsFn: o => o.WithException(new SaleExceptions.DateRequired()));
            Ensure.Comparable.IsLte(date, DateTime.UtcNow, optsFn: o => o.WithException(new SaleExceptions.DateInvalid()));

            Ensure.Comparable.IsGte(amount, 0, optsFn: o => o.WithException(new SaleExceptions.AmountInvalid()));

            Ensure.String.HasLengthBetween(notes, 0, 256, optsFn: o => o.WithException(new SaleExceptions.NotesInvalid()));

            // Check for existing transaction with the same ID
            SaleEntity sale = await this.Repository.FindAsync(transactionId).ConfigureAwait(false);
            if (sale != null)
            {
                throw new SaleExceptions.AlreadyExists();
            }

            // Record transaction
            sale = new SaleEntity()
            {
                TransactionId = transactionId,
                Date = date.Date,
                Amount = amount,
                Notes = notes,
            };
            this.Repository.Add(sale);
            await this.Repository.SaveChangesAsync().ConfigureAwait(false);

            // Convert to business model
            return sale.ToBusinessModel();
        }

        /// <inheritdoc />
        public async Task<SaleDetails> GetSaleAsync(string transactionId)
        {
            // Validate
            Ensure.String.IsNotNullOrWhiteSpace(transactionId, optsFn: o => o.WithException(new SaleExceptions.TransactionIdRequired()));
            Ensure.String.HasLengthBetween(transactionId, 1, 32, optsFn: o => o.WithException(new SaleExceptions.TransactionIdInvalid()));
            Ensure.String.IsAllLettersOrDigits(transactionId, optsFn: o => o.WithException(new SaleExceptions.TransactionIdInvalid()));

            // Lookup sale
            SaleEntity sale = await this.Repository.FindAsync(transactionId).ConfigureAwait(false);
            if (sale == null)
            {
                throw new SaleExceptions.NotFound();
            }

            // Convert to business model
            return sale.ToBusinessModel();
        }

        /// <inheritdoc />
        public async Task<IList<SaleDetails>> GetSalesAsync(DateTime from, DateTime to)
        {
            // Validate
            Ensure.Any.IsNotDefault(from, optsFn: o => o.WithException(new SaleExceptions.FromDateRequired()));
            Ensure.Any.IsNotDefault(to, optsFn: o => o.WithException(new SaleExceptions.ToDateRequired()));

            Ensure.Comparable.IsGte(to, from, optsFn: o => o.WithException(new SaleExceptions.ToDateMustBeAfterFromDate()));
            Ensure.Comparable.IsLte(to - from, TimeSpan.FromDays(366), optsFn: o => o.WithException(new SaleExceptions.DateRangeMustNoMoreThanAYear()));

            // Ensure only date component is used
            from = from.Date;
            to = to.Date;

            // Query database
            IEnumerable<SaleEntity> sales = await this.Repository.GetAsync(from, to).ConfigureAwait(false);

            // Convert to business model
            return sales.Select(x => x.ToBusinessModel()).ToList();
        }
    }
}
