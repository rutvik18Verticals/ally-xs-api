using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.Exceptions;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers.Implementations
{
    /// <summary>
    /// Responsible for persisting a payload tagged with the given Responsibility. Orchestrates one or more DbStores to
    /// accomplish this.
    /// </summary>
    public class TransactionsDbStoreManager : DbStoreManagerBase, IDbStoreManager
    {

        #region Private Fields

        private readonly ITransactionsDbStore _transactionsDbStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Manages Db Store dependencies in order to persist domain objects to the database store.
        /// </summary>
        /// <param name="transactionsDbStore">A <seealso cref="ITransactionsDbStore"/> implementation capable of persisting
        /// <seealso cref="TransactionsModel"/> models to storage.</param>
        /// <param name="loggerFactory">A <seealso cref="IThetaLoggerFactory"/> that can provide loggers.</param>
        /// <param name="configuration"></param>
        public TransactionsDbStoreManager(ITransactionsDbStore transactionsDbStore, IThetaLoggerFactory loggerFactory,
            IConfiguration configuration) : base(loggerFactory, configuration)
        {
            _transactionsDbStore = transactionsDbStore ?? throw new ArgumentNullException(nameof(transactionsDbStore));
        }

        #endregion

        #region IDbStoreManager Implementation

        /// <summary>
        /// The responsibility of the <seealso cref="TransactionsDbStoreManager"/> is updating <seealso cref="TransactionsModel"/> data.
        /// </summary>
        public string Responsibility { get; } = TableNames.tblTransactions.ToString();

        /// <summary>
        /// Manages one or more DbStores to asynchronously update the underlying MongoDB data store.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
        public async Task<DbStoreResult> UpdateAsync(string payload, string correlationId)
        {
            return await UpdateAsync<UpdatePayload, TransactionsModel>(payload, correlationId);
        }

        #endregion

        #region Overridden hook methods

        /// <summary>
        /// Maps a payload to a <seealso cref="TransactionsModel"/> document.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TDocument">The type of the mapped document.</typeparam>
        /// <param name="payload">The received payload.</param>
        /// <returns>The mapped document.</returns>
        /// <exception cref="NotSupportedException">
        /// When the <paramref name="payload"/> is not of type <seealso cref="UpdatePayload"/>.
        /// </exception>
        protected override TDocument Map<TPayload, TDocument>(TPayload payload)
        {
            if (typeof(TPayload) != typeof(UpdatePayload))
            {
                throw new NotSupportedException("TransactionsModelDbStore only supports updating UpdatePayload payloads.");
            }

            var transactions = payload as UpdatePayload;

            var mappedResult = TransactionPayloadMapper.Map(transactions);

            return mappedResult as TDocument;
        }

        /// <summary>
        /// Updates the underlying MongoDB store with the <paramref name="document"/>.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document that should be saved.</typeparam>
        /// <param name="document">The document to save.</param>
        /// <returns>Whether the document was saved successfully or not.</returns>
        /// <exception cref="NotSupportedException">
        /// When the <paramref name="document"/> is not of type <seealso cref="TransactionsModel"/>.
        /// </exception>
        /// <exception cref="ArgumentMemberException">
        /// When the CustomerId on the <seealso cref="TransactionsModel"/> is null.
        /// </exception>
        /// <exception cref="ArgumentMemberException">
        /// When the transactionId on the <seealso cref="TransactionsModel"/> is null.
        /// </exception>
        protected override async Task<bool> UpdateDbStore<TDocument>(TDocument document)
        {
            var logger = LoggerFactory.Create(LoggingModel.MongoDataStore);

            if (typeof(TDocument) != typeof(TransactionsModel))
            {
                throw new NotSupportedException("TransactionsDbStore only supports updating Asset documents.");
            }

            var transactions = document as TransactionsModel;

            if (transactions?.TransactionID == null)
            {
                logger.Write(Level.Error, "transactions DB Store Manager - Missing Transaction Id for asset.");

                throw new ArgumentMemberException(nameof(transactions), nameof(transactions.TransactionID));
            }

            return await _transactionsDbStore.UpdateAsync(transactions);
        }

        #endregion

    }
}