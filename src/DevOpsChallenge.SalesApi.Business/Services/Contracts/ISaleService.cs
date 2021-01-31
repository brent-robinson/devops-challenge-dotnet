using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Business.Services
{
    /// <summary>
    /// Service for managing sale records.
    /// </summary>
    public interface ISaleService
    {
        /// <summary>
        /// Record a sale.
        /// </summary>
        /// <param name="transactionId">The transaction ID of the sale.</param>
        /// <param name="date">The date of the sale.</param>
        /// <param name="amount">The amount of the same.</param>
        /// <param name="notes">Optionally, any notes of the sale.</param>
        /// <exception cref="SaleExceptions.TransactionIdRequired">Transaction ID is required. It cannot be null or whitespace.</exception>
        /// <exception cref="SaleExceptions.TransactionIdInvalid">Transaction ID is invalid. It must be no more than 32 characters and only contain numbers/letters.</exception>
        /// <exception cref="SaleExceptions.AlreadyExists">Another sale with the same transaction ID already exists.</exception>
        /// <exception cref="SaleExceptions.DateRequired">Date required. It cannot be default(DateTime).</exception>
        /// <exception cref="SaleExceptions.DateInvalid">Date cannot be in the future.</exception>
        /// <exception cref="SaleExceptions.AmountInvalid">Amount cannot be less than zero.</exception>
        /// <exception cref="SaleExceptions.NotesInvalid">Notes must be no more than 256 characters.</exception>
        /// <returns>Details of the recorded sale.</returns>
        Task<SaleDetails> AddSaleAsync(string transactionId, DateTime date, double amount, string notes);

        /// <summary>
        /// Get a sale by its transaction ID.
        /// </summary>
        /// <param name="transactionId">The transction ID.</param>
        /// <exception cref="SaleExceptions.TransactionIdRequired">Transaction ID is required. It cannot be null or whitespace.</exception>
        /// <exception cref="SaleExceptions.TransactionIdInvalid">Transaction ID is invalid. It must be no more than 32 characters and only contain numbers/letters.</exception>
        /// <exception cref="SaleExceptions.NotFound">A transaction with the transaction ID was not found.</exception>
        /// <returns>Details of the recorded sale.</returns>
        Task<SaleDetails> GetSaleAsync(string transactionId);

        /// <summary>
        /// Get all sales between a date range (inclusive).
        /// </summary>
        /// <param name="from">The from date of the sales.</param>
        /// <param name="to">The to date of the sales.</param>
        /// <exception cref="SaleExceptions.FromDateRequired">From date is required. It cannot be default(DateTime).</exception>
        /// <exception cref="SaleExceptions.ToDateRequired">To date is required. It cannot be default(DateTime).</exception>
        /// <exception cref="SaleExceptions.ToDateMustBeAfterFromDate">The "to" date must be after the "from" date.</exception>
        /// <exception cref="SaleExceptions.DateRangeMustNoMoreThanAYear">The search range must be no more than a year.</exception>
        /// <returns>Details of the recorded sales during the date range.</returns>
        Task<IList<SaleDetails>> GetSalesAsync(DateTime from, DateTime to);
    }
}
