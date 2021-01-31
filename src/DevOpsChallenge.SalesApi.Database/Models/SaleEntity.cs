using System;

namespace DevOpsChallenge.SalesApi.Database.Models
{
    /// <summary>
    /// A sale entity. Used to record sales which can be reported on.
    /// </summary>
    public class SaleEntity
    {
        /// <summary>
        /// Gets or sets the ID of the sale in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public string Notes { get; set; }
    }
}
