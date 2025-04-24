using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.WellControl.Contracts;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services;
using Theta.XSPOC.Apex.Api.WellControl.Data.Services.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Utilities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Theta.XSPOC.Apex.Api.WellControl.Services
{
    /// <summary>
    /// This class is the implementation of <seealso cref="IProcessingDataUpdatesService"/>
    /// for  processing data updates service.
    /// </summary>
    public class ProcessingDataUpdatesService : IProcessingDataUpdatesService
    {

        #region Private Fields

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IPublishMessage<ProcessDataUpdateContract, Responsibility> _publishTransaction;
        private readonly IPublishMessage<ProcessDataUpdateContract, Responsibility> _publishLegacyTransaction;
        private readonly IPublishMessage<ProcessDataUpdateContract, Responsibility> _publishLegacyToListner;
        private readonly IPrepareDataItems _dataStore;
        private readonly INodeMaster _nodeMaster;
        private readonly ITransactionPayloadCreator _transactionPayloadCreator;
        private readonly IPortConfigurationStore _portConfigurationStore;
        private readonly INotification _notificationStore;
        private readonly IDateTimeConverter _dateTimeConverter;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs an instance of <seealso cref="ProcessingDataUpdatesService"/>.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="publishMessage">The publish message.</param>
        /// <param name="dataStore">The mongodb data store.</param>
        /// <param name="nodeMaster">The sql data store.</param>
        /// <param name="transactionPayloadCreator">The <seealso cref="ITransactionPayloadCreator"/>.</param>
        /// <param name="portConfigurationStore">The port configuration store.</param>
        /// <param name="notificationStore">The <seealso cref="INotification"/>.</param>
        /// <param name="dateTimeConverter">The <seealso cref="IDateTimeConverter"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null
        /// or
        /// <paramref name="publishMessage"/> is null.
        /// or
        /// <paramref name="dataStore"/> is null.
        /// or
        /// <paramref name="nodeMaster"/> is null.
        /// </exception>
        public ProcessingDataUpdatesService(IThetaLoggerFactory loggerFactory,
            IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>> publishMessage,
            IPrepareDataItems dataStore, INodeMaster nodeMaster, ITransactionPayloadCreator transactionPayloadCreator,
            IPortConfigurationStore portConfigurationStore, INotification notificationStore, IDateTimeConverter dateTimeConverter)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _publishTransaction = publishMessage.FirstOrDefault(m => m.Responsibility == Responsibility.PublishTransationDataToMicroservices) ??
                throw new ArgumentNullException(nameof(publishMessage));
            _publishLegacyTransaction = publishMessage.FirstOrDefault(m => m.Responsibility == Responsibility.PublishStoreUpdateDataToLegacyDBStore) ??
                throw new ArgumentNullException(nameof(publishMessage));
            _publishLegacyToListner = publishMessage.FirstOrDefault(m => m.Responsibility == Responsibility.PublishTransationIdToListener) ??
                throw new ArgumentNullException(nameof(publishMessage));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _nodeMaster = nodeMaster ?? throw new ArgumentNullException(nameof(nodeMaster));
            _transactionPayloadCreator = transactionPayloadCreator ?? throw new ArgumentNullException(nameof(transactionPayloadCreator));
            _portConfigurationStore = portConfigurationStore ?? throw new ArgumentNullException(nameof(portConfigurationStore));
            _notificationStore = notificationStore ?? throw new ArgumentNullException(nameof(notificationStore));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        #region IProcessingDataUpdatesService Implementation

        /// <summary>
        /// Method to process the message from Rabbit MQ and do validation on that message, then process device scanned
        /// data.
        /// </summary>
        /// <param name="messageWithCorrelationId">The message to process.</param>
        /// <param name="assetGuid">The asset guid.</param>
        /// <returns></returns>
        public async Task<ConsumerBaseAction> ProcessDataUpdatesAsync(
            WithCorrelationId<DataUpdateEvent> messageWithCorrelationId, Guid assetGuid)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellControl);

            var dataValue = messageWithCorrelationId?.Value;
            var correlationId = messageWithCorrelationId?.CorrelationId;

            if (dataValue?.PayloadType == null || dataValue.Payload == null)
            {
                logger.WriteCId(Level.Error, "Could not extract transaction from message: data incomplete.",
                    correlationId);

                return ConsumerBaseAction.Reject;
            }

            if (dataValue.PayloadType != "tblTransactions")
            {
                logger.WriteCId(Level.Error, $"{dataValue.PayloadType} is not supported.", correlationId);

                return ConsumerBaseAction.Reject;
            }

            if (dataValue.Action == null ||
                (dataValue.Action.ToLower() == "insert" || dataValue.Action.ToLower() == "update") == false)
            {
                logger.WriteCId(Level.Error, $"Received invalid action {dataValue.Action}.", correlationId);
                return ConsumerBaseAction.Reject;
            }

            var node = _nodeMaster.GetNode(assetGuid, messageWithCorrelationId.CorrelationId);

            if (node == null || node?.Enabled == false)
            {
                logger.WriteCNId(Level.Error, $"Cannot perform action on a disabled asset.",
                    correlationId, node?.NodeId);

                return ConsumerBaseAction.Reject;
            }

            var shouldProcess = true;

            UpdatePayload updatePayload = null;
            try
            {
                updatePayload = JsonSerializer.Deserialize<UpdatePayload>(dataValue.Payload);
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error processing {dataValue.Action} for {dataValue.PayloadType}.", ex, correlationId);

                shouldProcess = false;
            }

            if (!shouldProcess)
            {
                return ConsumerBaseAction.Reject;
            }

            OnDemandTransaction transaction = Map(updatePayload, correlationId);

            logger.WriteCNId(Level.Info, "Starting ProcessTransactionAsync for " +
                                          $"transaction id {transaction?.TransactionId}", correlationId,
                transaction?.NodeId);

            var result = _dataStore.PrepareDataItemsAsync(messageWithCorrelationId);

            if (result.Result == ConsumerBaseAction.Success)
            {
                var success = await SendMessageAsync(messageWithCorrelationId.Value.Action, messageWithCorrelationId.Value.PayloadType,
                    updatePayload, correlationId, transaction.PortId);

                logger.WriteCNId(Level.Info, "Finished ProcessTransactionAsync for " +
                                              $"transaction id {transaction?.TransactionId}.", correlationId,
                    transaction?.NodeId);

                return success;
            }

            return ConsumerBaseAction.Reject;
        }

        /// <summary>
        /// Method to process the message from Rabbit MQ and do validation on that message, then process device scanned
        /// data.
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <param name="logger">The <seealso cref="IThetaLogger"/></param>
        /// <returns></returns>
        public async Task<ConsumerBaseAction> ProcessMessageAsync(WithCorrelationId<DataUpdateEvent> message,
            IThetaLogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var result = new ConsumerBaseAction();

            var convertor = new List<JsonConverter>();
            convertor.Add(new CustomDateTimeConverter());
            if (message != null)
            {
                if (message.Value.Action.ToLower() == "update")
                {
                    var responsePayloadData = JsonSerializer
                        .Deserialize<WithCorrelationId<ProcessDeviceOperationData>>(message.Value.Payload);
                    var wellActionUpdatePayloadData = new DataUpdateEvent
                    {
                        Action = message.Value.Action,
                        PayloadType = message.Value.PayloadType,
                        Payload = JsonConvert.SerializeObject(responsePayloadData, convertor.ToArray()),
                        ResponseMetadata = message.Value.ResponseMetadata
                    };

                    var updatePayloadData = new DataUpdateEvent
                    {
                        Action = message.Value.Action,
                        Payload = JsonConvert.SerializeObject(DataUpdateEventMapper.Map(message, out var transactionId)),
                        PayloadType = message.Value.PayloadType,
                        ResponseMetadata = message.Value.ResponseMetadata
                    };

                    var data = new WithCorrelationId<DataUpdateEvent>(message?.CorrelationId, updatePayloadData);
                    result = await _dataStore.PrepareDataItemsAsync(data);

                    if (responsePayloadData != null &&
                        responsePayloadData.Value.CommunicationStatus == "OK")
                    {
                        switch (responsePayloadData.Value.PayloadType)
                        {
                            case PayloadType.WellAction:
                            case PayloadType.DeviceScanData:
                                {
                                    var responsePayload =
                                        JsonConvert.DeserializeObject<WellActionData>(responsePayloadData.Value.Payload);
                                    if (responsePayload.DeviceScanData != null)
                                    {
                                        var nodeMaster = _nodeMaster.GetNode(responsePayload.DeviceScanData.NodeId, message.CorrelationId);
                                        responsePayload.DeviceScanData.UtcScanTime = _dateTimeConverter
                                                .ConvertToApplicationServerTimeFromUTC(responsePayload.DeviceScanData.UtcScanTime,
                                                                                       message?.CorrelationId,
                                                                                       LoggingModel.WellControl);
                                        responsePayload.DeviceScanData.UtcScanTime = _dateTimeConverter
                                                .GetTimeZoneAdjustedTime(nodeMaster.Tzoffset, nodeMaster.Tzdaylight, responsePayload.DeviceScanData.UtcScanTime,
                                                                                       message?.CorrelationId,
                                                                                       LoggingModel.WellControl);
                                    }
                                    foreach (var item in responsePayload.LogEventDataList ?? new List<LogEventData>())
                                    {
                                        if (item != null)
                                        {
                                            item.EventTypeName = _notificationStore.GetEventTypeName((int)item.EventType, message.CorrelationId);
                                            item.Date = responsePayload.DeviceScanData?.UtcScanTime;
                                        }
                                    }
                                    responsePayloadData.Value.Payload = JsonConvert.SerializeObject(responsePayload, convertor.ToArray());

                                    wellActionUpdatePayloadData.Payload = JsonConvert.SerializeObject(responsePayloadData);
                                    break;
                                }
                            case PayloadType.GetData:
                                var getDataPayload = JsonConvert.DeserializeObject<GetDataPayload>(responsePayloadData.Value.Payload);
                                if (getDataPayload.LogEventDataItems != null)
                                {
                                    foreach (var item in getDataPayload.LogEventDataItems ?? new List<LogEventData>())
                                    {
                                        if (item != null)
                                        {
                                            item.EventTypeName = _notificationStore.GetEventTypeName((int)item.EventType, message.CorrelationId);
                                        }
                                    }
                                }
                                var outputData = getDataPayload.Output;
                                if (outputData != null)
                                {
                                    var index = 0;
                                    var registerDataList = TransactionByteStackUtility.PopRegList(ref outputData, ref index);
                                    wellActionUpdatePayloadData.Payload = JsonConvert.SerializeObject(registerDataList, convertor.ToArray());
                                    message.Value.Action = PayloadType.GetData.ToString();
                                }

                                break;
                            case PayloadType.Unknown:
                            case PayloadType.UpdatePocData:
                            case PayloadType.GetCard:
                            case PayloadType.DownloadEquipment:
                            case PayloadType.UploadEquipment:
                            case PayloadType.GetCardDirect:
                            case PayloadType.CommunicationLog:
                            case PayloadType.CaptureRegisterLog:
                            case PayloadType.OnOffCycles:
                            default:
                                break;
                        }

                        var responseMessage = DataUpdateEventMapper.GetBroadcastMessage(new WithCorrelationId<DataUpdateEvent>(
                            message?.CorrelationId, wellActionUpdatePayloadData), result, message.Value.Action.ToLower());

                        ClientInfo.Broadcast(message?.CorrelationId, JsonConvert.SerializeObject(responseMessage));
                        logger.WriteCId(Level.Debug, "Finished processing data updates service with transaction id "
                                                        + transactionId, message.CorrelationId);
                    }
                    else
                    {
                        var responseMessage = DataUpdateEventMapper.GetBroadcastMessage(new WithCorrelationId<DataUpdateEvent>(
                            message?.CorrelationId, wellActionUpdatePayloadData), result, message.Value.Action.ToLower());
                        ClientInfo.Broadcast(message?.CorrelationId, JsonConvert.SerializeObject(responseMessage));
                        logger.WriteCId(Level.Debug, "Failed to process data updates service", message.CorrelationId);
                    }
                }
                else if (message.Value.Action.ToLower() == "legacyupdate")
                {
                    var updatePayload = DataUpdateEventMapper.MapUpdateNodeData(message.Value, out var nodeId);
                    var dataUpdateEvent = new DataUpdateEvent
                    {
                        Action = "update",
                        PayloadType = TableNames.tblEvents.ToString(),
                        Payload = JsonConvert.SerializeObject(updatePayload),
                    };
                    var data = new WithCorrelationId<DataUpdateEvent>(message?.CorrelationId, dataUpdateEvent);

                    result = await _dataStore.PrepareDataItemsAsync(data);
                    var eventLog = EventsPayloadMapper.MapEventLogs(updatePayload);

                    var nodeMaster = _nodeMaster.GetNode(nodeId, message.CorrelationId);

                    eventLog.EventTypeName = _notificationStore.GetEventTypeName((int)eventLog.EventType, message.CorrelationId);
                    eventLog.Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC((DateTime)eventLog.Date, message?.CorrelationId, LoggingModel.WellControl);
                    eventLog.Date = _dateTimeConverter
                                              .GetTimeZoneAdjustedTime(nodeMaster.Tzoffset, nodeMaster.Tzdaylight, (DateTime)eventLog.Date,
                                                                                     message?.CorrelationId,
                                                                                     LoggingModel.WellControl);
                    var broadcastDataEvent = new DataUpdateEvent
                    {
                        Action = "update",
                        PayloadType = TableNames.tblEvents.ToString(),
                        Payload = JsonConvert.SerializeObject(eventLog, convertor.ToArray()),
                    };

                    var responseData = DataUpdateEventMapper.GetBroadcastMessage(new WithCorrelationId<DataUpdateEvent>(message?.CorrelationId, broadcastDataEvent), result, message.Value.Action.ToLower());

                    if (result == ConsumerBaseAction.Success)
                    {
                        logger.WriteCNId(Level.Info, "Finished processing data updates to legacy data store.",
                            message?.CorrelationId, nodeId);

                        ClientInfo.Broadcast(message?.CorrelationId, JsonConvert.SerializeObject(responseData));

                        return result;
                    }
                    else
                    {
                        logger.WriteCNId(Level.Info, "Failed to process data updates to legacy data store.",
                            message?.CorrelationId, nodeId);

                        ClientInfo.Broadcast(message?.CorrelationId, JsonConvert.SerializeObject(responseData));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the transaction payload for the setpoints read and call a method to send the 
        /// Rabbit MQ message to microservices to process the setpoints.
        /// </summary>
        /// <param name="assetGuid">The asset guid.</param>
        /// <param name="addressValues">The string array of address values.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>The method result of the rabbit mq opearation.</returns>
        public async Task<MethodResult<string>> SendReadRegisterTransaction(Guid assetGuid, string[] addressValues,
            string socketId)
        {
            MethodResult<string> payloadCreationResult;

            payloadCreationResult = _transactionPayloadCreator.CreateReadRegisterPayload(
                assetGuid, addressValues, socketId, out var payload);

            if (payloadCreationResult.Status != true)
            {
                return payloadCreationResult;
            }

            var dataUpdateEvent = new DataUpdateEvent
            {
                Action = "insert",
                PayloadType = "tblTransactions",
                Payload = JsonSerializer.Serialize(payload),
            };

            var request = new WithCorrelationId<DataUpdateEvent>(socketId, dataUpdateEvent);

            var sendTransactionResult = await ProcessDataUpdatesAsync(request, assetGuid);

            if (sendTransactionResult != ConsumerBaseAction.Success)
            {
                return new MethodResult<string>(false, "Transaction could not be sent");
            }

            return new MethodResult<string>(true, "Transaction sent successfully");
        }

        /// <summary>
        /// Creates the transaction payload for the setpoints write and call a method to send the 
        /// Rabbit MQ message to microservices to process the setpoints.
        /// </summary>
        /// <param name="assetGuid">The asset guid.</param>
        /// <param name="addressValues">The string array of address values.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>The method result of the rabbit mq opearation.</returns>
        public async Task<MethodResult<string>> SendWriteRegisterTransaction(Guid assetGuid,
            IDictionary<string, string> addressValues,
            string socketId)
        {
            MethodResult<string> payloadCreationResult;

            payloadCreationResult = _transactionPayloadCreator.CreateWriteRegisterPayload(
                assetGuid, addressValues, socketId, out var transactionPayload);

            if (payloadCreationResult.Status != true)
            {
                return payloadCreationResult;
            }

            var dataUpdateEvent = new DataUpdateEvent
            {
                Action = "insert",
                PayloadType = "tblTransactions",
                Payload = JsonSerializer.Serialize(transactionPayload),
            };

            var request = new WithCorrelationId<DataUpdateEvent>(socketId, dataUpdateEvent);

            var sendTransactionResult = await ProcessDataUpdatesAsync(request, assetGuid);

            if (sendTransactionResult != ConsumerBaseAction.Success)
            {
                return new MethodResult<string>(false, "Transaction could not be sent");
            }

            return new MethodResult<string>(true, "Transaction sent successfully");
        }

        #endregion

        #region Private Methods

        private OnDemandTransaction Map(UpdatePayload data, string correlationId)
        {
            if (data?.Data == null)
            {
                return null;
            }

            var result = new OnDemandTransaction();

            if (int.TryParse(data.Data.FirstOrDefault(x => x.Column.ToLower() == "transactionid")?.Value,
                    out var transactionId))
            {
                result.TransactionId = transactionId;
            }

            if (Guid.TryParse(correlationId, out var parsedCorrelationId))
            {
                result.CorrelationId = parsedCorrelationId;
            }

            if (DateTime.TryParse(data.Data.FirstOrDefault(x => x.Column.ToLower() == "daterequest")?.Value,
                    out var date))
            {
                result.Requested = date;
            }

            if (int.TryParse(data.Data.FirstOrDefault(x => x.Column.ToLower() == "portid")?.Value, out var portId))
            {
                result.PortId = portId;
            }

            if (int.TryParse(data.Data.FirstOrDefault(x => x.Column.ToLower() == "priority")?.Value, out var priority))
            {
                result.Priority = priority;
            }

            if (DateTime.TryParse(data.Data.FirstOrDefault(x => x.Column.ToLower() == "dateprocess")?.Value,
                    out var processDate))
            {
                result.Processed = processDate;
            }

            if (int.TryParse(data.Data.FirstOrDefault(x => x.Column.ToLower() == "tries")?.Value, out var tries))
            {
                result.Tries = tries;
            }

            result.TaskName = data.Data.FirstOrDefault(x => x.Column.ToLower() == "task")?.Value;
            result.TransactionSource = data.Data.FirstOrDefault(x => x.Column.ToLower() == "source")?.Value;
            result.NodeId = data.Data.FirstOrDefault(x => x.Column.ToLower() == "nodeid")?.Value;
            result.EncodedInput = data.Data.FirstOrDefault(x => x.Column.ToLower() == "input")?.Value;
            result.EncodedOutput = data.Data.FirstOrDefault(x => x.Column.ToLower() == "output")?.Value;
            result.InputText = data.Data.FirstOrDefault(x => x.Column.ToLower() == "inputtext")?.Value;
            result.Result = data.Data.FirstOrDefault(x => x.Column.ToLower() == "result")?.Value;

            return result;
        }

        private async Task<ConsumerBaseAction> SendMessageAsync(string action, string tableName,
            UpdatePayload updatePayload, string correlationId, int portId)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellControl);
            try
            {
                var message = new ProcessDataUpdateContract()
                {
                    Action = action,
                    PayloadType = tableName,
                    Payload = JsonSerializer.Serialize(updatePayload),
                    ResponseMetadata = Responsibility.PublishTransationDataToMicroservices.ToString(),
                };
                var messageWithCorrelationId = new WithCorrelationId<ProcessDataUpdateContract>(correlationId, message);

                if (await _portConfigurationStore.IsLegacyWellAsync(portId, correlationId))
                {
                    // publish to legacy db store
                    var legacyMessage = new ProcessDataUpdateContract()
                    {
                        Action = action,
                        PayloadType = "transaction.insert",
                        Payload = JsonSerializer.Serialize(updatePayload),
                    };
                    var legacyMessageWithCorrelationId =
                        new WithCorrelationId<ProcessDataUpdateContract>(correlationId, legacyMessage);

                    await _publishLegacyTransaction.PublishMessageAsync(legacyMessageWithCorrelationId);

                    var transactionMessage = new ProcessDataUpdateContract()
                    {
                        Action = action,
                        PayloadType = "transaction.monitor",
                        Payload = JsonSerializer.Serialize(updatePayload),
                    };

                    var transactionMessageWithCorrelationId =
                        new WithCorrelationId<ProcessDataUpdateContract>(correlationId, transactionMessage);

                    await _publishLegacyToListner.PublishMessageAsync(transactionMessageWithCorrelationId);
                }
                else
                {
                    await _publishTransaction.PublishMessageAsync(messageWithCorrelationId);
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "Error processing transaction from XSPOC", ex, correlationId);

                return ConsumerBaseAction.Reject;
            }

            return ConsumerBaseAction.Success;
        }

        #endregion

    }
}
