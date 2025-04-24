using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a node master.
    /// </summary>
    public class NodeMastersSQLStore : SQLStoreBase, INodeMaster
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="NodeMastersSQLStore"/> using the provided <paramref name="contextFactory"/> and <paramref name="loggerFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// </param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> to create loggers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> or <paramref name="loggerFactory"/> is null.
        /// </exception>
        public NodeMastersSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory)
            : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region INodeMaster Implementation

        /// <summary>
        ///  Get node data by provided asset id.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public string GetNodeIdByAsset(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdByAsset)}", correlationId);

            string nodeId = string.Empty;

            if (assetId == Guid.Empty)
            {
                logger.WriteCId(Level.Info, "Missing asset ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdByAsset)}", correlationId);

                return nodeId;
            }

            using (var context = _contextFactory.GetContext())
            {
                var node = context.NodeMasters.AsNoTracking()
                    .FirstOrDefault(x => x.AssetGuid == assetId);

                if (node != null)
                {
                    nodeId = node.NodeId;
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdByAsset)}", correlationId);

            return nodeId;
        }

        /// <summary>
        /// Gets a node's port id by its asset guid.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="portId">The port id retrieved, returned as an out variable.</param>
        /// <param name="correlationId"></param>
        /// <returns>
        /// True if port id successfully retrieved from node, otherwise, false.
        /// Additionally, the port id is returned through the out variable <paramref name="portId"/>.
        /// </returns>
        public bool TryGetPortIdByAssetGUID(Guid assetId, out short portId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(TryGetPortIdByAssetGUID)}", correlationId);

            portId = default;

            using (var context = _contextFactory.GetContext())
            {
                var node = context.NodeMasters.AsNoTracking().FirstOrDefault(x => x.AssetGuid == assetId);

                if (node == null || !node.PortId.HasValue)
                {
                    logger.WriteCId(Level.Info, "Missing node or port id", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(TryGetPortIdByAssetGUID)}", correlationId);

                    return false;
                }

                portId = node.PortId.Value;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(TryGetPortIdByAssetGUID)}", correlationId);

            return true;
        }

        /// <summary>
        /// Gets a node's poctype id by its asset guid.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="pocTypeId">The poctype id retrieved, returned as an out variable.</param>
        /// <param name="correlationId"></param>
        /// <returns>
        /// True if poc type successfully retrieved from node, otherwise, false.
        /// Additionally, the poctype id is returned through the out variable <paramref name="pocTypeId"/>.
        /// </returns>
        public bool TryGetPocTypeIdByAssetGUID(Guid assetId, out short pocTypeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(TryGetPocTypeIdByAssetGUID)}", correlationId);

            pocTypeId = default;

            using (var context = _contextFactory.GetContext())
            {
                var node = context.NodeMasters.AsNoTracking().FirstOrDefault(x => x.AssetGuid == assetId);

                if (node == null)
                {
                    logger.WriteCId(Level.Info, "Missing node", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(TryGetPocTypeIdByAssetGUID)}", correlationId);

                    return false;
                }

                pocTypeId = node.PocType;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(TryGetPocTypeIdByAssetGUID)}", correlationId);

            return true;
        }

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        public NodeMasterModel GetNode(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetNode)}", correlationId);

            var result = GetNodeMasterData(assetId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNode)}", correlationId);

            return result;
        }

        /// <summary>
        /// Gets the NodeMaster data based on node id.
        /// </summary>
        /// <param name="nodeId">The asset's node id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The NodeMaster data.</returns>
        public NodeMasterModel GetNode(string nodeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetNode)}", correlationId);

            using (var context = ContextFactory.GetContext())
            {
                var result = context.NodeMasters.AsNoTracking().Where(x => x.NodeId == nodeId)
                    .GroupJoin(context.Company.AsNoTracking(), nm => nm.CompanyId, c => c.Id,
                        (nm, c) => new
                        {
                            NodeMaster = nm,
                            Company = c,
                        })
                    .SelectMany(sp => sp.Company.DefaultIfEmpty(),
                        (x, r) => new
                        {
                            x.NodeMaster,
                            Company = r,
                        })
                    .ToList()
                .Select(x => new NodeMasterModel()
                {
                    NodeId = x.NodeMaster.NodeId,
                    AssetGuid = x.NodeMaster.AssetGuid,
                    PocType = x.NodeMaster.PocType,
                    RunStatus = x.NodeMaster.RunStatus,
                    TimeInState = x.NodeMaster.TimeInState,
                    TodayCycles = x.NodeMaster.TodayCycles,
                    TodayRuntime = x.NodeMaster.TodayRuntime,
                    InferredProd = x.NodeMaster.InferredProd,
                    Enabled = x.NodeMaster.Enabled,
                    ApplicationId = x.NodeMaster.ApplicationId,
                    PortId = x.NodeMaster.PortId,
                    CompanyGuid = x.Company?.CustomerGUID,
                    Tzoffset = x.NodeMaster.Tzoffset,
                    Tzdaylight = x.NodeMaster.Tzdaylight,
                }).FirstOrDefault();

                logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNode)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Retrieves the node data based on asset ids.
        /// </summary>
        /// <param name="assetId">The asset ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of <seealso cref="NodeMasterModel"/>.</returns>
        public async Task<IList<NodeMasterModel>> GetByAssetIdsAsync(IList<Guid> assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetByAssetIdsAsync)}", correlationId);

            await Task.Yield();

            var result = GetNodeMasterData(assetId.ToArray());

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetByAssetIdsAsync)}", correlationId);

            return result;
        }        

        /// <summary>
        /// Gets the NodeMasterData.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="NodeMasterDictionary"/></returns>
        public NodeMasterDictionary GetNodeMasterData(Guid guid, string[] columns, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetNodeMasterData)}", correlationId);

            NodeMasterDictionary nodeMasterData = new NodeMasterDictionary();
            var response = new List<NodeMasterValue>();
            using (var context = _contextFactory.GetContext())
            {
                var names = typeof(NodeMasterEntity).GetProperties()
                    .Select(property => property.Name)
                    .ToArray();

                if (columns != null)
                {
                    var result = context.NodeMasters.AsNoTracking().Where(a => a.AssetGuid == guid)?.FirstOrDefault();

                    foreach (var column in columns)
                    {
                        if (names.Contains(column))
                        {

                            var value = result.GetType().GetProperty(column).GetValue(result, null) == null
                                ? null
                                : result.GetType().GetProperty(column).GetValue(result).ToString();

                            if (response.Count > 0)
                            {
                                nodeMasterData.Data.Add(column, value);
                            }
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
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNodeMasterData)}", correlationId);

            return nodeMasterData;
        }

        /// <summary>
        /// Get the node id from node master by asset id.
        /// </summary>
        /// <param name="assetIds">The asset ids</param>
        /// <param name="correlationId"></param>
        /// <returns>The array of <seealso cref="string"/>nodes.</returns>
        public IList<NodeMasterModel> GetNodeIdsByAssetGuid(string[] assetIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdsByAssetGuid)}", correlationId);

            List<NodeMasterModel> nodes;

            if (assetIds == null)
            {
                logger.WriteCId(Level.Info, $"Missing {nameof(assetIds)}", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdsByAssetGuid)}", correlationId);

                return null;
            }

            if (assetIds.Length == 0)
            {
                logger.WriteCId(Level.Info, $"Missing {nameof(assetIds)}", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdsByAssetGuid)}", correlationId);

                return new List<NodeMasterModel>();
            }

            using (var context = _contextFactory.GetContext())
            {
                nodes = context.NodeMasters.AsNoTracking()
                    .Where(x => assetIds.Contains(x.AssetGuid.ToString()))
                    .Select(x => new NodeMasterModel
                    {
                        AssetGuid = x.AssetGuid,
                        NodeId = x.NodeId,
                    }).ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetNodeIdsByAssetGuid)}", correlationId);

            return nodes;
        }

        /// <summary>
        /// Determines if the wells are running comms on the legacy system.
        /// </summary>
        /// <param name="assetIds">The list of asset GUIDs.</param>
        /// <param name="correlationId"></param>
        /// <returns>A dictionary containing the asset GUIDs as keys and a boolean value indicating
        /// if the well is running on the legacy system.</returns>
        public async Task<IDictionary<Guid, bool>> GetLegacyWellAsync(IList<Guid> assetIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetLegacyWellAsync)}", correlationId);

            using (var context = ContextFactory.GetContext())
            {
                var portConfigurations = await context.NodeMasters.AsNoTracking().Where(x => assetIds.Contains(x.AssetGuid))
                    .GroupJoin(context.PortConfigurations.AsNoTracking(), nm => nm.PortId, pc => pc.PortId, (nm, pc) => new
                    {
                        NodeMaster = nm,
                        PortConfiguration = pc
                    })
                    .SelectMany(x => x.PortConfiguration.DefaultIfEmpty(), (nm, pc) => new
                    {
                        nm.NodeMaster.AssetGuid,
                        IsLegacyWell = pc == null || pc.PortType <= 5
                    }).ToDictionaryAsync(x => x.AssetGuid, x => x.IsLegacyWell);

                logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersSQLStore)} {nameof(GetLegacyWellAsync)}", correlationId);

                return portConfigurations;
            }
        }

        /// <summary>
        /// Filters the new architecture wells.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Guid}"/> object containing New Architecture asset guids.</returns>        
        public List<Guid> GetNewArchitectureWells(string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(NodeMastersSQLStore)} {nameof(GetNewArchitectureWells)}",
                correlationId);

            var assetIds = new List<Guid>();
            using (var context = _contextFactory.GetContext())
            {
                assetIds = context.NodeMasters.AsNoTracking()
                    .Join(context.PortConfigurations.AsNoTracking(), nm => nm.PortId, pc => pc.PortId, (nm, pc) => new { nm, pc })
                    .Where(x => x.pc.PortType >= 6)
                    .Select(x => x.nm.AssetGuid).Distinct().ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GetNewArchitectureWells)}" +
           $" {nameof(NodeMastersSQLStore)}", correlationId);

            return assetIds;
        }

        #endregion

    }
}
