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
    /// Implementation of sql operations related to control actions.
    /// </summary>
    public class ControlActionSQLStore : SQLStoreBase, IControlAction
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes an new intance of a <see cref="ControlActionSQLStore"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="contextFactory"/> is null.
        /// </exception>
        public ControlActionSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region IControlAction Implementation

        /// <summary>
        /// Gets the supported control actions for the well represented by the provided <paramref name="assetGUID"/>.
        /// </summary>
        /// <param name="assetGUID">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The supported control actions for the well represented by the provided <paramref name="assetGUID"/>.</returns>
        public IList<ControlAction> GetControlActions(Guid assetGUID, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ControlActionSQLStore)} {nameof(GetControlActions)}", correlationId);

            IList<ControlAction> result;

            using (var context = _contextFactory.GetContext())
            {
                result = context.ControlActions.AsNoTracking()
                    .Join(context.POCTypeActions.AsNoTracking(),
                            ca => ca.ControlActionId,
                            pa => pa.ControlActionId,
                            (ca, pa) => new { ca, pa })
                    .Join(context.NodeMasters.AsNoTracking(),
                            combined => combined.pa.POCType,
                            nm => nm.PocType,
                            (combined, nm) => new { combined.ca, combined.pa, nm })
                    .Where(combined => combined.nm.AssetGuid == assetGUID)
                    .Select(combined => new ControlAction
                    {
                        Id = combined.ca.ControlActionId,
                        Name = combined.ca.Description
                    })
                    .ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(ControlActionSQLStore)} {nameof(GetControlActions)}", correlationId);

            return result;
        }

        #endregion

    }
}
