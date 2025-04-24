using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoLookupCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to node master data.
    /// </summary>
    public class ParameterDataTypeMongoStore : MongoOperations, IParameterDataType
    {

        #region Private Constants

        private const string ASSET_COLLECTION = "Asset";
        private const string PARAMETER_COLLECTION = "MasterVariables";
        private const string LOOKUP_COLLECTION = "Lookup";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="ParameterDataTypeMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public ParameterDataTypeMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IParameterDataTypeMongoStore Implementation

        /// <summary>
        /// Gets the data types.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="DataTypesModel"/>.</returns>
        public IList<DataTypesModel> GetItems(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ParameterDataTypeMongoStore)} {nameof(GetItems)}", correlationId);

            var dataTypesModel = new List<DataTypesModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.DataTypes.ToString());

                var lookupDataTypes = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupDataTypes == null)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(ParameterDataTypeMongoStore)} {nameof(GetItems)}", correlationId);

                    return dataTypesModel;
                }
                else
                {
                    dataTypesModel = lookupDataTypes.Select(a => new DataTypesModel
                    {
                        Comment = ((MongoLookupCollection.DataTypes)a.LookupDocument).Comment,
                        DataType = (byte)((MongoLookupCollection.DataTypes)a.LookupDocument).DataTypeId,
                        Description = ((MongoLookupCollection.DataTypes)a.LookupDocument).Description,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(ParameterDataTypeMongoStore)} {nameof(GetItems)}", correlationId);

            return dataTypesModel;
        }

        /// <summary>
        /// Gets the data types for each of the provided addresses for a given <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A dictionary with keys as addresses and values as data types.</returns>
        public IDictionary<int, short?> GetParametersDataTypes(Guid assetId, IList<int> addresses, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ParameterDataTypeMongoStore)} {nameof(GetParametersDataTypes)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, filter, correlationId);

            if (assetData == null)
            {
                logger.WriteCId(Level.Info, "Missing node", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(ParameterDataTypeMongoStore)} {nameof(GetParametersDataTypes)}", correlationId);

                return null;
            }
            var pocType = assetData.FirstOrDefault()?.POCType?.LegacyId["POCTypesId"];

            var pocTypeIdString = LegacyKeys.POCTypesId.ToString();

            var filterParameter = new FilterDefinitionBuilder<Parameters>().Where(x => addresses.Contains(x.Address)
                                      && ((x.POCType.LegacyId.ContainsKey(pocTypeIdString) && x.POCType.LegacyId[pocTypeIdString] != null &&
                                      (x.POCType.LookupType == LookupTypes.POCTypes.ToString() && x.POCType.LegacyId[pocTypeIdString] == pocType))));

            logger.WriteCId(Level.Trace, $"Finished {nameof(ParameterDataTypeMongoStore)} {nameof(GetParametersDataTypes)}", correlationId);

            var parameters = Find<Parameters>(PARAMETER_COLLECTION, filterParameter, correlationId);

            return MapParameterData(parameters);
        }

        #endregion

        #region Private Methods

        private Dictionary<int, short?> MapParameterData(IList<Parameters> parameters)
        {
            var result = new Dictionary<int, short?>();
            foreach (var parameter in parameters)
            {
                // Map the data
                var isValidDataType = short.TryParse(parameter.DataType?.LegacyId["DataTypeId"], out var dataType);
                if (isValidDataType)
                {
                    result.TryAdd(parameter.Address, dataType);
                }
            }

            return result;
        }

        #endregion

    }
}
