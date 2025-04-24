using MongoDB.Driver;
using System;
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
    /// Represents a Mongo store for GLManufacturer entities.
    /// </summary>
    public class GLManufacturerMongoStore : MongoOperations, IManufacturer
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
        /// Constructs a new <seealso cref="GLManufacturerMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public GLManufacturerMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IManufacturer Implementation

        /// <summary>
        /// Retrieves a manufacturer by its Id.
        /// </summary>
        /// <param name="id">The Id of the manufacturer.</param>
        /// <param name="correlationId">The correlation Id for tracking purposes.</param>
        /// <returns>The manufacturer model.</returns>
        public GLManufacturerModel Get(int id, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLManufacturerMongoStore)} {nameof(Get)}", correlationId);
            var glManufacturer = new GLManufacturerModel();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.GLManufacturers.ToString() && ((GLManufacturers)x.LookupDocument).ManufacturerId == id);
                var lookupGLManufacturer = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);
                if (lookupGLManufacturer == null || lookupGLManufacturer.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(GLManufacturerMongoStore)} {nameof(Get)}", correlationId);
                    return glManufacturer;
                }
                else
                {
                    var lookupData = lookupGLManufacturer.FirstOrDefault();
                    glManufacturer = new GLManufacturerModel()
                    {
                        Manufacturer = ((MongoLookupCollection.GLManufacturers)lookupData.LookupDocument)?.Manufacturer,
                        ManufacturerID = ((MongoLookupCollection.GLManufacturers)lookupData.LookupDocument).ManufacturerId
                    };
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLManufacturerMongoStore)} {nameof(Get)}", correlationId);

            return glManufacturer;
        }

        #endregion

    }
}
