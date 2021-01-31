using System;

namespace DevOpsChallenge.SalesApi.Business.Models
{
    /// <summary>
    /// Details of aggregate daily sales.
    /// </summary>
    public class DailySalesReportDetails
    {
        /// <summary>
        /// Gets or sets the start date of the report.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the report.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the data points of the report.
        /// </summary>
        public DailySalesReportEntryDetails[] Entries { get; set; }
    }
}
