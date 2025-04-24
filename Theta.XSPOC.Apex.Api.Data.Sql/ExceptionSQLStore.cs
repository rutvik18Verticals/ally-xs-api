using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Represents a SQL store for retrieving exceptions.
    /// </summary>
    public class ExceptionSQLStore : SQLStoreBase, IException
    {

        private readonly IThetaLoggerFactory _loggerFactory;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public ExceptionSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IExceptionRepository Implementation

        /// <summary>
        /// Retrieves a list of exceptions based on the provided node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of exception models.</returns>
        public IList<ExceptionModel> GetExceptions(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ExceptionSQLStore)} " +
                $"{nameof(GetExceptions)}", correlationId);

            if (nodeIds == null)
            {
                logger.WriteCId(Level.Info, "Missing node ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionSQLStore)}" +
                    $" {nameof(GetExceptions)}", correlationId);

                throw new ArgumentNullException(nameof(nodeIds));
            }

            using (var context = ContextFactory.GetContext())
            {
                var query = context.Exception.AsNoTracking()
                    .Where(e => nodeIds.Contains(e.NodeId))
                    .GroupBy(e => e.NodeId)
                    .Select(g => g.OrderByDescending(e => e.Priority)
                        .ThenBy(e => e.ExceptionGroupName)
                        .FirstOrDefault())
                    .ToList()
                    .Select(x => new ExceptionModel()
                    {
                        NodeId = x.NodeId,
                        ExceptionGroupName = x.ExceptionGroupName,
                        Priority = x.Priority,
                    });

                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionSQLStore)}" +
                    $" {nameof(GetExceptions)}", correlationId);

                return query.ToList();
            }
        }

        /// <summary>
        /// This method will retrieve the exceptions for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <param name="correlationId"></param>
        /// <returns>A <seealso cref="IList{ExceptionData}"/> that represents the most recent historical data
        /// for the defined registers for the provided <paramref name="assetId"/>.</returns>
        public async Task<IList<ExceptionData>> GetAssetStatusExceptionsAsync(Guid assetId, Guid customerId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ExceptionSQLStore)} " +
                $"{nameof(GetAssetStatusExceptionsAsync)}", correlationId);

            if (assetId == Guid.Empty)
            {
                logger.WriteCId(Level.Info, "Missing asset id", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionSQLStore)}" +
                                       $" {nameof(GetAssetStatusExceptionsAsync)}", correlationId);

                return null;
            }

            await using (var context = ContextFactory.GetContext())
            {
                var result = context.NodeMasters.AsNoTracking().Join(context.Exceptions.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                    (node, exception) => new
                    {
                        node,
                        exception,
                    }).Where(m => m.node.AssetGuid == assetId).Select(m => new ExceptionData()
                    {
                        Description = m.exception.ExceptionGroupName,
                        Priority = m.exception.Priority ?? 0,
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionSQLStore)}" +
                    $" {nameof(GetAssetStatusExceptionsAsync)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}
