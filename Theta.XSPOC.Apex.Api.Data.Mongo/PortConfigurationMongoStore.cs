using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoPortCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for Port Configuration entities.
    /// </summary>
    public class PortConfigurationMongoStore : MongoOperations, IPortConfigurationStore
    {

        #region Private Constants

        private const string PORT_COLLECTION = "Port";
        private const string ASSET_COLLECTION = "Asset";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="PortConfigurationMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PortConfigurationMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IPortConfigurationStore Implementation

        /// <summary>
        /// Determines if the well is running comms on the legacy system.
        /// </summary>
        /// <param name="portId">The port id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>True if the well is running on the legacy system, false otherwise.</returns>
        public async Task<bool> IsLegacyWellAsync(int portId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PortConfigurationMongoStore)} {nameof(IsLegacyWellAsync)}", correlationId);

            bool isLegacyWell = false;
            try
            {
                await Task.Yield();

                var filter = new FilterDefinitionBuilder<MongoPortCollection.Ports>()
                   .Where(x => x.PortID == portId);

                var portDocument = Find<MongoPortCollection.Ports>(PORT_COLLECTION, filter, correlationId);

                if (portDocument == null || portDocument.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing port data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationMongoStore)} {nameof(IsLegacyWellAsync)}", correlationId);

                    return isLegacyWell;
                }
                else
                {
                    var lookupData = portDocument.FirstOrDefault();
                    isLegacyWell = lookupData.PortType <= 5;
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationMongoStore)} {nameof(IsLegacyWellAsync)}", correlationId);

            return isLegacyWell;
        }

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        public NodeMasterModel GetNode(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PortConfigurationMongoStore)} {nameof(GetNode)}", correlationId);

            var nodeMasterData = new NodeMasterModel();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

                var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, filter, correlationId);

                if (assetData == null || assetData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing asset data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationMongoStore)} {nameof(GetNode)}", correlationId);

                    return nodeMasterData;
                }
                else
                {
                    var asset = assetData.FirstOrDefault();
                    nodeMasterData = new NodeMasterModel
                    {
                        AssetGuid = Guid.Parse(asset.LegacyId["AssetGUID"]),
                        NodeId = asset.Name,
                        PocType = short.Parse(asset.POCType.LegacyId["POCTypesId"]),
                        ApplicationId = GetArtificialLiftType(asset.ArtificialLiftType),
                        RunStatus = asset.AssetConfig.RunStatus,
                        TimeInState = asset.AssetConfig.TimeInState,
                        TodayCycles = asset.AssetConfig.TodayCycles,
                        TodayRuntime = (float?)asset.AssetConfig.TodayRuntime,
                        InferredProd = (float?)asset.AssetConfig.InferredProduction,
                        Enabled = asset.AssetConfig.IsEnabled,
                    };

                    var port = Find<Ports>(PORT_COLLECTION, new FilterDefinitionBuilder<Ports>()
                                 .Where(x => x.Id == asset.AssetConfig.PortId), correlationId);
                    if (port != null && port.Any())
                    {
                        nodeMasterData.PortId = (short)(port.FirstOrDefault()?.PortID);
                    }

                    var customer = Find<Customer>("Customers", new FilterDefinitionBuilder<Customer>()
                                                          .Where(x => x.Id == asset.CustomerId), correlationId);
                    if (customer != null && customer.Any())
                    {
                        nodeMasterData.CompanyGuid = Guid.Parse(customer.FirstOrDefault().LegacyId["CustomerGUID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationMongoStore)} {nameof(GetNode)}", correlationId);

            return nodeMasterData;
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

        #endregion

    }
}
