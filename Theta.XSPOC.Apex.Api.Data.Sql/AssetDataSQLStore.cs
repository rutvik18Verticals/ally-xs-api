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
    /// Implementation of sql operations related to asset data.
    /// </summary>
    public class AssetDataSQLStore : SQLStoreBase, IAssetData
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an new instance of a <see cref="AssetDataSQLStore"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="contextFactory"/> is null.
        /// </exception>
        public AssetDataSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region IAssetData Implementation

        /// <summary>
        /// Gets the well's enabled status.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The well's enabled status.</returns>
        public bool? GetWellEnabledStatus(Guid assetId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AssetDataSQLStore)} {nameof(GetWellEnabledStatus)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var well = context.NodeMasters.AsNoTracking()
                    .FirstOrDefault(nm => nm.AssetGuid == assetId);

                if (well == null)
                {
                    logger.WriteCId(Level.Info, "Missing node", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataSQLStore)} {nameof(GetWellEnabledStatus)}", correlationId);

                    return null;
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataSQLStore)} {nameof(GetWellEnabledStatus)}", correlationId);

                return well.Enabled;
            }
        }

        #endregion

    }
}
