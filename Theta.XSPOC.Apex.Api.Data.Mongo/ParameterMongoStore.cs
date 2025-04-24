using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Manages access to the <seealso cref="Parameters"/> collection.
    /// </summary>
    public class ParameterMongoStore : IParameterMongoStore
    {

        #region Private Constants

        private const string COLLECTION_NAME = "MasterVariables";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <seealso cref="ParameterMongoStore"/>.
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// If the parameterCollection is null.
        /// </exception>
        public ParameterMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IParameterMongoStore Implementation

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocType"/> and
        /// <paramref name="address"/>.
        /// </summary>
        /// <param name="pocType">The poc type.</param>
        /// <param name="address">The register address.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Parameters}"/> object containing channel id.</returns>
        public List<Parameters> GetParameterData(string pocType, int address, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(Parameters)}" +
                $" {nameof(GetParameterData)}", correlationId);
            var result = new List<Parameters>();
            var mongoCollection = _database.GetCollection<Parameters>(COLLECTION_NAME);
            var pocTypeIdString = LegacyKeys.POCTypesId.ToString();
            try
            {
                var filterParameter = new FilterDefinitionBuilder<Parameters>().Where(x => x.Address == address
                                 && ((x.POCType.LegacyId.ContainsKey(pocTypeIdString) && x.POCType.LegacyId[pocTypeIdString] != null &&
                                 (x.POCType.LookupType == LookupTypes.POCTypes.ToString() && x.POCType.LegacyId[pocTypeIdString] == pocType)) ||
                                 (x.LegacyId["POCType"] == "99")));

                var parameter = mongoCollection.FindSync(filterParameter)?.FirstOrDefault();

                if (parameter != null)
                {
                    result.Add(parameter);
                    if (parameter.POCType?.LegacyId["POCTypesId"] == "99")
                    {
                        var paramStandardType = parameter.ParamStandardType?.LegacyId["ParamStandardTypesId"];
                        if (paramStandardType != null)
                        {
                            parameter = GetParameterByParamStdType(pocType, int.Parse(paramStandardType), correlationId);
                            result.Add(parameter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, ex.Message, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Ending {nameof(Parameters)}" +
                               $" {nameof(GetParameterData)}", correlationId);

            return result.DistinctBy(x => x.ChannelId).ToList() ?? new List<Parameters>();
        }

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocType"/> and
        /// <paramref name="paramStdType"/>.
        /// </summary>
        /// <param name="pocType">The poc type.</param>
        /// <param name="paramStdType">The param standard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="Parameters"/> object containing channel id.</returns>
        public Parameters GetParameterByParamStdType(string pocType, int paramStdType, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ParameterMongoStore)}" +
                $" {nameof(GetParameterData)}", correlationId);

            var mongoCollection = _database.GetCollection<Parameters>(COLLECTION_NAME);
            var pocTypeIdString = LegacyKeys.POCTypesId.ToString();
            var paramStdTypeString = LegacyKeys.ParamStandardTypesId.ToString();

            var filterParameter = new FilterDefinitionBuilder<Parameters>().Where(x =>
                          x.ParamStandardType.LookupType == LookupTypes.ParamStandardTypes.ToString() && x.ParamStandardType.LegacyId.ContainsKey(paramStdTypeString)
                         && x.ParamStandardType.LegacyId[paramStdTypeString] != null && x.ParamStandardType.LegacyId[paramStdTypeString] == paramStdType.ToString()
                         && ((x.POCType.LookupType == LookupTypes.POCTypes.ToString() && x.POCType.LegacyId.ContainsKey(pocTypeIdString)
                         && x.POCType.LegacyId[pocTypeIdString] != null && x.POCType.LegacyId[pocTypeIdString] == pocType)
                         || (x.LegacyId.ContainsKey("POCType") && x.LegacyId["POCType"] == "99")));

            var parameter = mongoCollection.FindSync(filterParameter).FirstOrDefault();

            return parameter ?? new Parameters();
        }

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocType"/> and
        /// <paramref name="paramStdTypes"/>.
        /// </summary>
        /// <param name="pocType">The poc type.</param>
        /// <param name="paramStdTypes">The list of param standard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Parameters}"/> object containing channel id.</returns>
        public List<Parameters> GetParameterByParamStdType(string pocType, List<string> paramStdTypes,
            string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(Parameters)}" +
                $" {nameof(GetParameterByParamStdType)}", correlationId);

            var mongoCollection = _database.GetCollection<Parameters>(COLLECTION_NAME);
            var pocTypeIdString = LegacyKeys.POCTypesId.ToString();
            var paramStdTypeString = LegacyKeys.ParamStandardTypesId.ToString();

            var filterParameter = new FilterDefinitionBuilder<Parameters>().Where(x =>
                                       x.ParamStandardType.LookupType == LookupTypes.ParamStandardTypes.ToString() && x.ParamStandardType.LegacyId.ContainsKey(paramStdTypeString)
                                      && x.ParamStandardType.LegacyId[paramStdTypeString] != null && paramStdTypes.Contains(x.ParamStandardType.LegacyId[paramStdTypeString])
                                      && ((x.POCType.LookupType == LookupTypes.POCTypes.ToString() && x.POCType.LegacyId.ContainsKey(pocTypeIdString)
                                      && x.POCType.LegacyId[pocTypeIdString] != null && x.POCType.LegacyId[pocTypeIdString] == pocType)
                                      || (x.LegacyId.ContainsKey("POCType") && x.LegacyId["POCType"] == "99")));

            var parameters = mongoCollection.FindSync(filterParameter).ToList();

            return parameters ?? new List<Parameters>();
        }

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocTypes"/> and
        /// <paramref name="paramStdTypes"/>.
        /// </summary>
        /// <param name="pocTypes">The poc type.</param>
        /// <param name="paramStdTypes">The list of param standard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Parameters}"/> object containing channel id.</returns>
        public List<Parameters> GetParameterByParamStdType(List<string> pocTypes, List<string> paramStdTypes,
            string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(Parameters)}" +
                $" {nameof(GetParameterByParamStdType)}", correlationId);

            var mongoCollection = _database.GetCollection<Parameters>(COLLECTION_NAME);
            var pocTypeIdString = LegacyKeys.POCTypesId.ToString();
            var paramStdTypeString = LegacyKeys.ParamStandardTypesId.ToString();

            var filterParameter = new FilterDefinitionBuilder<Parameters>().Where(x =>
                                       x.ParamStandardType.LookupType == LookupTypes.ParamStandardTypes.ToString() && x.ParamStandardType.LegacyId.ContainsKey(paramStdTypeString)
                                      && x.ParamStandardType.LegacyId[paramStdTypeString] != null && paramStdTypes.Contains(x.ParamStandardType.LegacyId[paramStdTypeString])
                                      && ((x.POCType.LookupType == LookupTypes.POCTypes.ToString() && x.POCType.LegacyId.ContainsKey(pocTypeIdString)
                                      && x.POCType.LegacyId[pocTypeIdString] != null && pocTypes.Contains(x.POCType.LegacyId[pocTypeIdString])     //x.POCType.LegacyId[pocTypeIdString] == pocType
                                      )
                                      || (x.LegacyId.ContainsKey("POCType") && x.LegacyId["POCType"] == "99")));

            var parameters = mongoCollection.FindSync(filterParameter).ToList();

            logger.WriteCId(Level.Trace, $"Ending {nameof(Parameters)}" +
                $" {nameof(GetParameterByParamStdType)}", correlationId);

            return parameters ?? new List<Parameters>();
        }

        #endregion

    }
}
