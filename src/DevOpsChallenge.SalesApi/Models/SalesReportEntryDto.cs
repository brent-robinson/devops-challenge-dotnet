using System.Text.Json.Serialization;

namespace DevOpsChallenge.SalesApi.Models
{
    /// <summary>
    /// DTO for the data points for the sales report.
    /// </summary>
    public class SalesReportEntryDto
    {
        /// <summary>
        /// Gets or sets the label of the data point.
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the value of the data point.
        /// </summary>
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
