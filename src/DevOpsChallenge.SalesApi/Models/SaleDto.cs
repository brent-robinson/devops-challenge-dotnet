using System.Text.Json.Serialization;

namespace DevOpsChallenge.SalesApi.Models
{
    /// <summary>
    /// DTO of a sale.
    /// </summary>
    public class SaleDto
    {
        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }
}
