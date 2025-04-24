using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the port master configuration methods
    /// against the xspoc database.
    /// </summary>
    public class PortConfigurationSQLStore : SQLStoreBase, IPortConfigurationStore
    {

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="PortConfigurationSQLStore"/> using the provided <paramref name="contextFactory"/>
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> to use for database
        /// operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="contextFactory"/> is null
        /// </exception> 
        public PortConfigurationSQLStore(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
        }

        #endregion

        #region IPortConfiguration Implementation

        /// <summary>
        /// Determines if the well is running comms on the legacy system.
        /// </summary>
        /// <param name="portId">The port id.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if the well is running on the legacy system, false otherwise.</returns>
        public async Task<bool> IsLegacyWellAsync(int portId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PortConfigurationSQLStore)} {nameof(IsLegacyWellAsync)}", correlationId);

            using (var context = ContextFactory.GetContext())
            {
                var portConfiguration = await context.PortConfigurations.AsNoTracking().FirstOrDefaultAsync(m => m.PortId == portId);

                if (portConfiguration?.PortType == null)
                {
                    logger.WriteCId(Level.Info, "Missing port type", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationSQLStore)} {nameof(IsLegacyWellAsync)}", correlationId);

                    return true;
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationSQLStore)} {nameof(IsLegacyWellAsync)}", correlationId);

                return portConfiguration.PortType <= 5;
            }
        }

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        public NodeMasterModel GetNode(Guid assetId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PortConfigurationSQLStore)} {nameof(GetNode)}", correlationId);

            var result = GetNodeMasterData(assetId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(PortConfigurationSQLStore)} {nameof(GetNode)}", correlationId);

            return result;
        }

        #endregion

    }
}