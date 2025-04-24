using System;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// The store for retrieving values from the system parameters.
    /// </summary>
    public class SystemParameterSQLStore : SQLStoreBase, ISystemParameter
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemParameterSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory for creating instances of the XspocDbContext.</param>
        /// <param name="loggerFactory"></param>
        public SystemParameterSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region ISystemParameter Members

        /// <summary>
        /// Retrieves the system parameter value.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The system parameter value.</returns>
        public string Get(string parameter, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);

            logger.WriteCId(Level.Trace,
                $"Starting {nameof(SystemParameterSQLStore)} {nameof(Get)}", correlationId);

            var gaugeOffHour = GetSystemParameterData(parameter);

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(SystemParameterSQLStore)} {nameof(Get)}", correlationId);

            return gaugeOffHour;
        }

        #endregion

    }
}
