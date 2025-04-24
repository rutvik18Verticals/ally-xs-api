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
    /// This is the implementation that represents the configuration of a EnumEntitySQLStore.
    /// </summary>
    public class EnumEntitySQLStore : SQLStoreBase, IEnumEntity
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        private enum EnumTable
        {

            Applications,
            Correlations,
            CorrelationTypes,
            AnalysisTypes,
            AnalysisCurveSetTypes,
            UnitTypes,

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="EnumEntitySQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public EnumEntitySQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        /// <summary>
        /// Gets the AnalysisTypeEntities data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetAnalysisTypeEntities()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetAnalysisTypeEntities)}");

            using (var context = _contextFactory.GetContext())
            {
                var result = (from a in context.AnalysisTypeEntities.AsNoTracking()
                              where a.PhraseId.HasValue
                              select new EnumEntityModel()
                              {
                                  Table = (int)EnumTable.AnalysisTypes,
                                  Id = a.Id,
                                  PhraseId = a.PhraseId.Value
                              }).ToList();

                logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetAnalysisTypeEntities)}");

                return result;
            }
        }

        /// <summary>
        /// Gets the CorrelationEntities data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetCorrelationEntities()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetCorrelationEntities)}");

            using (var context = _contextFactory.GetContext())
            {
                var result = (from c in context.CorrelationEntities.AsNoTracking()
                              select new EnumEntityModel()
                              {
                                  Table = (int)EnumTable.Correlations,
                                  Id = c.Id,
                                  PhraseId = c.PhraseId
                              }).ToList();

                logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetCorrelationEntities)}");

                return result;
            }
        }

        /// <summary>
        /// Gets the GetCorrelationTypeEntities data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetCorrelationTypeEntities()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetCorrelationTypeEntities)}");

            using (var context = _contextFactory.GetContext())
            {
                var result = (from c in context.CorrelationTypeEntities.AsNoTracking()
                              select new EnumEntityModel()
                              {
                                  Table = (int)EnumTable.CorrelationTypes,
                                  Id = c.Id,
                                  PhraseId = c.PhraseId
                              }).ToList();

                logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetCorrelationTypeEntities)}");

                return result;
            }
        }

        /// <summary>
        /// Gets the AnalysisCurveSetTypes data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetAnalysisCurveSetTypes()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetAnalysisCurveSetTypes)}");

            using (var context = _contextFactory.GetContext())
            {
                var result = (from c in context.AnalysisCurveSetTypeEntities.AsNoTracking()
                              select new EnumEntityModel()
                              {
                                  Table = (int)EnumTable.AnalysisCurveSetTypes,
                                  Id = c.Id,
                                  PhraseId = c.PhraseId
                              }).ToList();

                logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetAnalysisCurveSetTypes)}");

                return result;
            }
        }

        /// <summary>
        /// Gets the Application data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetApplicationEntities()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetApplicationEntities)}");

            using (var context = _contextFactory.GetContext())
            {
                var result = (from a in context.ApplicationEntities.AsNoTracking()
                              where a.PhraseId.HasValue
                              select new EnumEntityModel()
                              {
                                  Table = (int)EnumTable.Applications,
                                  Id = a.Id,
                                  PhraseId = a.PhraseId.Value
                              }).ToList();

                logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetApplicationEntities)}");

                return result;
            }
        }

        /// <summary>
        /// Gets the UnitTypes data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetUnitCategories()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetUnitCategories)}");

            using (var context = _contextFactory.GetContext())
            {
                var result = (from c in context.UnitTypes.AsNoTracking()
                              select new EnumEntityModel()
                              {
                                  Table = (int)EnumTable.UnitTypes,
                                  Id = c.UnitTypeId,
                                  PhraseId = c.PhraseId == null ? 0 : (int)c.PhraseId
                              }).ToList();

                logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetUnitCategories)}");

                return result;
            }
        }

        /// <summary>
        /// Gets the CurveTypes.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetCurveTypes(int key)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetCurveTypes)}");

            IDictionary<int, int> phraseIdsByOutputId = new Dictionary<int, int>();

            using (var context = _contextFactory.GetContext())
            {
                var entities = context.CurveTypes.AsNoTracking().Where(ct => ct.ApplicationTypeId == key);

                foreach (var entity in entities)
                {
                    phraseIdsByOutputId[entity.Id] = entity.PhraseId;
                }
            }

            logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetCurveTypes)}");

            return phraseIdsByOutputId;
        }

        /// <summary>
        /// Gets the GLFlowControlDeviceState.
        /// </summary>
        /// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetGLFlowControlDeviceState()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetGLFlowControlDeviceState)}");

            IDictionary<int, int> phraseIdsByOutputId = new Dictionary<int, int>();

            using (var context = _contextFactory.GetContext())
            {
                var entities = context.GlflowControlDeviceStateEntities;

                foreach (var entity in entities)
                {
                    phraseIdsByOutputId[entity.Id] = entity.PhraseId;
                }
            }

            logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetGLFlowControlDeviceState)}");

            return phraseIdsByOutputId;
        }

        /// <summary>
        /// Gets the GLValveConfigurationOption.
        /// </summary>
        /// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetGLValveConfigurationOption()
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.Write(Level.Trace, $"Starting {nameof(EnumEntitySQLStore)} {nameof(GetGLValveConfigurationOption)}");

            IDictionary<int, int> phraseIdsByOutputId = new Dictionary<int, int>();

            using (var context = _contextFactory.GetContext())
            {
                var entities = context.GLValveConfigurationOptionsEntities.AsNoTracking();

                foreach (var entity in entities)
                {
                    phraseIdsByOutputId[entity.Id] = entity.PhraseId;
                }
            }

            logger.Write(Level.Trace, $"Finished {nameof(EnumEntitySQLStore)} {nameof(GetGLValveConfigurationOption)}");

            return phraseIdsByOutputId;
        }

    }
}
