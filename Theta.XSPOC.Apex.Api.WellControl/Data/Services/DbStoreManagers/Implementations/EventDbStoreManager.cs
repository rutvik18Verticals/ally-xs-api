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
    public class EventDbStoreManager : DbStoreManagerBase, IDbStoreManager
    {

        #region Private Fields

        private readonly IEventsDbStore _eventDbStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Manages Db Store dependencies in order to persist domain objects to the database store.
        /// </summary>
        /// <param name="eventsDbStore">A <seealso cref="IEventsDbStore"/> implementation capable of persisting
        /// <seealso cref="EventModel"/> models to storage.</param>
        /// <param name="loggerFactory">A <seealso cref="IThetaLoggerFactory"/> that can provide loggers.</param>
        /// <param name="configuration"></param>
        public EventDbStoreManager(IEventsDbStore eventsDbStore, IThetaLoggerFactory loggerFactory,
            IConfiguration configuration) : base(loggerFactory, configuration)
        {
            _eventDbStore = eventsDbStore ?? throw new ArgumentNullException(nameof(eventsDbStore));
        }

        #endregion

        #region IDbStoreManager Implementation

        /// <summary>
        /// The responsibility of the <seealso cref="EventDbStoreManager"/> is updating <seealso cref="EventModel"/> data.
        /// </summary>
        public string Responsibility { get; } = TableNames.tblEvents.ToString();

        /// <summary>
        /// Manages one or more DbStores to asynchronously update the underlying MongoDB data store.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
        public async Task<DbStoreResult> UpdateAsync(string payload, string correlationId)
        {
            return await UpdateAsync<UpdatePayload, EventModel>(payload, correlationId);
        }

        #endregion

        #region Overridden hook methods

        /// <summary>
        /// Maps a payload to a <seealso cref="EventModel"/> document.
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
                throw new NotSupportedException("payloads not supported by EventModelDbStore.");
            }

            var eventData = payload as UpdatePayload;

            var mappedResult = EventsPayloadMapper.Map(eventData);

            return mappedResult as TDocument;
        }

        /// <summary>
        /// Updates the underlying MongoDB store with the <paramref name="document"/>.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document that should be saved.</typeparam>
        /// <param name="document">The document to save.</param>
        /// <returns>Whether the document was saved successfully or not.</returns>
        /// <exception cref="NotSupportedException">
        /// When the <paramref name="document"/> is not of type <seealso cref="EventModel"/>.
        /// </exception>
        /// <exception cref="ArgumentMemberException">
        /// When the CustomerId on the <seealso cref="EventModel"/> is null.
        /// </exception>
        /// <exception cref="ArgumentMemberException">
        /// When the NodeId on the <seealso cref="EventModel"/> is null.
        /// </exception>
        protected override async Task<bool> UpdateDbStore<TDocument>(TDocument document)
        {
            var logger = LoggerFactory.Create(LoggingModel.MongoDataStore);

            if (typeof(TDocument) != typeof(EventModel))
            {
                throw new NotSupportedException("EventDbStore only supports updating events documents.");
            }

            var eventData = document as EventModel;

            if (eventData?.NodeId == null)
            {
                logger.Write(Level.Error, "EventDbStoreManager DB Store Manager - Missing Node Id for event.");

                throw new ArgumentMemberException(nameof(eventData), nameof(eventData.NodeId));
            }
            if (eventData?.EventTypeId == null)
            {
                logger.Write(Level.Error, "EventDbStoreManager DB Store Manager - Missing EventTypeId Id for event.");

                throw new ArgumentMemberException(nameof(eventData), nameof(eventData.NodeId));
            }

            return await _eventDbStore.UpdateAsync(eventData);
        }

        #endregion

    }
}