using System;

namespace DevOpsChallenge.SalesApi.Database.Models
{
    /// <summary>
    /// An entry in the daily sales report.
    /// </summary>
    public class DailySalesReportEntryEntity
    {
        /// <summary>
        /// Gets or sets the date of the aggregate entry.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount on the date.
        /// </summary>
        public double Amount { get; set; }
    }
}
