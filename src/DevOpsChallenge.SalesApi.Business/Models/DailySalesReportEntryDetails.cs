using System;

namespace DevOpsChallenge.SalesApi.Business.Models
{
    /// <summary>
    /// Details of an entry in the daily sales report.
    /// </summary>
    public class DailySalesReportEntryDetails
    {
        /// <summary>
        /// Gets or sets the date for the entry.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the amount for the date.
        /// </summary>
        public double Amount { get; set; }
    }
}
