using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Theta.XSPOC.Apex.Api.Common.Communications.Models;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.Utilities;

namespace Theta.XSPOC.Apex.Api.Common.Communications
{
    /// <summary>
    /// Service which prepares data for an xspoc transaction object, 
    /// and returns it in the form of a <see cref="UpdatePayload"/>.
    /// </summary>
    public class TransactionPayloadCreator : ITransactionPayloadCreator
    {

        #region Private Enums

        // NOTE: The transaction task name is case-sensitive so the casing must remain as-is.
        private enum Task
        {

            GetData,
            WellControl,

        }

        // NOTE: The transaction column name is case-sensitive so the casing must remain as-is.
        private enum TransactionColumnName
        {

            TransactionID,
            DateRequest,
            PortID,
            Task,
            Input,
            NodeID,
            Priority,
            Source,
            CorrelationId,
        }

        #endregion

        #region Private Constants

        private const int DEFAULT_EQUIPMENT_SELECTION = 7;

        #endregion

        #region Private Fields

        private readonly INodeMaster _nodeMaster;
        private readonly ITransaction _transaction;
        private readonly IParameterDataType _parameterDataType;
        private readonly Random _random;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a <see cref="TransactionPayloadCreator"/>.
        /// </summary>
        /// <param name="nodeMaster">The node master service.</param>
        /// <param name="transaction">The transaction service.</param>
        /// <param name="parameterDataType">The parameter data type service.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="nodeMaster"/> is null or
        /// when <paramref name="transaction"/> is null or
        /// when <paramref name="parameterDataType"/> is null.
        /// </exception>
        public TransactionPayloadCreator(INodeMaster nodeMaster, ITransaction transaction, IParameterDataType parameterDataType)
        {
            _nodeMaster = nodeMaster ?? throw new ArgumentNullException(nameof(nodeMaster));
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _parameterDataType = parameterDataType ?? throw new ArgumentNullException(nameof(parameterDataType));
            _random = new Random();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a transaction payload for a read register action.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns>
        public MethodResult<string> CreateReadRegisterPayload(
            Guid assetId, string[] addresses, string correlationId, out UpdatePayload payload)
        {
            var addressesAsInt = Array.ConvertAll(addresses, int.Parse);

            var addressesWithDefaultValues = addressesAsInt.ToDictionary(key => key, value => 0f);

            var payloadCreationParameters = new TransactionPayloadCreationParameters()
            {
                ActionType = ActionType.Read,
                AssetGUID = assetId,
                AddressValues = addressesWithDefaultValues,
            };

            return CreateTransactionPayload(payloadCreationParameters, correlationId, out payload);
        }

        /// <summary>
        /// Creates a transaction payload for a write register action.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="addressValues">A dictionary of address values.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns>
        public MethodResult<string> CreateWriteRegisterPayload(
            Guid assetId, IDictionary<string, string> addressValues, string correlationId,
            out UpdatePayload payload)
        {
            var addressValuesConverted = addressValues.ToDictionary(
                kvp => int.Parse(kvp.Key),
                kvp => float.Parse(kvp.Value));

            var payloadCreationParameters = new TransactionPayloadCreationParameters()
            {
                ActionType = ActionType.Write,
                AssetGUID = assetId,
                AddressValues = addressValuesConverted,
            };

            return CreateTransactionPayload(payloadCreationParameters, correlationId, out payload);
        }

        /// <summary>
        /// Creates a transaction payload for a well control action.
        /// NOTE: This overloaded method includes <paramref name="equipmentSelection"/> option which is used for upload equipment control type.
        /// </summary>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="deviceControlType">The device control type which specifies what action to perform on a well.</param>
        /// <param name="equipmentSelection">The equipment selection. Used to specify which equipment to upload for a upload equipment control type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns> 
        public MethodResult<string> CreateWellControlPayload(
            out UpdatePayload payload,
            Guid assetId,
            DeviceControlType deviceControlType,
            int equipmentSelection,
            string correlationId)
        {
            var payloadCreationParameters = new TransactionPayloadCreationParameters()
            {
                ActionType = ActionType.WellControl,
                AssetGUID = assetId,
                ControlType = deviceControlType,
                EquipmentSelection = equipmentSelection
            };

            return CreateTransactionPayload(payloadCreationParameters, correlationId, out payload);
        }

        /// <summary>
        /// Creates a transaction payload for a well control action.
        /// </summary>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="deviceControlType">The device control type which specifies what action to perform on a well.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns> 
        public MethodResult<string> CreateWellControlPayload(
            out UpdatePayload payload,
            Guid assetId,
            DeviceControlType deviceControlType,
            string correlationId)
        {
            return CreateWellControlPayload(out payload, assetId, deviceControlType, DEFAULT_EQUIPMENT_SELECTION,
                correlationId);
        }

        #endregion

        #region Private Methods

        private MethodResult<string> CreateTransactionPayload(
            TransactionPayloadCreationParameters request,
            string correlationId,
            out UpdatePayload payload)
        {
            // TODO: Source should be actual username. 
            var source = "username";

            payload = new UpdatePayload();

            string nodeId = null;
            if (request.AssetGUID != null && request.AssetGUID != Guid.Empty)
            {
                nodeId = _nodeMaster.GetNodeIdByAsset(request.AssetGUID.Value, correlationId);
            }

            // Return error on insufficient inputs.
            if (request.ActionType == ActionType.Read ||
                request.ActionType == ActionType.Write ||
                request.ActionType == ActionType.WellControl)
            {
                if (nodeId == null)
                {
                    return new MethodResult<string>(false, "Node id cannot be null.");
                }

                if (nodeId.Length == 0)
                {
                    return new MethodResult<string>(false, "Node id cannot be empty.");
                }
            }

            // Audit Log on well control actions if interval is set.
            var isFutureTransaction = request.Interval > 0;

            if (request.ActionType == ActionType.WellControl && isFutureTransaction)
            {
                // TODO: write entry to audit log like windows client?
                // ORIGINAL CODE BELOW
                // auditLogRequest As New TransactionRequest
                // auditLogRequest = CreateAuditLogRequest(request.NodeID, request.ControlType, request.Source)
                // Send(auditLogRequest, BlockMethod.UnBlocked)
            }

            // Generate buffer input
            Task task;
            int index = 0;
            var buffer = Array.Empty<byte>();

            // TODO: update this switch-statement when supporting different actions.
            // (Logic comes from TransactionClient.cs in xs-common repository)
            switch (request.ActionType)
            {
                case ActionType.Write:
                case ActionType.Read:
                    task = Task.GetData;

                    TransactionByteStackUtility.PushString(ref buffer, nodeId, ref index);
                    TransactionByteStackUtility.PushInteger(ref buffer, (int)request.ActionType, ref index);

                    // Get parameter data for all addresses provided.
                    var regList = CreateRegList(request.AssetGUID.Value, request.ActionType, request.AddressValues, correlationId);

                    TransactionByteStackUtility.PushRegList(ref buffer, regList, ref index);

                    if (request.EventGroupId.HasValue)
                    {
                        TransactionByteStackUtility.PushInteger(ref buffer, request.EventGroupId.Value, ref index);
                    }

                    break;

                case ActionType.WellControl:
                    task = Task.WellControl;

                    TransactionByteStackUtility.PushString(ref buffer, nodeId, ref index);
                    TransactionByteStackUtility.PushInteger(ref buffer, (int)request.ControlType, ref index);

                    _nodeMaster.TryGetPocTypeIdByAssetGUID(request.AssetGUID.Value, out var pocTypeId, correlationId);

                    TransactionByteStackUtility.PushInteger(ref buffer, pocTypeId, ref index);
                    TransactionByteStackUtility.PushInteger(ref buffer, request.EquipmentSelection, ref index);

                    break;

                case ActionType.StartPortLogging:
                case ActionType.StopPortLogging:
                case ActionType.GetCard:
                case ActionType.GetCardDirect:
                case ActionType.EmailAnalysis:
                case ActionType.NodeIDTask:
                case ActionType.RequestESPLogs:
                case ActionType.LogItServer:
                case ActionType.RequestPCSFTrendData:
                case ActionType.RequestEFMTrendData:
                case ActionType.RequestEFMCustodyTransferData:
                case ActionType.RequestLACTBatchLogData:
                case ActionType.RequestESPAnalysisResult:
                case ActionType.RequestHealthMonitorLogList:
                case ActionType.RequestHealthMonitorLogContent:
                case ActionType.RequestHealthMonitorRestartServer:
                case ActionType.RequestHealthMonitorRestartScheduler:
                case ActionType.SendPushNotification:
                case ActionType.GetCardDirectWithDH:
                default:
                    return new MethodResult<string>(false, $"Invalid action {request.ActionType}");
            }

            var transactionId = GenerateNewTransactionId(correlationId);

            DateTime requestDate = DateTime.UtcNow;

            payload = new UpdatePayload();

            return request.ActionType switch
            {
                ActionType.NodeIDTask or
                ActionType.LogItServer or
                ActionType.RequestESPAnalysisResult or
                ActionType.SendPushNotification =>
                    // TODO: Implement method below when implementing one of the above action types.
                    CreateTransactionUpdatePayload1(),

                ActionType.Read or
                ActionType.Write or
                ActionType.WellControl or
                ActionType.GetCard or
                ActionType.GetCardDirect or
                ActionType.GetCardDirectWithDH or
                ActionType.RequestESPLogs or
                ActionType.RequestPCSFTrendData or
                ActionType.RequestEFMTrendData or
                ActionType.RequestEFMCustodyTransferData or
                ActionType.RequestLACTBatchLogData =>

                    CreateTransactionUpdatePayload2(
                        out payload,
                        request.AssetGUID.Value,
                        transactionId,
                        requestDate,
                        request.Interval,
                        task.ToString(),
                        buffer,
                        nodeId,
                        request.Priority,
                        source,
                        correlationId),

                ActionType.StartPortLogging or
                ActionType.StopPortLogging or
                ActionType.EmailAnalysis or
                ActionType.RequestHealthMonitorLogList or
                ActionType.RequestHealthMonitorLogContent or
                ActionType.RequestHealthMonitorRestartServer or
                ActionType.RequestHealthMonitorRestartScheduler =>
                    // TODO: Implement method below when implementing one of the above action types.
                    CreateTransactionUpdatePayload3(),

                _ => new MethodResult<string>(false, $"Action {request.ActionType} is not supported"),
            };
        }

        private MethodResult<string> CreateTransactionUpdatePayload1()
        {
            throw new NotImplementedException();
        }

        private MethodResult<string> CreateTransactionUpdatePayload2(
            out UpdatePayload payload,
            Guid assetId,
            long transactionId,
            DateTime requestDate,
            int interval,
            string taskName,
            byte[] input,
            string nodeId,
            int priority,
            string source,
            string correlationId)
        {
            payload = new UpdatePayload();

            payload.Key = new List<UpdateColumnValuePair>();
            payload.Data = new List<UpdateColumnValuePair>();

            if (!_nodeMaster.TryGetPortIdByAssetGUID(assetId, out var portId, correlationId))
            {
                return new MethodResult<string>(false, "Could not retrieve node's port id.");
            }

            var requestDateWithInterval = requestDate.AddSeconds(interval);

            AddPayloadKey(payload, TransactionColumnName.TransactionID.ToString(), transactionId.ToString());
            AddPayloadData(payload, TransactionColumnName.TransactionID.ToString(), transactionId.ToString());
            AddPayloadData(payload, TransactionColumnName.DateRequest.ToString(),
               requestDateWithInterval.ToString(CultureInfo.CurrentCulture));

            AddPayloadData(payload, TransactionColumnName.PortID.ToString(), portId.ToString());
            AddPayloadData(payload, TransactionColumnName.Task.ToString(), taskName);
            AddPayloadData(payload, TransactionColumnName.Input.ToString(), Convert.ToBase64String(input));
            AddPayloadData(payload, TransactionColumnName.NodeID.ToString(), nodeId);
            AddPayloadData(payload, TransactionColumnName.Priority.ToString(), priority.ToString());
            AddPayloadData(payload, TransactionColumnName.Source.ToString(), source);
            AddPayloadData(payload, TransactionColumnName.CorrelationId.ToString(), correlationId);

            return new MethodResult<string>(true, "Transaction payload created successfully");
        }

        private MethodResult<string> CreateTransactionUpdatePayload3()
        {
            throw new NotImplementedException();
        }

        private void AddPayloadKey(UpdatePayload payload, string column, string value)
        {
            payload.Key.Add(new UpdateColumnValuePair()
            {
                Column = column,
                Value = value,
            });
        }

        private void AddPayloadData(UpdatePayload payload, string column, string value)
        {
            payload.Data.Add(new UpdateColumnValuePair()
            {
                Column = column,
                Value = value,
            });
        }

        private int GenerateNewTransactionId(string correlationId)
        {
            int newId = _random.Next(1, _random.Next(1, int.MaxValue));

            bool transactionIdExists;
            do
            {
                transactionIdExists = _transaction.TransactionIdExists(newId, correlationId);
            } while (transactionIdExists);

            return newId;
        }

        private IDictionary<int, IList<object>> CreateRegList(Guid assetId, ActionType actionType, IDictionary<int, float> addressValues, string correlationId)
        {
            bool isWrite = actionType == ActionType.Write;

            IDictionary<int, IList<object>> regList = new Dictionary<int, IList<object>>()
            {
                { 1, new List<object>() }, // Address
                { 2, new List<object>() }, // Data Type
                { 3, new List<object>() }, // Value ( either sent on a write, or retrieved on a read. Typically a read is empty )
                { 4, new List<object>() }, // DB value ( eg from facility tag or Curr Raw Scan. Not used for this, leave blank )
                { 5, new List<object>() }, // Bit Information ( not always present )                
            };

            var sortedAddressValues = addressValues
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Get data types of each address if writing.
            IDictionary<int, float?> addressDataTypes;

            if (isWrite)
            {
                addressDataTypes = _parameterDataType
                    .GetParametersDataTypes(assetId, sortedAddressValues.Keys.ToList(), correlationId)
                    .ToDictionary(x => x.Key, x => (float?)x.Value);
            }
            else
            {
                addressDataTypes = sortedAddressValues
                    .ToDictionary(x => x.Key, x => (float?)0);
            }

            foreach (var address in sortedAddressValues)
            {
                AddRegListItem(ref regList, 1, address.Key);

                // Default to datatype 3 (float modicon) if parameter not found in db.
                AddRegListItem(ref regList, 2,
                    addressDataTypes.TryGetValue(address.Key, out var addressDataType)
                        ? addressDataType.Value : 3f);

                AddRegListItem(ref regList, 3, address.Value);
                AddRegListItem(ref regList, 4, 0f);
                AddRegListItem(ref regList, 5, 0f);
            }

            return regList;
        }

        private void AddRegListItem(ref IDictionary<int, IList<object>> regList, int index, object value)
        {
            regList[index].Add(value);
        }

        #endregion

    }
}
