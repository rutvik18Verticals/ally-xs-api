using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Contracts;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Theta.XSPOC.Apex.Api.WellControl.Services
{
    /// <summary>
    /// This class is the implementation of <seealso cref="IWellEnableDisableService"/>.
    /// for  processing data updates service.
    /// </summary>
    public class WellEnableDisableService : IWellEnableDisableService
    {

        #region Private Fields

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IPublishMessage<ProcessDataUpdateContract> _publishMessage;
        private readonly IPublishMessage<ProcessDataUpdateContract> _publishMessageToComms;
        private readonly INodeMaster _nodeMaster;
        private readonly IConfiguration _configuration;

        #endregion

        #region Private Constants

        private const string BASE_ROUTE_KEY = "Edge.Comms.Config.Update";

        #endregion

        #region Constructor

        /// <summary>
        /// This class is the implementation of <seealso cref="IWellEnableDisableService"/>..
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="publishMessage">The publish message.</param>
        /// <param name="nodeMaster">The node master service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publishMessage"/> does not contain PublishStoreUpdateDataToLegacyDBStore Responsibility
        /// or
        /// <paramref name="publishMessage"/> does not contain PublishStoreUpdateDataToCommsWrapper Responsibility
        /// or
        /// <paramref name="loggerFactory"/> is null
        /// or
        /// <paramref name="publishMessage"/> is null
        /// or
        /// <paramref name="nodeMaster"/> is null
        /// or
        /// <paramref name="configuration"/> is null.
        /// </exception>
        public WellEnableDisableService(IThetaLoggerFactory loggerFactory,
            IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>> publishMessage,
            INodeMaster nodeMaster, IConfiguration configuration)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _publishMessage = publishMessage.FirstOrDefault(m => m.Responsibility == Responsibility.PublishStoreUpdateDataToLegacyDBStore) ??
                throw new ArgumentNullException(nameof(publishMessage));
            _publishMessageToComms = publishMessage
                .FirstOrDefault(m => m.Responsibility == Responsibility.PublishStoreUpdateDataToCommsWrapper) ??
                throw new ArgumentNullException(nameof(publishMessage));
            _nodeMaster = nodeMaster ?? throw new ArgumentNullException(nameof(nodeMaster));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion

        #region IConsumerTransaction Implementation

        /// <summary>
        /// Pass asset guid, enabled, dataCollection and disableCode to service.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="enabled">The Enabled.</param>
        /// <param name="dataCollection">The data Collection.</param>
        /// <param name="disableCode">The disable Code.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>Task<seealso cref="ConsumerBaseAction" />></returns>
        public async Task<ConsumerBaseAction> WellEnableDisableAsync(Guid assetId, string enabled, string dataCollection,
            string disableCode, string socketId)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellControl);

            var nodeId = _nodeMaster.GetNodeIdByAsset(assetId, socketId);

            UpdatePayload payload = new UpdatePayload()
            {
                Key = new List<UpdateColumnValuePair>()
                    {
                        new UpdateColumnValuePair()
                        {
                            Column = "NodeID",
                            Value = nodeId,
                        }
                    },
                Data = new List<UpdateColumnValuePair>()
                    {
                        new UpdateColumnValuePair { Column = "NodeID", Value = nodeId },
                        new UpdateColumnValuePair { Column = "Enabled", Value = enabled },
                        new UpdateColumnValuePair { Column = "DataCollection", Value = dataCollection },
                        new UpdateColumnValuePair { Column = "DisableCode", Value = disableCode },
                        new UpdateColumnValuePair { Column = "UserId", Value = "username" },
                    }
            };

            var dataUpdateEvent = new DataUpdateEvent
            {
                Action = "update",
                PayloadType = "WellEnableDisable.update",
                Payload = JsonConvert.SerializeObject(payload),
            };

            var correlationId = socketId;
            var request = new WithCorrelationId<DataUpdateEvent>(correlationId, dataUpdateEvent);
            var dataValue = request?.Value;

            if (dataValue?.PayloadType == null || dataValue.Payload == null)
            {
                logger.WriteCId(Level.Warn, "Could not extract transaction from message: data incomplete",
                    correlationId);

                return ConsumerBaseAction.Reject;
            }

            if (dataValue.PayloadType != "WellEnableDisable.update")
            {
                logger.WriteCId(Level.Warn, $"{dataValue.PayloadType} is not supported", correlationId);

                return ConsumerBaseAction.Reject;
            }

            if (dataValue.Action == null ||
                (dataValue.Action.ToLower() == "insert" || dataValue.Action.ToLower() == "update") == false)
            {
                logger.WriteCId(Level.Warn, $"Received undefined action {dataValue.Action}", correlationId);

                return ConsumerBaseAction.Reject;
            }

            var shouldProcess = true;

            NodeUpdatePayload updatePayload = null;
            EventLogEventPayload eventLogPayload = null;
            try
            {
                updatePayload = JsonSerializer.Deserialize<NodeUpdatePayload>(dataValue.Payload);

                eventLogPayload = MapEventLogPayload(nodeId, enabled);
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Warn, $"Error processing {dataValue.Action} for {dataValue.PayloadType}", ex, correlationId);

                shouldProcess = false;
            }

            if (!shouldProcess)
            {
                return ConsumerBaseAction.Reject;
            }

            logger.WriteCNId(Level.Trace, "Starting ProcessTransactionAsync for " +
                                          $"transaction id {updatePayload.Key}", correlationId,
                updatePayload.Key.ToString());

            var result = await SendMessageAsync(request, updatePayload, eventLogPayload, assetId);

            return result;
        }

        #endregion

        #region Private Methods

        private async Task<ConsumerBaseAction> SendMessageAsync(WithCorrelationId<DataUpdateEvent> request,
            NodeUpdatePayload updatePayload, EventLogEventPayload eventLogPayload, Guid assetId)
        {
            if (!_nodeMaster.TryGetPortIdByAssetGUID(assetId, out var portId, request.CorrelationId))
            {
                return ConsumerBaseAction.Reject;
            }

            var message = new ProcessDataUpdateContract()
            {
                Action = request.Value.Action,
                PayloadType = request.Value.PayloadType,
                Payload = JsonSerializer.Serialize(updatePayload)
            };
            var messageWithCorrelationId =
                new WithCorrelationId<ProcessDataUpdateContract>(request.CorrelationId, message);

            await _publishMessage.PublishMessageAsync(messageWithCorrelationId);

            var eventLogMessage = new ProcessDataUpdateContract()
            {
                Action = "ïnsert",
                PayloadType = "event.logevent",
                Payload = JsonSerializer.Serialize(eventLogPayload)
            };
            var eventLogMessageWithCorrelationId =
                new WithCorrelationId<ProcessDataUpdateContract>(request.CorrelationId, eventLogMessage);

            await _publishMessage.PublishMessageAsync(eventLogMessageWithCorrelationId);

            var route = _configuration.GetSection("TopicExchanges:DataStoreUpdatesCommBaseRoutingKey").Value ??
                BASE_ROUTE_KEY;

            var commMessage = new ProcessDataUpdateContract()
            {
                Action = request.Value.Action,
                PayloadType = "tblNodeMaster",
                Payload = JsonSerializer.Serialize(updatePayload)
            };

            var commMessageWithCorrelationId = new WithCorrelationId<ProcessDataUpdateContract>(request.CorrelationId, commMessage);
            await _publishMessageToComms.PublishMessageAsync(commMessageWithCorrelationId, $"{route}.{portId}");

            return ConsumerBaseAction.Success;
        }

        private EventLogEventPayload MapEventLogPayload(string nodeId, string enabled)
        {
            return new EventLogEventPayload
            {
                NodeId = nodeId,
                EventTypeId = (int)EventType.EnableDisable,
                Description = int.Parse(enabled) == 1 ? "Enabled for Scanning" : "Disabled",
                Date = DateTime.UtcNow,
                UserId = "username", //TODO capture userid
            };
        }

        #endregion

    }
}
