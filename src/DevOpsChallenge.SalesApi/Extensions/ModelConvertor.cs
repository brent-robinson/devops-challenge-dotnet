using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace DevOpsChallenge.SalesApi
{
    /// <summary>
    /// Converts business models to DTOs.
    /// </summary>
    internal static class ModelConvertor
    {
        /// <summary>
        /// Convert business model to DTO.
        /// </summary>
        /// <param name="model">The business model.</param>
        /// <returns>The DTO.</returns>
        public static SaleDto ToDto(this SaleDetails model)
        {
            if (model == null)
            {
                return null;
            }

            return new SaleDto()
            {
                TransactionId = model.TransactionId,
                Date = model.Date.ToString("yyyy-MM-dd"),
                Amount = model.Amount,
                Notes = model.Notes,
            };
        }

        /// <summary>
        /// Convert business model to DTO.
        /// </summary>
        /// <param name="models">The business models.</param>
        /// <returns>The DTO.</returns>
        public static SaleSummaryDto[] ToDto(this IList<SaleDetails> models)
        {
            if (models == null)
            {
                return null;
            }

            return models.Select(model => new SaleSummaryDto()
            {
                TransactionId = model.TransactionId,
                Date = model.Date.ToString("yyyy-MM-dd"),
                Amount = model.Amount,
            }).ToArray();
        }

        /// <summary>
        /// Convert business model to DTO.
        /// </summary>
        /// <param name="model">The business model.</param>
        /// <returns>The DTO.</returns>
        public static SalesReportDto ToDto(this DailySalesReportDetails model)
        {
            if (model == null)
            {
                return null;
            }

            return new SalesReportDto()
            {
                StartDate = model.StartDate.ToString("yyyy-MM-dd"),
                EndDate = model.EndDate.ToString("yyyy-MM-dd"),
                GroupBy = SalesReportGroupByOption.Day,
                XAxisLabel = "Date",
                YAxisLabel = "Total sales",
                Entries = model.Entries.Select(x => new SalesReportEntryDto()
                {
                    Label = x.Date.ToString("yyyy-MM-dd"),
                    Value = x.Amount,
                }).ToArray(),
            };
        }
    }
}
