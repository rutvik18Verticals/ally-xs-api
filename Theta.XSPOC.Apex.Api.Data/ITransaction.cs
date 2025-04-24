namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Interface representing operations on a transaction.
    /// </summary>
    public interface ITransaction
    {

        /// <summary>
        /// Determines whether a transaction
        /// with specified <paramref name="transactionId"/> already exists.
        /// </summary>
        /// <param name="transactionId">The transaction id to check.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if an entry with <paramref name="transactionId"/> already exists, false otherwise.</returns>
        bool TransactionIdExists(int transactionId, string correlationId);

    }
}
