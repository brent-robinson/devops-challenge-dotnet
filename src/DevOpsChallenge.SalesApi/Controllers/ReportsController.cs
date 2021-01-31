using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Business.Services;
using DevOpsChallenge.SalesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevOpsChallenge.SalesApi.Controllers
{
    /// <summary>
    /// Controller for reports.
    /// </summary>
    [Route("api/reports")]
    public class ReportsController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="reportService">The reportservice.</param>
        public ReportsController(IReportService reportService)
        {
            this.ReportService = reportService;
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        protected IReportService ReportService { get; }

        /// <summary>
        /// Generate the sales report which aggregates sales by day.
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
        [HttpGet("daily-sales")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SalesReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDailySalesReportAsync(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            try
            {
                // Lookup sales
                DailySalesReportDetails report = await this.ReportService.GetDailySalesReportAsync(from, to).ConfigureAwait(false);

                // Map to DTO model
                return this.Ok(report.ToDto());
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
    }
}
