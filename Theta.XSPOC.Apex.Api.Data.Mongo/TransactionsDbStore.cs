using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{

    /// <summary>
    /// Manages access to the <seealso cref="TransactionsDbStore"/> collection.
    /// </summary>
    public class TransactionsDbStore : ITransactionsDbStore
    {

        #region Private Fields

        private readonly IMongoCollection<TransactionsModel> _transactionCollection;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <seealso cref="TransactionsDbStore"/>.
        /// </summary>
        /// <param name="transactionCollection">
        /// The <seealso cref="IMongoCollection{TDocument}"/> that this store is responsible for.
        /// </param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// If the 
        /// </exception>
        public TransactionsDbStore(IMongoCollection<TransactionsModel> transactionCollection, IThetaLoggerFactory loggerFactory)
        {
            _transactionCollection = transactionCollection ?? throw new ArgumentNullException(nameof(transactionCollection));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        }

        #endregion

        #region ICompanyDbStore Implementation

        /// <summary>
        /// Persists the given <param name="transactions"/> in the data store. If exits will get updated, 
        /// if not data gets created.
        /// </summary>
        /// <returns><c>true</c> if the persistence succeeded, false otherwise.</returns>
        public async Task<bool> UpdateAsync(TransactionsModel transactions)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            try
            {
                if (transactions == null)
                {
                    return false;
                }

                var existing = await _transactionCollection.Find(x => x.TransactionID ==
                    transactions.TransactionID).FirstOrDefaultAsync();

                if (existing == null)
                {
                    await _transactionCollection.InsertOneAsync(transactions);

                    return true;
                }

                var filter = Builders<TransactionsModel>.Filter.Eq
                    (t => t.TransactionID, existing.TransactionID);

                var update = Builders<TransactionsModel>.Update
                    .Set(t => t.Result, transactions.Result)
                    .Set(t => t.DateProcess, transactions.DateProcess)
                    .Set(t => t.Output, transactions.Output ?? null);

                await _transactionCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, ex.Message);

                return false;
            }

            return true;
        }

        #endregion

    }
}
