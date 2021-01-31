using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Database.Models;

namespace DevOpsChallenge.SalesApi.Business
{
    /// <summary>
    /// Convert from database models to business models.
    /// </summary>
    internal static class ModelConvertor
    {
        /// <summary>
        /// Convert database model to business model.
        /// </summary>
        /// <param name="db">The database model.</param>
        /// <returns>The business model.</returns>
        public static SaleDetails ToBusinessModel(this SaleEntity db)
        {
            if (db == null)
            {
                return null;
            }

            return new SaleDetails()
            {
                TransactionId = db.TransactionId,
                Date = db.Date,
                Amount = db.Amount,
                Notes = db.Notes,
            };
        }
    }
}
