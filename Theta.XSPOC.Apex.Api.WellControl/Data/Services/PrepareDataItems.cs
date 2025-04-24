using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.Factories;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services
{
    /// <summary>
    /// needed to persist data to various data store implementations.
    /// </summary>
    public class PrepareDataItems : IPrepareDataItems
    {

        #region Private Fields

        private readonly IPersistenceFactory _persistenceFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Protected Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an instance of the <seealso cref="PrepareDataItems"/>.
        /// </summary>
        /// <param name="persistenceFactory">An implementation of <seealso cref="IPersistenceFactory"/> to provide
        /// the appropriate Db Store Manager to persist non-trend data.</param>
        /// <param name="loggerFactory">The logging factory.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="loggerFactory"/> is null
        /// or
        /// When <paramref name="persistenceFactory"/> is null
        /// </exception>
        public PrepareDataItems(IThetaLoggerFactory loggerFactory,
            IPersistenceFactory persistenceFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _persistenceFactory = persistenceFactory ?? throw new ArgumentNullException(nameof(persistenceFactory));
        }

        #endregion

        /// <summary>
        /// Acts on a <paramref name="messageWithCorrelationId"/> from Rabbit MQ and validate the message, 
        /// do any pre processing of the data and send the data to the db store for operation.
        /// </summary>
        /// <param name="messageWithCorrelationId">The message to process.</param>
        /// <returns>
        /// <seealso cref="ConsumerBaseAction.Success"/> if the performing the operation was successful,
        /// <seealso cref="ConsumerBaseAction.Reject"/> if the contract was bad, and
        /// <seealso cref="ConsumerBaseAction.Reject"/> if performing the operation failed because of a db failure.
        /// </returns>
        public async Task<ConsumerBaseAction> PrepareDataItemsAsync(
            WithCorrelationId<DataUpdateEvent> messageWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.DataStoreService);

            if (messageWithCorrelationId == null)
            {
                logger.Write(Level.Error,
                    $"Prepare data item failed because {nameof(messageWithCorrelationId)} is missing.");

                return ConsumerBaseAction.Reject;
            }

            var correlationId = messageWithCorrelationId.CorrelationId;
            var message = messageWithCorrelationId.Value;

            try
            {
                var dbStoreManager = _persistenceFactory.Create(message.PayloadType);

                var result = await dbStoreManager.UpdateAsync(message.Payload, correlationId);

                if (result?.KindOfError == ErrorType.NotRecoverable)
                {
                    return ConsumerBaseAction.Reject;
                }

                if (result?.KindOfError == ErrorType.LikelyRecoverable)
                {
                    return ConsumerBaseAction.Requeue;
                }

                return ConsumerBaseAction.Success;
            }
            catch (NotSupportedException e)
            {
                logger.WriteCId(Level.Error, "No DbStore was found to handle the request.", e, correlationId);
            }
            catch (Exception e)
            {
                logger.WriteCId(Level.Error, "Failed to obtain a Db Store Manager.", e, correlationId);

                return ConsumerBaseAction.Reject;
            }

            return ConsumerBaseAction.Reject;
        }
    }
}