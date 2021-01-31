using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Business.Services;
using DevOpsChallenge.SalesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Controllers
{
    /// <summary>
    /// Controller for sale operations.
    /// </summary>
    [Route("api/sales")]
    public class SalesController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesController"/> class.
        /// </summary>
        /// <param name="saleService">The sale service.</param>
        public SalesController(ISaleService saleService)
        {
            this.SaleService = saleService;
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        protected ISaleService SaleService { get; }

        /// <summary>
        /// Get a sale record.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <response code="200">The details of the sale record.</response>
        /// <response code="400">
        /// Unable to process the request. An error code will indicate the cause.
        /// * TransactionIdRequired: The transaction ID was missing.
        /// * TransactionIdInvalid: The transaction ID was malformed. It should be up to 32 characters and only contain letters and numbers.
        /// </response>
        /// <response code="404">A sale record was not found with the transaction ID.</response>
        /// <returns>A HTTP response.</returns>
        [HttpGet("{transactionId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string transactionId)
        {
            try
            {
                // Lookup sale
                SaleDetails sale = await this.SaleService.GetSaleAsync(transactionId).ConfigureAwait(false);

                // Map to DTO model
                return this.Ok(sale.ToDto());
            }
            catch (SaleExceptions.NotFound)
            {
                return this.NotFound();
            }
            catch (SaleExceptions.SaleException ex) when (
                ex is SaleExceptions.TransactionIdRequired ||
                ex is SaleExceptions.TransactionIdInvalid)
            {
                return this.BadRequest(new ErrorDto()
                {
                    Code = ex.GetType().Name,
                });
            }
        }

        /// <summary>
        /// List sale records between certain dates.
        /// </summary>
        /// <param name="from">The inclusive from date.</param>
        /// <param name="to">The inclusive to date.</param>
        /// <response code="200">The details of the sale records.</response>
        /// <response code="400">
        /// Unable to process the request. An error code will indicate the cause.
        /// * FromDateRequired: The "from" date is required.
        /// * ToDateRequired: The "to" date is required.
        /// * ToDateMustBeAfterFromDate: The "to" date must be later in time than the "from" date.
        /// * DateRangeMustNoMoreThanAYear: The difference between the "to" and "from" date cannot be more than a year.
        /// </response>
        /// <returns>A HTTP response.</returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SaleSummaryDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            try
            {
                // Lookup sales
                IList<SaleDetails> sales = await this.SaleService.GetSalesAsync(from, to).ConfigureAwait(false);

                // Map to DTO model
                return this.Ok(sales.ToDto());
            }
            catch (SaleExceptions.SaleException ex) when (
                ex is SaleExceptions.FromDateRequired ||
                ex is SaleExceptions.ToDateRequired ||
                ex is SaleExceptions.ToDateMustBeAfterFromDate ||
                ex is SaleExceptions.DateRangeMustNoMoreThanAYear)
            {
                return this.BadRequest(new ErrorDto()
                {
                    Code = ex.GetType().Name,
                });
            }
        }

        /// <summary>
        /// Add a new sale record.
        /// </summary>
        /// <param name="dto">The DTO containing the details of the sale.</param>
        /// <response code="201">The details of the created sale record.</response>
        /// <response code="400">
        /// Unable to process the request. An error code will indicate the cause.
        /// * TransactionIdRequired: The transaction ID was missing.
        /// * TransactionIdInvalid: The transaction ID was malformed. It should be up to 32 characters and only contain letters and numbers.
        /// * DateRequired: The date was missing.
        /// * DateInvalid: The date must not be in the future.
        /// * AmountRequired: The amount was missing.
        /// * AmountInvalid: The amount cannot be less than zero.
        /// * NotesInvalid: The notes must not be more than 256 characters.
        /// </response>
        /// <response code="409">A sale with the same transaction ID already exists. The sale has not been recorded..</response>
        /// <returns>A HTTP response.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddAsync(
            [FromBody] SaleDto dto)
        {
            try
            {
                // Parse DTO
                if (string.IsNullOrWhiteSpace(dto?.Date))
                {
                    throw new SaleExceptions.DateRequired();
                }

                if (!DateTime.TryParseExact(dto?.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    throw new SaleExceptions.DateInvalid();
                }

                // Add sale
                SaleDetails sale = await this.SaleService.AddSaleAsync(
                    dto?.TransactionId,
                    date,
                    dto?.Amount ?? throw new SaleExceptions.AmountRequired(),
                    dto?.Notes).ConfigureAwait(false);

                // Map to DTO model
                string actionName = nameof(this.GetAsync);
                if (actionName.EndsWith("Async"))
                {
                    actionName = actionName[0..^5];
                }

                return this.CreatedAtAction(
                    "Get",
                    new { transactionId = sale.TransactionId },
                    sale.ToDto());
            }
            catch (SaleExceptions.AlreadyExists)
            {
                return this.Conflict();
            }
            catch (SaleExceptions.SaleException ex) when (
                ex is SaleExceptions.TransactionIdRequired ||
                ex is SaleExceptions.TransactionIdInvalid ||
                ex is SaleExceptions.DateRequired ||
                ex is SaleExceptions.DateInvalid ||
                ex is SaleExceptions.AmountRequired ||
                ex is SaleExceptions.AmountInvalid ||
                ex is SaleExceptions.NotesInvalid)
            {
                return this.BadRequest(new ErrorDto()
                {
                    Code = ex.GetType().Name,
                });
            }
        }
    }
}
