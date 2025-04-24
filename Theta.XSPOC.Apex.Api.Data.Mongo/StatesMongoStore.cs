using MongoDB.Driver;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoLookupCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for State entities.
    /// </summary>
    public class StatesMongoStore : MongoOperations, IStates
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
        /// Constructs a new <seealso cref="StatesMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public StatesMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IStates Implementation

        /// <summary>
        /// Get poc type 17 card type ms by card type name.
        /// </summary>
        /// <param name="causeId">The cause id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="string"/>The card type name.</returns>
        public string GetCardTypeNamePocType17CardTypeMS(int? causeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(StatesMongoStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

            if (causeId == null)
            {
                logger.WriteCId(Level.Info, "Missing cause id", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(StatesMongoStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

                return string.Empty;
            }
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.States.ToString()
                   && ((States)x.LookupDocument).StatesId == 308 && ((States)x.LookupDocument).Value == causeId);

                var lookupState = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupState == null || lookupState.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(StatesMongoStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

                    return string.Empty;
                }
                else
                {
                    var lookupData = lookupState.FirstOrDefault();
                    if (((States)lookupData.LookupDocument).PhraseId == null)
                    {
                        return ((States)lookupData.LookupDocument).Text;
                    }
                    else
                    {
                        var filterLocalePhrase = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                           .Where(x => x.LookupType == LookupTypes.LocalePhrases.ToString()
                           && ((LocalePhrases)x.LookupDocument).PhraseId == ((States)lookupData.LookupDocument).PhraseId);

                        var lookupLocalePhrase = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filterLocalePhrase, correlationId);

                        if (lookupState != null && lookupState.Count > 0)
                        {
                            return ((LocalePhrases)lookupLocalePhrase.FirstOrDefault()?.LookupDocument)?.English;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(StatesMongoStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

            return string.Empty;
        }

        #endregion

    }
}
