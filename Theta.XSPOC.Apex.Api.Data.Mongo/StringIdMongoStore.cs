using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Strings;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for PocType entities.
    /// </summary>
    public class StringIdMongoStore : MongoOperations, IStringIdStore
    {

        #region Private Constants

        private const string ROUTE_COLLECTION = "Route";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="StringIdMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public StringIdMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IPocType Implementation

        /// <summary>
        /// Gets the unit names for the specified node Ids.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of StringIdModel objects.</returns>
        public IList<StringIdModel> GetStringId(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(StringIdMongoStore)} {nameof(GetStringId)}", correlationId);

            var result = new List<StringIdModel>();

            try
            {

                var routeCollectionData = FindAll<Route>(ROUTE_COLLECTION, correlationId);

                if (routeCollectionData == null || routeCollectionData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(StringIdMongoStore)} {nameof(GetStringId)}", correlationId);

                    return result;
                }
                else
                {
                    result = routeCollectionData.Select(x => new StringIdModel
                    {
                        ContactListId = x.ContactListID,
                        ResponderListId = x.ResponderListID,
                        StringId = x.StringID,
                        StringName = x.StringName,
                    }).OrderBy(x => x.StringName).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(StringIdMongoStore)} {nameof(GetStringId)}", correlationId);

            return result;
        }

        #endregion

    }
}
