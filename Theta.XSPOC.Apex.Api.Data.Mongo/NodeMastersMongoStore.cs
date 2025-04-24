using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to node master data.
    /// </summary>
    public class NodeMastersMongoStore : MongoOperations, INodeMaster
    {

        #region Private Constants

        private const string COLLECTION_NAME = "Asset";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="NodeMastersMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public NodeMastersMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        /// <summary>
        /// Get the node id from node master by asset id.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{NodeMasterModel}"/>.</returns>
        public async Task<IList<NodeMasterModel>> GetByAssetIdsAsync(IList<Guid> assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" +
                $" {nameof(GetByAssetIdsAsync)}", correlationId);

            await Task.Yield();

            var stringAssetId = assetId.Select(x => x.ToString().ToUpper()).ToArray();

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                             stringAssetId.Contains(x.LegacyId["AssetGUID"].ToUpper())));

            var assetData = _database.GetCollection<MongoAssetCollection.Asset>(COLLECTION_NAME);

            // Build the aggregation pipeline
            var pipeline = assetData.Aggregate()
                .Lookup(
                    foreignCollectionName: "Customers",
                    localField: "CustomerId",
                    foreignField: "_id",
                    @as: "AssetCustomers"
                );

            var assets = pipeline.ToList();

            if (assets != null || assets.Count > 0)
            {
                var result = assets.Select(asset => new NodeMasterModel
                {
                    AssetGuid = Guid.Parse((asset["LegacyId"].AsBsonDocument)["AssetGUID"].AsString),
                    NodeId = asset["Name"].AsString,
                    PocType = (short)(asset["POCType"].BsonType != BsonType.Null ? short.Parse(asset["POCType"]?.AsBsonDocument["LegacyId"]?.AsBsonDocument?["POCTypesId"]?.AsString) : 0),
                    ApplicationId = GetArtificialLiftType(asset["ArtificialLiftType"].AsString),
                    CompanyGuid = asset["AssetCustomers"] != null && (asset["AssetCustomers"]?.AsBsonArray).Any() ?
                        Guid.Parse((asset["AssetCustomers"].AsBsonArray)[0]["LegacyId"].AsBsonDocument["CustomerGUID"].AsString) : Guid.Empty
                }).ToList();

                return result.Where(x => assetId.Contains(x.AssetGuid)).ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetByAssetIdsAsync)}", correlationId);

            return new List<NodeMasterModel>();
        }

        /// <summary>
        /// Get the legacy well status.
        /// </summary>
        /// <param name="assetId">The asset ids to check legacy well or new architechture well.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IDictionary{TKey, TValue}"/> with guid and bool status of 
        /// legacy well or new architechture well.</returns>
        public async Task<IDictionary<Guid, bool>> GetLegacyWellAsync(IList<Guid> assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetLegacyWellAsync)}", correlationId);
            await Task.Yield();

            var stringAssetId = assetId.Select(x => x.ToString().ToUpper()).ToArray();

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                             stringAssetId.Contains(x.LegacyId["AssetGUID"].ToUpper())));

            var assetData = _database.GetCollection<MongoAssetCollection.Asset>(COLLECTION_NAME);

            // Build the aggregation pipeline
            var pipeline = assetData.Aggregate()
                .Lookup(
                    foreignCollectionName: "Port",
                    localField: "AssetConfig.PortId",
                    foreignField: "_id",
                    @as: "AssetPorts"
                )
                .Project(new BsonDocument
                {
                    { "LegacyId", 1 },
                    { "AssetPorts", 1 }
                });

            var assets = pipeline.ToList();

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetLegacyWellAsync)}", correlationId);

            return MapPortInfo(assets, assetId);
        }

        /// <summary>
        /// Filters the new architecture wells.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <see cref="List{Guid}"/> of new archtecture wells.</returns>
        public List<Guid> GetNewArchitectureWells(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNewArchitectureWells)}", correlationId);

            var assetData = _database.GetCollection<MongoAssetCollection.Asset>(COLLECTION_NAME);

            var portData = FindAll<Ports>("Port", correlationId);

            // Build the aggregation pipeline
            var pipeline = assetData.Aggregate()
                .Lookup(
                    foreignCollectionName: "Port",
                    localField: "AssetConfig.PortId",
                    foreignField: "_id",
                    @as: "AssetPorts"
                )
                .Project(new BsonDocument
                {
                    { "LegacyId", 1 },
                    { "AssetPorts", 1 }
                });

            var assets = pipeline.ToList();

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNewArchitectureWells)}", correlationId);

            return MapPortInfo(assets, null).Select(x => x.Key).ToList();
        }

        /// <summary>
        /// Gets the node master data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="NodeMasterModel"/> details.</returns>
        public NodeMasterModel GetNode(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNode)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                           (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNode)}", correlationId);

            var asset = assetData?.FirstOrDefault();

            if (asset != null)
            {
                var result = Map(asset);

                var port = Find<Ports>("Port", new FilterDefinitionBuilder<Ports>()
                                   .Where(x => x.Id == asset.AssetConfig.PortId), correlationId);

                if (port != null && port.Any())
                {
                    result.PortId = (short?)(port.FirstOrDefault()?.PortID);
                }

                var customer = Find<Customer>("Customers", new FilterDefinitionBuilder<Customer>()
                                                      .Where(x => x.Id == asset.CustomerId), correlationId);

                if (customer != null && customer.Any())
                {
                    result.CompanyGuid = Guid.Parse(customer.FirstOrDefault().LegacyId["CustomerGUID"]);
                }

                return result;
            }

            return new NodeMasterModel();
        }

        /// <summary>
        /// Gets the NodeMaster data based on node id.
        /// </summary>
        /// <param name="nodeId">The asset's node id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The NodeMaster data.</returns>
        public NodeMasterModel GetNode(string nodeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNode)}", correlationId);

            var node = new NodeMasterModel();

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                          (x.LegacyId["NodeId"].ToUpper() == nodeId.ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNode)}", correlationId);

            var asset = assetData?.FirstOrDefault();

            if (asset != null)
            {
                var result = Map(asset);

                var port = Find<Ports>("Port", new FilterDefinitionBuilder<Ports>()
                                   .Where(x => x.Id == asset.AssetConfig.PortId), correlationId);

                if (port != null && port.Any())
                {
                    result.PortId = (short?)(port.FirstOrDefault()?.PortID);
                }

                var customer = Find<Customer>("Customers", new FilterDefinitionBuilder<Customer>()
                                                      .Where(x => x.Id == asset.CustomerId), correlationId);

                if (customer != null && customer.Any())
                {
                    result.CompanyGuid = Guid.Parse(customer.FirstOrDefault().LegacyId["CustomerGUID"]);
                }

                return result;
            }

            return new NodeMasterModel();
        }

        /// <summary>
        /// Gets the node master data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public string GetNodeIdByAsset(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNodeIdByAsset)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                           (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNodeIdByAsset)}", correlationId);

            return Map(assetData?.FirstOrDefault())?.NodeId ?? string.Empty;
        }

        /// <summary>
        /// Gets the node master data based on asset guid.
        /// </summary>
        /// <param name="assetIds">The list of asset id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{NodeMasterModel}"/> details.</returns>
        public IList<NodeMasterModel> GetNodeIdsByAssetGuid(string[] assetIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNodeIdsByAssetGuid)}", correlationId);

            var stringAssetId = assetIds.Select(x => x.ToString().ToUpper()).ToArray();
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                             stringAssetId.Contains(x.LegacyId["AssetGUID"].ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNodeIdsByAssetGuid)}", correlationId);

            return Map(assetData) ?? new List<NodeMasterModel>();
        }

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="columns">The columns to fetch.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public NodeMasterDictionary GetNodeMasterData(Guid assetId, string[] columns, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetNodeMasterData)}", correlationId);

            NodeMasterDictionary nodeMasterData = new NodeMasterDictionary();
            var response = new List<NodeMasterValue>();

            if (columns != null)
            {
                var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                   .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                              (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

                var names = typeof(NodeMasterEntity).GetProperties()
                   .Select(property => property.Name)
                   .ToArray();

                var result = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

                var asset = result?.FirstOrDefault();

                foreach (var column in columns)
                {
                    if (names.Contains(column))
                    {
                        GetColumnValue(asset, column, out string value);

                        nodeMasterData.Data.TryAdd(column, value);
                    }
                }

                if (nodeMasterData.Data.Count == 0)
                {
                    foreach (var column in columns)
                    {
                        nodeMasterData.Data.Add(column, null);
                    }
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} {nameof(GetNodeMasterData)}", correlationId);

            return nodeMasterData;
        }

        /// <summary>
        /// Gets a node's poctype id by its asset guid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="pocTypeId">The output port id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public bool TryGetPocTypeIdByAssetGUID(Guid assetId, out short pocTypeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(TryGetPortIdByAssetGUID)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                           (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(TryGetPortIdByAssetGUID)}", correlationId);

            return short.TryParse(Map(assetData.FirstOrDefault())?.PocType.ToString(), out pocTypeId);
        }

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="portId">The output port id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public bool TryGetPortIdByAssetGUID(Guid assetId, out short portId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(TryGetPortIdByAssetGUID)}", correlationId);

            portId = 0;
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                           (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));
            var result = false;
            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            if (assetData != null && assetData.Any())
            {
                var asset = assetData.FirstOrDefault();

                var port = Find<Ports>("Port", new FilterDefinitionBuilder<Ports>()
                                   .Where(x => x.Id == asset.AssetConfig.PortId), correlationId);

                if (port != null && port.Any())
                {
                    portId = (short)(port.FirstOrDefault()?.PortID);
                    result = true;
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(TryGetPortIdByAssetGUID)}", correlationId);

            return result;
        }

        #region Private Methods

        private List<NodeMasterModel> Map(IList<MongoAssetCollection.Asset> assetData)
        {
            if (assetData != null || assetData.Count > 0)
            {
                return assetData.Select(x => new NodeMasterModel
                {
                    AssetGuid = Guid.Parse(x.LegacyId["AssetGUID"]),
                    NodeId = x.Name,
                    PocType = short.Parse(x.POCType.LegacyId["POCTypesId"]),
                    ApplicationId = GetArtificialLiftType(x.ArtificialLiftType)
                }).ToList();
            }

            return new List<NodeMasterModel>();
        }

        #endregion

        #region Private Methods

        private int? GetArtificialLiftType(string artificialLiftType)
        {
            if (artificialLiftType == null)
            {
                return null;
            }
            else
            {
                if (Enum.TryParse(typeof(Applications), artificialLiftType, out var result))
                {
                    return ((int?)(Applications)result);
                }
            }

            return null;
        }

        private NodeMasterModel Map(MongoAssetCollection.Asset assetData)
        {
            return new NodeMasterModel
            {
                AssetGuid = Guid.Parse(assetData.LegacyId["AssetGUID"]),
                NodeId = assetData.Name,
                PocType = short.Parse(assetData.POCType.LegacyId["POCTypesId"]),
                ApplicationId = GetArtificialLiftType(assetData.ArtificialLiftType),
                RunStatus = assetData.AssetConfig.RunStatus,
                TimeInState = assetData.AssetConfig.TimeInState,
                TodayCycles = assetData.AssetConfig.TodayCycles,
                TodayRuntime = (float?)assetData.AssetConfig.TodayRuntime,
                InferredProd = (float?)assetData.AssetConfig.InferredProduction,
                Enabled = assetData.AssetConfig.IsEnabled,
                Tzoffset = (float)assetData.AssetConfig.TimeZoneOffset,
                Tzdaylight = assetData.AssetConfig.HonorDayLightSavings,
            };

        }

        private Dictionary<Guid, bool> MapPortInfo(List<BsonDocument> assets, IList<Guid> assetId)
        {
            var result = assets.Select(x => new
            {
                assetGuid = Guid.Parse((x["LegacyId"].AsBsonDocument)["AssetGUID"].AsString),
                IsLegacy = (x["AssetPorts"].AsBsonArray).Any() && (x["AssetPorts"].AsBsonArray)[0]["PortType"].AsNullableInt32 <= 5
            });

            if (assetId != null)
            {
                result = result.Where(a => assetId.Contains(a.assetGuid)).ToList();
            }

            return result.ToDictionary(x => x.assetGuid, x => x.IsLegacy);
        }

        private void GetColumnValue(MongoAssetCollection.Asset asset, string column, out string value)
        {
            value = null;
            switch (column)
            {
                case "NodeId":
                    value = asset.Name;
                    break;
                case "Node":
                    value = asset.AssetConfig?.NodeAddress;
                    break;
                case "Enabled":
                    value = asset.AssetConfig.IsEnabled.ToString();
                    break;
                case "CommStatus":
                    value = asset.AssetConfig.CommunicationStatus?.ToString();
                    break;
                case "Comment":
                    value = asset.AssetConfig.Comment;
                    break;
                case "LastGoodScanTime":
                    value = asset.AssetConfig.LastGoodScanTime?.ToString();
                    break;
                case "RunStatus":
                    value = asset.AssetConfig.RunStatus.ToString();
                    break;
                case "HighPriAlarm":
                    value = asset.AssetConfig.HighPriorityAlarm.ToString();
                    break;
                case "CommAttempt":
                    value = asset.AssetConfig.CommunicationAttempts.ToString();
                    break;
                case "CommSuccess":
                    value = asset.AssetConfig.CommunicationSuccesses.ToString();
                    break;
                case "DataCollection":
                    value = asset.AssetConfig.IsDataCollection.ToString();
                    break;
                case "Positioner":
                    value = asset.AssetConfig.IsPositioner.ToString();
                    break;
                case "StringId":
                    //value = asset.AssetConfig.StringId;
                    break;
                case "AdhocGroup1":
                    //value = asset.AssetConfig.AdhocGroup1;
                    break;
                case "AdhocGroup2":
                    //value = asset.AssetConfig.AdhocGroup2;
                    break;
                case "AdhocGroup3":
                    //value = asset.AssetConfig.AdhocGroup3;
                    break;
                case "PagingEnabled":
                    value = asset.AssetConfig.IsPagingEnabled.ToString();
                    break;
                case "PortId":
                    value = asset.AssetConfig.PortId.ToString();
                    break;
                case "Runtime24Flag":
                    value = asset.AssetConfig.Runtime24Flag.ToString();
                    break;
                case "PocType":
                    value = asset.POCType.LegacyId["POCTypesId"];
                    break;
                case "GroupSdflag":
                    value = asset.AssetConfig.GroupShutDownFlag.ToString();
                    break;
                case "OtherWellId1":
                    value = asset.AssetConfig.OtherWellId1;
                    break;
                case "TodayRuntime":
                    value = asset.AssetConfig.TodayRuntime.ToString();
                    break;
                case "TodayCycles":
                    value = asset.AssetConfig.TodayCycles.ToString();
                    break;
                case "TimeInState":
                    value = asset.AssetConfig.TimeInState.ToString();
                    break;
                case "LastAnalCondition":
                    value = asset.AssetConfig.LastAnalysisCondition.ToString();
                    break;
                case "WellOperatingType":
                    value = asset.AssetConfig.WellOperatingType.ToString();
                    break;
                case "PumpFillage":
                    value = asset.AssetConfig.PumpFillage.ToString();
                    break;
                case "BakerVersion":
                    value = asset.AssetConfig.BakerVersion?.ToString();
                    break;
                case "RuntimeAcc":
                    value = asset.AssetConfig.RuntimeAcc.ToString();
                    break;
                case "RuntimeAccGo":
                    value = asset.AssetConfig.RuntimeAccGO.ToString();
                    break;
                case "RuntimeAccGoyest":
                    value = asset.AssetConfig.RuntimeAccGOYesterday.ToString();
                    break;
                case "RuntimeSinceStart":
                    value = asset.AssetConfig.RuntimeSinceStart.ToString();
                    break;
                case "StartsAcc":
                    value = asset.AssetConfig.StartsAcc.ToString();
                    break;
                case "StartsAccGo":
                    value = asset.AssetConfig.StartsAccGO.ToString();
                    break;
                case "StartsAccGoyest":
                    value = asset.AssetConfig.StartsAccGOYesterday.ToString();
                    break;
                case "Kwh":
                    value = asset.AssetConfig.KiloWattHours.ToString();
                    break;
                case "RuntimeAccGoyy":
                    value = asset.AssetConfig.RuntimeAccGOYY.ToString();
                    break;
                case "StartsAccGoyy":
                    value = asset.AssetConfig.StartsAccGOYY.ToString();
                    break;
                case "EnergyMode":
                    value = asset.AssetConfig.EnergyMode.ToString();
                    break;
                case "EnergyGroup":
                    value = asset.AssetConfig.EnergyGroup.ToString();
                    break;
                case "PercentCommunicationsYesterday":
                    value = asset.AssetConfig.PercentCommunicationsYesterday.ToString();
                    break;
                case "HostAlarm":
                    //value = asset.AssetConfig.HostAlarm.ToString();
                    break;
                case "HostAlarmState":
                    //value = asset.AssetConfig.HostAlarmState.ToString();
                    break;
                case "LastGoodHistScan":
                    value = asset.AssetConfig.LastGoodHistoryScan.ToString();
                    break;
                case "FiterId":
                    value = asset.AssetConfig.FilterId.ToString();
                    break;
                case "FirmwareVersion":
                    value = asset.AssetConfig.FirmwareVersion.ToString();
                    break;
                case "InferredProd":
                    value = asset.AssetConfig.InferredProduction.ToString();
                    break;
                case "FilterId":
                    value = asset.AssetConfig.FilterId.ToString();
                    break;
                case "FastScan":
                    value = asset.AssetConfig.FastScan.ToString();
                    break;
                case "VoiceNodeId":
                    value = asset.AssetConfig.VoiceNodeId.ToString();
                    break;
                case "YesterdayRuntimePercent":
                    value = asset.AssetConfig.YesterdayRuntimePercent.ToString();
                    break;
                case "TodayRuntimePercent":
                    value = asset.AssetConfig.TodayRuntimePercent.ToString();
                    break;
                case "DisableCode":
                    value = asset.AssetConfig.DisableCode.ToString();
                    break;
                case "ProductionPotential":
                    value = asset.AssetConfig.ProductionPotential.ToString();
                    break;
                case "RecSpm":
                    value = asset.AssetConfig.RecordedStrokesPerMinute.ToString();
                    break;
                case "LastAlarmDate":
                    value = asset.AssetConfig.LastAlarmDate.ToString();
                    break;
                case "Latitude":
                    value = asset.AssetConfig.Latitude.ToString();
                    break;
                case "Longitude":
                    value = asset.AssetConfig.Longitude.ToString();
                    break;
                case "MapId":
                    value = asset.AssetConfig.MapId.ToString();
                    break;
                case "ConsecutiveCommunicationFailures":
                    value = asset.AssetConfig.ConsecutiveCommunicationFailures.ToString();
                    break;
                case "AlarmAction":
                    value = asset.AssetConfig.AlarmAction.ToString();
                    break;
                case "RegisterBlockLimit":
                    value = asset.AssetConfig.RegisterBlockLimit.ToString();
                    break;
                case "TechNote":
                    value = asset.AssetConfig.TechNote.ToString();
                    break;
                case "ScanCluster":
                    value = asset.AssetConfig.ScanCluster.ToString();
                    break;
                case "AllowStartLock":
                    value = asset.AssetConfig.AllowStartLock.ToString();
                    break;
                case "Tzoffset":
                    value = asset.AssetConfig.TimeZoneOffset.ToString();
                    break;
                case "Tzdaylight":
                    value = asset.AssetConfig.HonorDayLightSavings.ToString();
                    break;
                case "DisplayName":
                    value = asset.AssetConfig.DisplayName.ToString();
                    break;
                case "LastGoodHistCollection":
                    value = asset.AssetConfig.LastGoodHistoryCollection.ToString();
                    break;
                case "Comment2":
                    value = asset.AssetConfig.Comment2;
                    break;
                case "Comment3":
                    value = asset.AssetConfig.Comment3;
                    break;
                case "Apiport":
                    //value = asset.AssetConfig.ApiPort.ToString();
                    break;
                case "Apiusername":
                    //value = asset.AssetConfig.ApiUsername;
                    break;
                case "Apipassword":
                    //value = asset.AssetConfig.ApiPassword;
                    break;
                case "CreationDateTime":
                    value = asset.CreatedOnDate.ToString();
                    break;
                case "OperationalScore":
                    value = asset.AssetConfig.OperationalScore.ToString();
                    break;
                case "ApplicationId":
                    value = GetArtificialLiftType(asset.ArtificialLiftType).ToString();
                    break;
                case "ParentNodeId":
                    value = asset.AssetConfig.ParentNodeId;
                    break;
                case "AssetGuid":
                    value = asset.LegacyId["AssetGUID"];
                    break;
                default:
                    value = null;
                    break;
            }
        }

        #endregion

    }
}
