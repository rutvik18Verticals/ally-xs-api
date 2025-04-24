using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to node master data.
    /// </summary>
    public class SetpointGroupMongoStore : MongoOperations, ISetpointGroup
    {

        #region Private Constants

        private const string COLLECTION_NAME_ASSET = "Asset";                       //tblNodeMaster
        private const string COLLECTION_NAME_LOOKUP = "Lookup";                     //tblSetpointGroups
        private const string COLLECTION_NAME_MASTERVARIABLES = "MasterVariables";   //tblParameters 
        private const string COLLECTION_NAME_SAVEDPARAMETERS = "SavedParameters";   //tblSavedParameters

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IDateTimeConverter _dateTimeConverter;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="NodeMastersMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="dateTimeConverter"></param>
        public SetpointGroupMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory, IDateTimeConverter dateTimeConverter)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        #region ISetpointGroup Implementation

        /// <summary>
        /// Get setpoint groups and setpoint registers
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{SetpointGroupModel}"/> data.</returns>
        public IList<SetpointGroupModel> GetSetPointGroupData(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(SetpointGroupMongoStore)} {nameof(GetSetPointGroupData)}", correlationId);

            var setpointGroups = new List<SetpointGroupModel>();

            var node = GetNodeByAsset(assetId, correlationId);

            if (node != null)
            {
                // Fetch SetpointGroup from Lookup Collection
                var setpointGroupCollection = _database.GetCollection<Lookup>(COLLECTION_NAME_LOOKUP);
                var setpointGroupFilter = Builders<Lookup>.Filter.And(
                    Builders<Lookup>.Filter.Eq("LookupType", "SetpointGroups"),
                    Builders<Lookup>.Filter.Gt("LookupDocument.SetpointGroupsId", 0)
                );

                // Fetch Parameters from MasterVariables Collection where POCType matches
                var masterVariablesCollection = _database.GetCollection<Parameters>(COLLECTION_NAME_MASTERVARIABLES);
                var masterVariablesFilter = Builders<Parameters>.Filter.And(
                    Builders<Parameters>.Filter.Eq("POCType.LegacyId.POCTypesId", node.POCType.LegacyId["POCTypesId"]),
                    Builders<Parameters>.Filter.Eq("ParameterType", "Param")
                );

                // Run both fetch operations in parallel using Task.Run and Wait for completion
                var setpointGroupTask = Task.Run(() => setpointGroupCollection.Find(setpointGroupFilter).ToList());
                var parameterDocsTask = Task.Run(() => masterVariablesCollection.Find(masterVariablesFilter).ToList());

                Task.WaitAll(setpointGroupTask, parameterDocsTask);

                // Process Setpoint Groups data
                var setpointGroupDocs = setpointGroupTask.Result;
                var setpointGroupsData = setpointGroupDocs.Select(doc =>
                {
                    var sg = doc.LookupDocument as SetpointGroups;
                    return new
                    {
                        SetpointGroupId = sg.SetpointGroupsId,
                        DisplayName = string.IsNullOrEmpty(sg.DisplayName) ? string.Empty : sg.DisplayName,
                        sg?.DisplayOrder
                    };
                }).ToList();

                // Process Parameters data
                var parameterDocs = parameterDocsTask.Result;
                var parametersData = parameterDocs.Select(doc =>
                {
                    var parameterDetails = doc.ParameterDocument as ParameterDetails;
                    return new
                    {
                        doc.Address,
                        doc.Description,
                        parameterDetails.SetpointGroup,
                        parameterDetails.EarliestSupportedVersion,
                        StateId = doc.State,
                    };
                }).ToList();

                // Filter parameters that has matching SetpointGroup record in SetpointGroups from Lookup Collection 
                var setpointGroupIds = setpointGroupsData.Select(sg => sg.SetpointGroupId).ToList();
                var filteredParameters = parametersData
                    .Where(p => p.SetpointGroup.HasValue && p.SetpointGroup.Value != 0 && setpointGroupIds.Contains(p.SetpointGroup.Value))
                    .ToList();

                // Fetch SavedParameters for the node and parameters
                var parameterAddresses = filteredParameters.Select(p => p.Address).ToList();
                var savedParameterCollection = _database.GetCollection<SavedParameters>(COLLECTION_NAME_SAVEDPARAMETERS);
                var savedParameterFilter = Builders<SavedParameters>.Filter.And(
                    Builders<SavedParameters>.Filter.Eq("AssetId", node.Id),
                    Builders<SavedParameters>.Filter.In("Address", parameterAddresses)
                );

                // Process Saved Parameters data
                var savedParameterDocs = savedParameterCollection.Find(savedParameterFilter).ToList();
                var savedParametersData = savedParameterDocs.Select(doc => new
                {
                    doc.Address,
                    doc.Value ,
                    doc.BackupDate
                }).ToList();

                // Prepare a lookup for SavedParameters by Address
                var savedParametersLookup = savedParametersData.ToDictionary(sp => sp.Address, sp => sp);

                // Fetch States of all StateId present in parameters
                var stateIds = filteredParameters.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).Distinct().ToList();
                var statesCollection = _database.GetCollection<Lookup>(COLLECTION_NAME_LOOKUP);
                var statesFilter = Builders<Lookup>.Filter.And(
                    Builders<Lookup>.Filter.Eq("LookupType", "States"),
                    Builders<Lookup>.Filter.In("LookupDocument.StatesId", stateIds)
                );
                var statesDocs = statesCollection.Find(statesFilter).ToList();
                var statesData = statesDocs.Select(doc =>
                {
                    var states = doc.LookupDocument as States;
                    return new
                    {
                        states.StatesId,
                        states.Value,
                        states.Text
                    };
                }).ToList();

                // Group states by StateId and prepare states lookup
                var statesLookup = statesData.GroupBy(s => s.StatesId).ToDictionary(g => g.Key, g => g.Select(s => 
                    new LookupValueModel
                    {
                        Value = s.Value,
                        Text = s.Text
                    }).ToList());

                // Get only the setpointGroupsData that have matching SetpointGroupId in the filtered parameters
                var setpointGroupIdsInParameters = filteredParameters.Select(p => p.SetpointGroup.Value).Distinct().ToList();
                var matchedSetpointGroupsData = setpointGroupsData.Where(sg => setpointGroupIdsInParameters.Contains(sg.SetpointGroupId)).ToList();

                // Build SetpointGroupModels
                foreach (var sg in matchedSetpointGroupsData)
                {
                    var groupParameters = filteredParameters.Where(p => p.SetpointGroup == sg.SetpointGroupId).ToList();

                    if (groupParameters.Count > 0)
                    {
                        var setpoints = groupParameters.Select(p =>
                        {
                            savedParametersLookup.TryGetValue(p.Address, out var sp);

                            List<LookupValueModel> backupLookupValues = null;
                            if (p.StateId.HasValue && statesLookup.TryGetValue(p.StateId.Value, out var lookupValues))
                            {
                                backupLookupValues = lookupValues;
                            }

                            DateTime? backupDate = (sp != null && sp.BackupDate.HasValue)
                                ? _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(sp.BackupDate.Value, correlationId, LoggingModel.MongoDataStore)
                                : null;

                            return new SetpointModel
                            {
                                    Description = p.Description,
                                    Parameter = p.Address.ToString(),
                                    BackupDate = backupDate,
                                    BackupValue = (sp?.Value).ToString() ?? string.Empty,
                                    IsSupported = CompareVersions(p.EarliestSupportedVersion, node.AssetConfig.FirmwareVersion),
                                    StateId = p.StateId,
                                    BackUpLookUpValues = backupLookupValues
                            };
                        }).ToList();

                        setpointGroups.Add(new SetpointGroupModel
                        {
                            SetpointGroup = sg.SetpointGroupId,
                            SetpointGroupName = sg.DisplayName,
                            RegisterCount = groupParameters.Count,
                            DisplayOrder = sg.DisplayOrder,
                            Setpoints = setpoints
                        });
                    }
                }

                // Sort setpoint groups by DisplayOrder
                setpointGroups = setpointGroups.OrderBy(sg => sg.DisplayOrder).ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(SetpointGroupMongoStore)} {nameof(GetSetPointGroupData)}", correlationId);
            return setpointGroups;
        }

        #endregion

        #region Private Methods

        private MongoAssetCollection.Asset GetNodeByAsset(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} {nameof(GetNodeByAsset)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                           (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            MongoAssetCollection.Asset assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME_ASSET, filter, correlationId).FirstOrDefault();

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} {nameof(GetNodeByAsset)}", correlationId);

            return assetData;
        }
        private bool CompareVersions(float version1, double? version2)
        {
            int compared = version2.HasValue ? version1.CompareTo((float)version2.Value) : 1;
            return compared <= 0;
        }

        #endregion

    }
}
