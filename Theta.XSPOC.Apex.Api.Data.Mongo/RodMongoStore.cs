using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for Rods entities.
    /// </summary>
    public class RodMongoStore : MongoOperations, IRod
    {

        #region Private Constants

        private const string LOOKUP_COLLECTION = "Lookup";
        private const string ASSET_COLLECTION = "Asset";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="RodMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public RodMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IRod Implementation

        /// <summary>
        /// Retrieves the rods for a group status.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of rods for the group status.</returns>
        public IList<RodForGroupStatusModel> GetRodForGroupStatus(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodMongoStore)} {nameof(GetRodForGroupStatus)}", correlationId);

            var response = new List<RodForGroupStatusModel>();
            try
            {
                var stringNodeIds = nodeIds.Select(x => x.ToString()).ToArray();

                var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                    .Where(x => (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                                 stringNodeIds.Contains(x.LegacyId["NodeId"])) &&
                                 x.ArtificialLiftType == Models.MongoCollection.Enums.Applications.RodLift.ToString()
                                 && ((RodPumpDetail)x.AssetDetails) != null && ((RodPumpDetail)x.AssetDetails).Rods != null);

                var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, filter, correlationId);

                var wellRods = assetData.Select(a => new
                {
                    NodeId = a.LegacyId["NodeId"],
                    ((RodPumpDetail)a.AssetDetails).Rods
                }).ToList();

                if (wellRods != null && wellRods.Count > 0)
                {
                    response = new List<RodForGroupStatusModel>();
                    foreach (var wellrod in wellRods)
                    {
                        foreach (var rod in wellrod.Rods)
                        {
                            response.Add(new RodForGroupStatusModel
                            {
                                NodeId = wellrod.NodeId,
                                RodNum = rod.RodNumber,
                                Name = ((Models.MongoCollection.Lookup.RodGrade)rod.RodGrade.LookupDocument)?.Name
                            });
                        }
                    }

                    response = response.Distinct()
                    .OrderBy(r => r.NodeId)
                    .ThenBy(r => r.RodNum)
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(RodMongoStore)} {nameof(GetRodForGroupStatus)}", correlationId);

            return response;
        }

        #endregion

    }
}
