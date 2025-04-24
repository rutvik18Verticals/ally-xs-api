using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoLookupCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// This is the implementation of IEnumEntity interface using mongo data store.
    /// </summary>
    public class EnumEntityMongoStore : MongoOperations, IEnumEntity
    {

        #region Private Constants

        private const string LOOKUP_COLLECTION = "Lookup";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        private enum EnumTable
        {
            Applications,
            Correlations,
            CorrelationTypes,
            AnalysisTypes,
            AnalysisCurveSetTypes,
            UnitTypes,
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="EnumEntityMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public EnumEntityMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IEnumEntity Implementation

        /// <summary>
        /// Gets the AnalysisCurveSetTypes data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetAnalysisCurveSetTypes()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetAnalysisCurveSetTypes)}");

            var correlationId = Guid.NewGuid().ToString();

            var analysisCurveSetData = new List<EnumEntityModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.AnalysisCurveSetTypes.ToString());
                var lookupAnalysisCurveSet = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupAnalysisCurveSet == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetAnalysisCurveSetTypes)}");

                    return analysisCurveSetData;
                }
                else
                {
                    analysisCurveSetData = lookupAnalysisCurveSet.Select(a => new EnumEntityModel
                    {
                        Table = (int)EnumTable.AnalysisCurveSetTypes,
                        Id = byte.Parse(a.LegacyId["AnalysisCurveSetTypesId"]),
                        PhraseId = ((MongoLookupCollection.AnalysisCurveSetTypes)a.LookupDocument).PhraseId,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }
            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetAnalysisCurveSetTypes)}");

            return analysisCurveSetData;
        }

        /// <summary>
        /// Gets the AnalysisTypeEntities data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetAnalysisTypeEntities()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetAnalysisTypeEntities)}");

            var correlationId = Guid.NewGuid().ToString();

            var analysisTypesData = new List<EnumEntityModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.AnalysisTypes.ToString() &&
                   ((MongoLookupCollection.AnalysisTypes)x.LookupDocument).PhraseId != null);

                var lookupAnalysisTypes = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupAnalysisTypes == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetAnalysisTypeEntities)}");

                    return analysisTypesData;
                }
                else
                {
                    analysisTypesData = lookupAnalysisTypes.Select(a => new EnumEntityModel
                    {
                        Table = (int)EnumTable.AnalysisTypes,
                        Id = byte.Parse(a.LegacyId["AnalysisTypesId"]),
                        PhraseId = (int)((MongoLookupCollection.AnalysisTypes)a.LookupDocument).PhraseId,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }
            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetAnalysisTypeEntities)}");

            return analysisTypesData;
        }

        /// <summary>
        /// Gets the Application data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetApplicationEntities()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetApplicationEntities)}");

            var correlationId = Guid.NewGuid().ToString();

            var applicationData = new List<EnumEntityModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.Applications.ToString() &&
                   ((MongoLookupCollection.Applications)x.LookupDocument).PhraseId != null);

                var lookupApplication = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupApplication == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetApplicationEntities)}");

                    return applicationData;
                }
                else
                {
                    applicationData = lookupApplication.Select(a => new EnumEntityModel
                    {
                        Table = (int)EnumTable.Applications,
                        Id = byte.Parse(a.LegacyId["ApplicationsId"]),
                        PhraseId = (int)((MongoLookupCollection.Applications)a.LookupDocument).PhraseId,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }
            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetApplicationEntities)}");

            return applicationData;
        }

        /// <summary>
        /// Gets the Correlations data
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetCorrelationEntities()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationEntities)}");

            var correlationId = Guid.NewGuid().ToString();

            var correlationsData = new List<EnumEntityModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.Correlations.ToString());

                var lookupCorrelation = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupCorrelation == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationEntities)}");

                    return correlationsData;
                }
                else
                {
                    correlationsData = lookupCorrelation.Select(a => new EnumEntityModel
                    {
                        Table = (int)EnumTable.Correlations,
                        Id = byte.Parse(a.LegacyId["CorrelationsId"]),
                        PhraseId = ((MongoLookupCollection.Correlations)a.LookupDocument).PhraseId,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }
            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationEntities)}");

            return correlationsData;
        }

        /// <summary>
        /// Gets the Correlations types data
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetCorrelationTypeEntities()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationTypeEntities)}");

            var correlationId = Guid.NewGuid().ToString();

            var correlationTypesData = new List<EnumEntityModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.CorrelationTypes.ToString());

                var lookupCorrelationTypes = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupCorrelationTypes == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationTypeEntities)}");

                    return correlationTypesData;
                }
                else
                {
                    correlationTypesData = lookupCorrelationTypes.Select(a => new EnumEntityModel
                    {
                        Table = (int)EnumTable.Correlations,
                        Id = byte.Parse(a.LegacyId["CorrelationTypesId"]),
                        PhraseId = ((MongoLookupCollection.CorrelationTypes)a.LookupDocument).PhraseId,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }
            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationTypeEntities)}");

            return correlationTypesData;
        }

        /// <summary>
        /// Gets the Curve Types.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The IDictionary of curve types by application type</returns>
        public IDictionary<int, int> GetCurveTypes(int key)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetCurveTypes)}");

            var correlationId = Guid.NewGuid().ToString();
            IDictionary<int, int> phraseIdsByOutputId = new Dictionary<int, int>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.CurveTypes.ToString() && ((CurveTypes)x.LookupDocument).ApplicationTypeId == key);

                var lookupCurveTypes = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupCurveTypes == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationEntities)}");

                    return phraseIdsByOutputId;
                }
                else
                {
                    foreach (var data in lookupCurveTypes)
                    {
                        var curveTypes = (CurveTypes)data.LookupDocument;
                        int curveTypesId = int.Parse(data.LegacyId["CurveTypesId"]?.ToString());
                        phraseIdsByOutputId[curveTypesId] = curveTypes.PhraseId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }

            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCurveTypes)}");

            return phraseIdsByOutputId;
        }

        /// <summary>
        /// Gets the GLFlowControlDeviceState.
        /// </summary>
        ///// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetGLFlowControlDeviceState()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetGLFlowControlDeviceState)}");

            var correlationId = Guid.NewGuid().ToString();
            IDictionary<int, int> phraseIdsByOutputId = new Dictionary<int, int>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.GLFlowControlDeviceStates.ToString());

                var lookupGLFlowControlDeviceState = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupGLFlowControlDeviceState == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationEntities)}");

                    return phraseIdsByOutputId;
                }
                else
                {
                    foreach (var data in lookupGLFlowControlDeviceState)
                    {
                        var glFlowControlDeviceStates = (GLFlowControlDeviceStates)data.LookupDocument;
                        int glFlowControlDeviceStatesId = int.Parse(data.LegacyId["GLFlowControlDeviceStatesId"]?.ToString());
                        phraseIdsByOutputId[glFlowControlDeviceStatesId] = glFlowControlDeviceStates.PhraseId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }

            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetGLFlowControlDeviceState)}");

            return phraseIdsByOutputId;
        }

        /// <summary>
        /// Gets the GLValveConfigurationOption.
        /// </summary>
        /// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetGLValveConfigurationOption()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetGLValveConfigurationOption)}");

            var correlationId = Guid.NewGuid().ToString();
            IDictionary<int, int> phraseIdsByOutputId = new Dictionary<int, int>();

            var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
               .Where(x => x.LookupType == LookupTypes.GLValveConfigurationOptions.ToString());
            var lookupGLValveConfigurationOption = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

            if (lookupGLValveConfigurationOption == null)
            {
                logger.Write(Level.Info, "Missing lookup data");
                logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetCorrelationEntities)}");

                return phraseIdsByOutputId;
            }
            else
            {
                foreach (var data in lookupGLValveConfigurationOption)
                {
                    var glValveConfigurationOptions = (GLValveConfigurationOptions)data.LookupDocument;
                    int glValveConfigurationOptionsId = int.Parse(data.LegacyId["GLValveConfigurationOptionsId"]?.ToString());
                    phraseIdsByOutputId[glValveConfigurationOptionsId] = glValveConfigurationOptions.PhraseId;
                }
            }

            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetGLValveConfigurationOption)}");

            return phraseIdsByOutputId;
        }

        /// <summary>
        /// Gets the UnitTypes data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetUnitCategories()
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntityMongoStore)} {nameof(GetUnitCategories)}");

            var correlationId = Guid.NewGuid().ToString();

            var unitTypesData = new List<EnumEntityModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.UnitTypes.ToString());

                var lookupUnitTypes = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupUnitTypes == null)
                {
                    logger.Write(Level.Info, "Missing lookup data");
                    logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetUnitCategories)}");

                    return unitTypesData;
                }
                else
                {
                    unitTypesData = lookupUnitTypes.Select(a => new EnumEntityModel
                    {
                        Table = (int)EnumTable.Correlations,
                        Id = byte.Parse(a.LegacyId["UnitTypesId"]),
                        PhraseId = (int)((MongoLookupCollection.UnitTypes)a.LookupDocument).PhraseId,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Write(Level.Error, "An error occurred", ex);
            }
            logger.Write(Level.Trace, $"Finished {nameof(EnumEntityMongoStore)} {nameof(GetUnitCategories)}");

            return unitTypesData;
        }

        #endregion
    }
}
