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
    /// This is the implementation that represents the configuration of a transaction.
    /// </summary>
    public class TransactionSQLStore : SQLStoreBase, ITransaction
    {

        #region Private Members

        private readonly IThetaDbContextFactory<XspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="TransactionSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public TransactionSQLStore(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ITransaction Implementation

        /// <summary>
        /// Determines whether a transaction
        /// with specified <paramref name="transactionId"/> already exists.
        /// </summary>
        /// <param name="transactionId">The transaction id to check.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if an entry with <paramref name="transactionId"/> already exists, false otherwise.</returns>
        public bool TransactionIdExists(int transactionId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(TransactionSQLStore)} {nameof(TransactionIdExists)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.Transactions.AsNoTracking().Any(t => t.TransactionId == transactionId);

                logger.WriteCId(Level.Trace, $"Finished {nameof(TransactionSQLStore)} {nameof(TransactionIdExists)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}