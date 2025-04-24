using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a states
    /// on the current XSPOC database.
    /// </summary>
    public class StatesSQLStore : SQLStoreBase, IStates
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="StatesSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public StatesSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(StatesSQLStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

            if (causeId == null)
            {
                logger.WriteCId(Level.Info, "Missing cause id", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(StatesSQLStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

                return string.Empty;
            }

            using (var context = _contextFactory.GetContext())
            {
                var states = context.States.AsNoTracking().GroupJoin(context.LocalePhrases.AsNoTracking(),
                        s => s.PhraseId, lp => lp.PhraseId, (tblState, tblLocalePhrase) => new
                        {
                            tblState,
                            tblLocalePhrase,
                        })
                    .SelectMany(x => x.tblLocalePhrase.DefaultIfEmpty(),
                        (x, tblLocalePhrase) => new
                        {
                            x.tblState,
                            tblLocalePhrase,
                        })
                    .Where(x => x.tblState.StateId == 308).ToList();

                var state = states.FirstOrDefault(x => x.tblState.Value == causeId.Value);

                logger.WriteCId(Level.Trace, $"Finished {nameof(StatesSQLStore)} {nameof(GetCardTypeNamePocType17CardTypeMS)}", correlationId);

                return string.IsNullOrWhiteSpace(state?.tblLocalePhrase?.English)
                    ? state?.tblState.Text
                    : state?.tblLocalePhrase.English;
            }
        }

        #endregion

    }
}
