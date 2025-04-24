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
    /// Represents the ParameterDataType.
    /// </summary>
    public class ParameterDataTypeSQLStore : SQLStoreBase, IParameterDataType
    {

        /// <summary>
        /// Gets and sets the DataType.
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// Gets and sets the Description.
        /// </summary>
        public string Description { get; set; }

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        /// <summary>
        /// Constructs a new <seealso cref="ParameterDataTypeSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public ParameterDataTypeSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        /// <summary>
        /// Gets the data types.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataTypesModel"/>.</returns>
        public IList<DataTypesModel> GetItems(string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ParameterDataTypeSQLStore)} {nameof(GetItems)}", correlationId);

            var dataTypesModel = new List<DataTypesModel>();
            try
            {
                using (var context = _contextFactory.GetContext())
                {
                    dataTypesModel = context.DataTypes.AsNoTracking()
                        .Select(a => new DataTypesModel
                        {
                            Comment = a.Comment,
                            DataType = a.DataType,
                            Description = a.Description,
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(ParameterDataTypeSQLStore)} {nameof(GetItems)}", correlationId);

            return dataTypesModel;
        }

        /// <summary>
        /// Gets the data types for each of the provided addresses for a given <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId"></param>
        /// <returns>A dictionary with keys as addresses and values as data types.</returns>
        public IDictionary<int, short?> GetParametersDataTypes(Guid assetId, IList<int> addresses, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ParameterDataTypeSQLStore)} {nameof(GetParametersDataTypes)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.Parameters.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), p => p.Poctype, nm => nm.PocType, (p, nm) => new
                    {
                        Parameter = p,
                        NodeMaster = nm,
                    })
                    .Where(x => x.NodeMaster.AssetGuid == assetId && addresses.Contains(x.Parameter.Address))
                    .ToDictionary(x => x.Parameter.Address, x => x.Parameter.DataType);

                logger.WriteCId(Level.Trace, $"Finished {nameof(ParameterDataTypeSQLStore)} {nameof(GetParametersDataTypes)}", correlationId);

                return result;
            }
        }

    }
}
