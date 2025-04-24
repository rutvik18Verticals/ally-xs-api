using Microsoft.EntityFrameworkCore;
using System;
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
    /// This is the implementation that represents the configuration of a GL Manufacturer
    /// on the current XSPOC database.
    /// </summary>
    public class GLManufacturerSQLStore : SQLStoreBase, IManufacturer
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GLManufacturerSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public GLManufacturerSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        }

        #endregion

        #region IManufacturer Implementation

        /// <summary>
        /// Retrieves a manufacturer by its Id.
        /// </summary>
        /// <param name="id">The Id of the manufacturer.</param>
        /// <param name="correlationId">The correlation Id for tracking purposes.</param>
        /// <returns>The manufacturer model.</returns>
        public GLManufacturerModel Get(int id, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLManufacturerSQLStore)} {nameof(GLManufacturerModel)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var tableManufacturers = context.GLManufacturer.AsNoTracking();
                var entity = tableManufacturers.FirstOrDefault(m => (m.ManufacturerId == id));

                if (entity == null)
                {
                    return null;
                }

                var result = new GLManufacturerModel
                {
                    Manufacturer = entity.Manufacturer,
                    ManufacturerID = entity.ManufacturerId
                };

                logger.WriteCId(Level.Trace, $"Finished {nameof(GLManufacturerSQLStore)} {nameof(GLManufacturerModel)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}
