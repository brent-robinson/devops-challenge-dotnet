namespace DevOpsChallenge.SalesApi.Models
{
    /// <summary>
    /// DTO for the sales report.
    /// </summary>
    public class SalesReportDto
    {
        /// <summary>
        /// Gets or sets the start date of the report.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the report.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the aggregation period of the report.
        /// </summary>
        public SalesReportGroupByOption GroupBy { get; set; }

        /// <summary>
        /// Gets or sets the X axis label which represents each entry.
        /// </summary>
        public string XAxisLabel { get; set; }

        /// <summary>
        /// Gets or sets the Y axis label which represents the value of an entry.
        /// </summary>
        public string YAxisLabel { get; set; }

        /// <summary>
        /// Gets or sets the data points of the report.
        /// </summary>
        public SalesReportEntryDto[] Entries { get; set; }
    }
}
