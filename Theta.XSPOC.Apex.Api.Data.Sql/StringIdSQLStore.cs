using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Represents a SQL store for PumpingUnit entities.
    /// </summary>
    public class StringIdSQLStore : SQLStoreBase, IStringIdStore
    {

        private readonly IThetaDbContextFactory<XspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringIdSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory for creating instances of the XspocDbContext.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public StringIdSQLStore(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory
        ) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets the unit names for the specified node Ids.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>The list of StringIdModel objects.</returns>
        public IList<StringIdModel> GetStringId(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(StringIdSQLStore)} {nameof(GetStringId)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.Strings.AsNoTracking()
                    .Select(x => new StringIdModel
                    {
                        ContactListId = x.ContactListId,
                        ResponderListId = x.ResponderListId,
                        StringId = x.StringId,
                        StringName = x.StringName,
                    }).OrderBy(x => x.StringName).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(StringIdSQLStore)} {nameof(GetStringId)}", correlationId);

                return result;
            }
        }

    }
}
