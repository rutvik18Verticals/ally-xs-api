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
    /// Retrieves and manipulates PocType data.
    /// </summary>
    public class PocTypeSQLStore : SQLStoreBase, IPocType
    {

        #region Private Members

        private readonly IThetaDbContextFactory<XspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PocTypeSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory for creating the database context.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PocTypeSQLStore(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(
            contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Retrieves all PocType models.
        /// </summary>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>A list of PocType models.</returns>
        public IList<PocTypeModel> GetAll(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PocTypeSQLStore)} {nameof(GetAll)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var entities = context.PocType.AsNoTracking().ToList();

                var result = entities.Select(x => new PocTypeModel()
                {
                    PocType = x.PocType,
                    Description = x.Description,
                }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(PocTypeSQLStore)} {nameof(GetAll)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Retrieves a specific PocType model by its Id.
        /// </summary>
        /// <param name="pocType">The Id of the PocType.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The PocType model.</returns>
        public PocTypeModel Get(int pocType, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PocTypeSQLStore)} {nameof(Get)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var entity = context.PocType.AsNoTracking().FirstOrDefault(x => x.PocType == pocType);

                if (entity == null)
                {
                    logger.WriteCId(Level.Trace, $"Entity not found for PocType: {pocType}", correlationId);

                    return null;
                }

                var model = new PocTypeModel()
                {
                    PocType = entity.PocType,
                    Description = entity.Description,
                };

                logger.WriteCId(Level.Trace, $"Finished {nameof(PocTypeSQLStore)} {nameof(Get)}", correlationId);

                return model;
            }
        }

    }
}
