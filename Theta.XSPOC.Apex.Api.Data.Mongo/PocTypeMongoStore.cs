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
    /// Represents a Mongo store for PocType entities.
    /// </summary>
    public class PocTypeMongoStore : MongoOperations, IPocType
    {

        #region Private Constants

        private const string LOOKUP_COLLECTION = "Lookup";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="PocTypeMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PocTypeMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IPocType Implementation

        /// <summary>
        /// Retrieves a specific PocType model by its Id.
        /// </summary>
        /// <param name="pocType">The Id of the PocType.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The PocType model.</returns>
        public PocTypeModel Get(int pocType, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PocTypeMongoStore)} {nameof(Get)}", correlationId);

            var pocTypeModel = new PocTypeModel();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.POCTypes.ToString() && ((POCTypes)x.LookupDocument).POCType == pocType);

                var lookupPocType = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupPocType == null || lookupPocType.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(PocTypeMongoStore)} {nameof(Get)}", correlationId);

                    return pocTypeModel;
                }
                else
                {
                    var lookupData = lookupPocType.FirstOrDefault();
                    pocTypeModel = new PocTypeModel()
                    {
                        PocType = ((MongoLookupCollection.POCTypes)lookupData.LookupDocument).POCType,
                        Description = ((MongoLookupCollection.POCTypes)lookupData.LookupDocument).Description,
                    };
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(PocTypeMongoStore)} {nameof(Get)}", correlationId);

            return pocTypeModel;
        }

        /// <summary>
        /// Retrieves all PocType models.
        /// </summary>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>A list of PocType models.</returns>
        public IList<PocTypeModel> GetAll(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PocTypeMongoStore)} {nameof(GetAll)}", correlationId);

            var pocTypeModels = new List<PocTypeModel>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.POCTypes.ToString());

                var lookupPocTypes = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupPocTypes == null)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(PocTypeMongoStore)} {nameof(GetAll)}", correlationId);

                    return pocTypeModels;
                }
                else
                {
                    pocTypeModels = lookupPocTypes.Select(a => new PocTypeModel
                    {
                        Description = ((MongoLookupCollection.POCTypes)a.LookupDocument).Description,
                        PocType = ((MongoLookupCollection.POCTypes)a.LookupDocument).POCType,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(PocTypeMongoStore)} {nameof(GetAll)}", correlationId);

            return pocTypeModels;
        }

        #endregion

    }
}
