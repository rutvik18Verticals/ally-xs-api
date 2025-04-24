using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoLookupCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for LocalePhrase entities.
    /// </summary>
    public class LocalePhrasesMongoStore : MongoOperations, ILocalePhrases
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
        /// Constructs a new <seealso cref="LocalePhrasesMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public LocalePhrasesMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ILocalePhrases Implementation

        /// <summary>
        /// Get local phrases data by provided id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The <seealso cref="string"/>.</returns>
        public string Get(int id, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(LocalePhrasesMongoStore)} {nameof(Get)}", correlationId);

            string translatedPhrase = string.Empty;
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.LocalePhrases.ToString()
                   && ((LocalePhrases)x.LookupDocument).PhraseId == id);

                var lookupLocalePhrase = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupLocalePhrase == null || lookupLocalePhrase.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesMongoStore)} {nameof(Get)}", correlationId);

                    return translatedPhrase;
                }
                else
                {
                    var lookupData = lookupLocalePhrase.FirstOrDefault();

                    translatedPhrase = ((LocalePhrases)lookupData.LookupDocument).English;
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesMongoStore)} {nameof(Get)}", correlationId);

            return translatedPhrase;
        }

        /// <summary>
        /// Get local phrases data by provided ids.
        /// </summary>
        /// <param name="correlationId">The correlation Id.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>A dictionary of int to string containing phrase ids mapped to their phrase strings.</returns>
        /// <exception cref="ArgumentNullException">Handling ids null exception.</exception>
        public IDictionary<int, string> GetAll(string correlationId, params int[] ids)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(LocalePhrasesMongoStore)} {nameof(GetAll)}", correlationId);

            var result = new Dictionary<int, string>();
            try
            {
                var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.LocalePhrases.ToString() && ids.Contains(((LocalePhrases)x.LookupDocument).PhraseId));

                var lookupLocalePhrase = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filter, correlationId);

                if (lookupLocalePhrase == null)
                {
                    logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesMongoStore)} {nameof(GetAll)}", correlationId);

                    throw new ArgumentNullException(nameof(ids));
                }
                else
                {
                    var lookupData = lookupLocalePhrase.DistinctBy(x => ((LocalePhrases)x.LookupDocument).PhraseId).ToList();
                    result = lookupData.ToDictionary(x => ((LocalePhrases)x.LookupDocument).PhraseId,
                        x => ((LocalePhrases)x.LookupDocument).English);
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesMongoStore)} {nameof(GetAll)}", correlationId);

            return result;
        }

        #endregion

    }
}
