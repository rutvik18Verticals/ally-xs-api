using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a local phrases
    /// on the current XSPOC database.
    /// </summary>
    public class LocalePhrasesSQLStore : SQLStoreBase, ILocalePhrases
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="LocalePhrasesSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public LocalePhrasesSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ILocalePhrases Implementation

        /// <summary>
        /// Get local phrases data by provided id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="string"/>.</returns>
        public string Get(int id, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(LocalePhrasesSQLStore)} {nameof(Get)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.LocalePhrases.AsNoTracking().FirstOrDefault(x => x.PhraseId == id)?.English ?? string.Empty;

                logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesSQLStore)} {nameof(Get)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Get local phrases data by provided ids.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="ids">The ids.</param>
        /// <returns>A dictionary of int to string containing phrase ids mapped to their phrase strings.</returns>
        /// <exception cref="ArgumentNullException">Handling ids null exception.</exception>
        public IDictionary<int, string> GetAll(string correlationId, params int[] ids)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(LocalePhrasesSQLStore)} {nameof(GetAll)}", correlationId);

            if (ids == null)
            {
                logger.WriteCId(Level.Info, $"Missing {nameof(ids)}", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesSQLStore)} {nameof(GetAll)}", correlationId);

                throw new ArgumentNullException(nameof(ids));
            }

            using (var context = _contextFactory.GetContext())
            {
                var queryResult = context.LocalePhrases.AsNoTracking().Where(x => ids.Contains(x.PhraseId)).ToList();
                var result = queryResult.ToDictionary(x => x.PhraseId, x => x.English);

                logger.WriteCId(Level.Trace, $"Finished {nameof(LocalePhrasesSQLStore)} {nameof(GetAll)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}
