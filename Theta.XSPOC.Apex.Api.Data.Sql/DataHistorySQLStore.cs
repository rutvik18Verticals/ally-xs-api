using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Implements the IDataHistorySQLStore interface.
    /// </summary>
    public class DataHistorySQLStore : SQLStoreBase, IDataHistorySQLStore
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IMemoryCache _cache;
        private readonly IDataHistoryMongoStore _dataHistoryMongoStore;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="DataHistorySQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="loggerFactory"></param>
        /// <param name="dataHistoryMongoStore"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// or
        /// </exception>
        public DataHistorySQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IMemoryCache memoryCache, IThetaLoggerFactory loggerFactory,
            IDataHistoryMongoStore dataHistoryMongoStore,IConfiguration configuration)
            : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _dataHistoryMongoStore = dataHistoryMongoStore;
            _configuration = configuration;
        }

        #endregion

        #region IDataHistorySQLStore Implementation

        /// <summary>
        /// Gets the <seealso cref="IList{GroupTrendDataModel}"/>.
        /// </summary>
        /// <returns>The <seealso cref="IList{GroupTrendDataModel}"/>.</returns>
        public IList<GroupTrendDataModel> GetGroupTrendData(string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetGroupTrendData)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.GroupParameters.AsNoTracking().GroupJoin(context.LocalePhrases.AsNoTracking(),
                        gp => gp.PhraseId, lp => lp.PhraseId, (gp, lp) => new
                        {
                            GroupParameters = gp,
                            LocalePhrases = lp
                        })
                    .SelectMany(x => x.LocalePhrases.DefaultIfEmpty(),
                        (x, localephrases) => new
                        {
                            x.GroupParameters,
                            LocalePhrases = localephrases,
                        })
                    .Select(x => new GroupTrendDataModel()
                    {
                        Id = x.GroupParameters.Id,
                        Description = x.GroupParameters.Description ?? x.LocalePhrases.English,
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetGroupTrendData)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets the AnalysisTrendData.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The analysis trend name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of columns of the view vwXdiagResults.</returns>
        public IList<AnalysisTrendDataModel> GetAnalysisTrendData(string nodeId, DateTime startDate, DateTime endDate,
             string name, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetAnalysisTrendData)}",
                correlationId);

            var response = new List<AnalysisTrendDataModel>();
            using (var context = _contextFactory.GetContext())
            {
                var names = typeof(XDiagResultEntity).GetProperties()
                   .Select(property => property.Name)
                   .ToArray();
                name = Regex.Replace(name, @"\s+", "");
                if (names.Contains(name))
                {
                    var result = context.XDiagResult.Where(x => x.NodeId == nodeId && x.Date >= startDate
                         && x.Date <= endDate && name != null).ToList();

                    response = result.Select(x => new AnalysisTrendDataModel
                    {
                        Date = x.Date,
                        Value = x.GetType().GetProperty(name).GetValue(x, null) == null ? null : float.Parse(x.GetType().GetProperty(name).GetValue(x).ToString())
                    }).OrderBy(x => x.Date).ToList();

                    return response;
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
                          $" {nameof(GetAnalysisTrendData)}", correlationId);

            return null;
        }

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="paramStandardType">The Param Standard Type ID.</param>
        /// <param name="startDate">The Start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{MeasurementTrendDataModel}"/>.</returns>
        public IList<MeasurementTrendDataModel> GetMeasurementTrendData(string nodeId,
            int paramStandardType, DateTime startDate, DateTime endDate, string correlationId)
        {
            bool isInfluxEnabled = _configuration.GetValue("EnableInflux", false);
            if (isInfluxEnabled)
            {
                return _dataHistoryMongoStore.GetMeasurementTrendData(nodeId, paramStandardType, startDate, endDate, correlationId);
            }
            else
            {
                return GetMeasurementTrendDataUsingSQL(nodeId, paramStandardType, startDate, endDate, correlationId);
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendItemModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{MeasurementTrendItemModel}"/>.</returns>
        public async Task<IList<MeasurementTrendItemModel>> GetMeasurementTrendItems(string nodeId, string correlationId)
        {
            bool isInfluxEnabled = _configuration.GetValue("EnableInflux", false);
            if (isInfluxEnabled)
            {
                return await _dataHistoryMongoStore.GetMeasurementTrendItems(nodeId, correlationId);
            }
            else
            {
                return GetMeasurementTrendItemsUsingSQL(nodeId, correlationId);
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{GroupTrendDataHistoryModel}"/>.
        /// </summary>
        /// <returns>The <seealso cref="IList{GroupTrendDataHistoryModel}"/>.</returns>
        public IList<GroupTrendDataHistoryModel> GetGroupTrendData(DateTime startDate, DateTime endDate, int groupParameterId,
            string groupName, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetGroupTrendData)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.GroupDataHistory.AsNoTracking()
                    .Where(x => x.Name == groupName && x.GroupParameterID == groupParameterId
                        && x.Date >= startDate && x.Date <= endDate && x.Value != null)
                    .Select(x => new GroupTrendDataHistoryModel()
                    {
                        Date = x.Date,
                        Value = (float)x.Value
                    }).ToList();
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetGroupTrendData)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Get the Operational Score Items by node id ,startDate and endDate.
        /// </summary>
        /// <param name="name">The trend column name.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ControllerTrendItemModel"/>.</returns>
        public IList<OperationalScoreDataModel> GetOperationalScoreTrendData
            (string name, string nodeId, DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetOperationalScoreTrendData)}",
                correlationId);

            var resultData = new List<OperationalScoreDataModel>();
            var names = typeof(OperationalScoreEntity).GetProperties()
                    .Select(property => property.Name)
                    .ToArray();

            name = Regex.Replace(name, @"\s+", "");

            if (names.Contains(name))
            {
                using (var context = _contextFactory.GetContext())
                {
                    var result = context.OperationalScore.AsNoTracking()
                        .Where(x => x.NodeId == nodeId
                        && (x.ScoreDateTime >= startDate
                        && x.ScoreDateTime <= endDate)).ToList();

                    resultData = result.Select(x => new OperationalScoreDataModel
                    {
                        Date = x.ScoreDateTime,
                        Value = x.GetType().GetProperty(name)?.GetValue(x, null) == null ? null
                        : float.Parse(x.GetType().GetProperty(name)?.GetValue(x).ToString())
                    }).OrderBy(x => x.Date).ToList();
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
            $" {nameof(GetOperationalScoreTrendData)}", correlationId);

            return resultData;
        }

        /// <summary>
        /// Gets the <seealso cref="List{DataHistoryItemModel}"/>.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryItemModel"/>.</returns>
        public DataHistoryItemModel GetDataHistoryTrends(string assetId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDataHistoryTrends)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var nodeData = GetNodeMasterData(new Guid(assetId));

                GetAllComponents(out var failureComponentTrendData, out var failureSubComponentTrendData);

                var meterTrendData = GetMeterTrendData();

                var pcsfDatalogConfiguration = GetPCSFDatalogConfiguration(nodeData.NodeId);

                var eventTrendData = GetEventTrendDataItems();

                var dataHistoryTrendData = new DataHistoryItemModel
                {
                    NodeMasterData = nodeData,
                    FailureComponentTrendData = failureComponentTrendData,
                    FailureSubComponentTrendData = failureSubComponentTrendData,
                    MeterTrendData = meterTrendData,
                    PCSFDatalogConfiguration = pcsfDatalogConfiguration,
                    EventsTrendData = eventTrendData,
                };
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetDataHistoryTrends)}", correlationId);

                return dataHistoryTrendData;
            }
        }

        /// <summary>
        /// Get the controller trend item by node id and poc type.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ControllerTrendItemModel"/>.</returns>
        public IList<ControllerTrendItemModel> GetControllerTrendItems(string nodeId, int pocType, string correlationId)
        {
            bool isInfluxEnabled = _configuration.GetValue("EnableInflux", false);
            if (isInfluxEnabled)
            {
                return _dataHistoryMongoStore.GetControllerTrendItems(nodeId, pocType, correlationId);
            }
            else
            {
                return GetControllerTrendItemsUsingSQL(nodeId, pocType, correlationId);
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{RodStressTrendItemModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{RodStressTrendItemModel}"/>.</returns>
        public IList<RodStressTrendItemModel> GetRodStressTrendItems(string nodeId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetRodStressTrendItems)}",
                correlationId);

            IList<RodStressTrendItemModel> dataModels = new List<RodStressTrendItemModel>();

            using (var context = _contextFactory.GetContext())
            {
                var xDiagRodResult = context.XDiagRodResult
                .Where(x => x.NodeId == nodeId)
                .GroupBy(x => x.Date)
                .Select(g => g.Key);

                if (xDiagRodResult != null && xDiagRodResult.Any())
                {
                    var latestDate = xDiagRodResult.Max();
                    dataModels = context.XDiagRodResult.AsNoTracking()
                    .Where(x => x.NodeId == nodeId && x.Date == latestDate)
                    .OrderByDescending(x => x.Date)
                    .ThenBy(x => x.RodNum)
                    .Select(x => new RodStressTrendItemModel
                    {
                        RodNum = x.RodNum,
                        Grade = x.Grade,
                        Diameter = x.Diameter,
                        Length = x.Length,
                        Description = x.Date.ToShortDateString() + ": " + x.RodNum + " (" + x.Grade + ", " + x.Diameter + "\", " + x.Length.ToString() + ")"
                    }).ToList();
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetRodStressTrendItems)}", correlationId);

            return dataModels;
        }

        /// <summary>
        /// Gets the <seealso cref="List{WellTestTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The param name.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="wellTestType">The well test type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{WellTestTrendDataModel}"/>.</returns>
        public IList<WellTestTrendDataModel> GetWellTestTrendData(string nodeId, string name, DateTime startDate,
            DateTime endDate, bool wellTestType, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetWellTestTrendData)}",
                correlationId);

            IList<WellTestTrendDataModel> dataModels = new List<WellTestTrendDataModel>();

            var names = typeof(WellTestEntity).GetProperties()
                .Select(property => property.Name)
                .ToArray();

            name = Regex.Replace(name, @"\s+", "");
            if (names.Contains(name))
            {
                using (var context = _contextFactory.GetContext())
                {
                    var wellTestData = context.WellTest.AsNoTracking()
                    .Where(wt =>
                    wt.TestDate >= startDate &&
                    wt.TestDate <= endDate &&
                    wt.NodeId == nodeId &&
                    ((wellTestType && wt.Approved == true) ||
                    (wellTestType == false && (wt.Approved == false || wt.Approved == null))))
                    .OrderBy(wt => wt.TestDate)
                    .AsEnumerable()
                    .Select(wt => new WellTestTrendDataModel
                    {
                        TestDate = wt.TestDate,
                        Value = name != "TotalFluid" ? (wt.GetType().GetProperty(name)?.GetValue(wt, null) == null ? null
                            : float.Parse(wt.GetType().GetProperty(name)?.GetValue(wt).ToString())) : (wt.OilRate + wt.WaterRate)
                    }).AsEnumerable();

                    dataModels = wellTestData.Where(wt => wt.Value != null).ToList();
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetWellTestTrendData)}", correlationId);

            return dataModels;
        }

        /// <summary>
        /// Gets the <seealso cref="IList{MeterTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The Start Date.</param>
        /// <param name="endDate">The End Date.</param>
        /// <param name="name">The trend parameter name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{MeterTrendDataModel}"/>.</returns>
        public IList<MeterTrendDataModel> GetMeterHistoryTrendData(string nodeId,
            DateTime startDate, DateTime endDate, string name, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetMeterHistoryTrendData)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var response = new List<MeterTrendDataModel>();
                var names = typeof(MeterHistoryEntity).GetProperties()
                            .Select(property => property.Name)
                            .ToArray();
                name = Regex.Replace(name, @"\s+", "");

                if (names.Contains(name))
                {
                    var result = context.MeterHistory.AsNoTracking().Where(x => x.NodeId == nodeId && x.Date >= startDate
                          && x.Date <= endDate).ToList();
                    response = result.Select(x => new MeterTrendDataModel
                    {
                        Date = x.Date,
                        Value = x.GetType().GetProperty(name).GetValue(x, null) == null ? null : float.Parse(x.GetType().GetProperty(name).GetValue(x).ToString())
                    }).OrderBy(x => x.Date).ToList();
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetMeterHistoryTrendData)}", correlationId);

                return response;
            }
        }

        /// <summary>
        /// Get the Event Trend data.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="eventTypeID">The event type id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="EventTrendDataModel"/>.</returns>
        public IList<EventTrendDataModel> GetEventTrendData(string nodeId, int eventTypeID,
            DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetEventTrendData)}",
                correlationId);

            IList<EventTrendDataModel> eventTrendDataModel = new List<EventTrendDataModel>();

            using (var context = _contextFactory.GetContext())
            {
                eventTrendDataModel = context.Events.AsNoTracking().Where(x => x.NodeId == nodeId && x.EventTypeId == eventTypeID && x.Date >= startDate &&
                x.Date <= endDate)
                .OrderBy(x => x.Date)
                .Select(x => new EventTrendDataModel
                {
                    Date = x.Date,
                    Note = x.Note,
                    EventTypeId = x.EventTypeId,
                    UserId = x.UserId
                }).ToList();
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetEventTrendData)}", correlationId);

            return eventTrendDataModel;
        }

        /// <summary>
        /// Gets the Get Rod Stress Trend Data <seealso cref="IList{RodStressTrendDataModel}"/>.
        /// </summary>
        /// <param name="stressColumn">The stress Column.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="rodNum">The rodNum.</param>
        /// <param name="grade">The grade.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="startDate">The startDate.</param>
        /// <param name="endDate">The endDate.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{RodStressTrendDataModel}"/></returns>
        public IList<RodStressTrendDataModel> GetRodStressTrendData(string stressColumn, string nodeId, int rodNum,
             string grade, float diameter, DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetRodStressTrendData)}",
                correlationId);

            List<RodStressTrendDataModel> rodStressTrendDataModel = new List<RodStressTrendDataModel>();

            using (var context = _contextFactory.GetContext())
            {
                var result = context.XDiagRodResult.AsNoTracking().Where(x => x.NodeId == nodeId && x.RodNum == rodNum && x.Grade == grade &&
                x.Diameter == diameter &&
                x.Date >= startDate &&
                x.Date <= endDate)
                .OrderBy(x => x.Date)
                .Select(x => new
                {
                    x.Date,
                    x.BottomMinStress,
                    x.TopMaxStress,
                    x.TopMinStress
                }).ToList();

                foreach (var item in result)
                {
                    if (nameof(item.TopMaxStress) == stressColumn)
                    {
                        rodStressTrendDataModel.Add
                               (
                                   new RodStressTrendDataModel
                                   {
                                       Date = item.Date,
                                       StressColumn = item.TopMaxStress
                                   }
                               );
                    }
                    else if (nameof(item.TopMinStress) == stressColumn)
                    {
                        rodStressTrendDataModel.Add
                           (
                               new RodStressTrendDataModel
                               {
                                   Date = item.Date,
                                   StressColumn = item.TopMinStress
                               }
                           );
                    }
                    else if (nameof(item.BottomMinStress) == stressColumn)
                    {
                        rodStressTrendDataModel.Add
                           (
                               new RodStressTrendDataModel
                               {
                                   Date = item.Date,
                                   StressColumn = item.BottomMinStress
                               }
                           );
                    }
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
              $" {nameof(GetRodStressTrendData)}", correlationId);

            return rodStressTrendDataModel;
        }

        /// <summary>
        /// Gets the AnalysisTrend Items.
        /// </summary>
        /// <returns>The list of columns of the view vwXdiagResults.</returns>
        public IList<string> GetAnalysisTrendItems(string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetAnalysisTrendItems)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                string query = $"SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.VIEWS t " +
                    " INNER JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME AND t.TABLE_SCHEMA = c.TABLE_SCHEMA " +
                    " WHERE t.TABLE_SCHEMA = 'dbo' AND t.TABLE_NAME IN ('vwXdiagResults') ORDER BY COLUMN_NAME ";

                var result = context.Database.SqlQuery<string>(FormattableStringFactory.Create(query)).AsNoTracking().ToList();
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
              $" {nameof(GetAnalysisTrendItems)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets the <seealso cref="DataHistoryModel"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryModel"/>.</returns>
        public DataHistoryModel GetDataHistoryList(Guid assetId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDataHistoryList)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var nodeData = GetNodeMasterData(assetId);

                if (nodeData == null || string.IsNullOrEmpty(nodeData.NodeId))
                {
                    return null;
                }

                var groupDataTrendData = GetGroupTrendData(correlationId);

                var controllerTrendData = GetControllerTrendItems(nodeData.NodeId, nodeData.PocType, correlationId);

                var analysisTrendData = GetAnalysisTrendItems(correlationId);

                var measurementTrendData = GetMeasurementTrendItems(nodeData.NodeId, correlationId);

                var rodStressTrendData = GetRodStressTrendItems(nodeData.NodeId, correlationId);

                GetAllComponents(out var failureComponentTrendData, out var failureSubComponentTrendData);

                var dataHistoryModel = new DataHistoryModel
                {
                    NodeMasterData = nodeData,
                    GroupTrendData = groupDataTrendData,
                    MeasurementTrendData = measurementTrendData,
                    ControllerTrendData = controllerTrendData,
                    AnalysisTrendData = analysisTrendData,
                    RodStressTrendData = rodStressTrendData,
                    FailureComponentTrendData = failureComponentTrendData,
                    FailureSubComponentTrendData = failureSubComponentTrendData,
                    MeterTrendData = GetMeterTrendData(),
                    EventsTrendData = GetEventTrendDataItems(),
                    PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(nodeData.NodeId),
                    GLIncludeInjGasInTest = GetSystemParameterData("GLIncludeInjGasInTest")
                };
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetDataHistoryList)}", correlationId);

                return dataHistoryModel;
            }
        }

        /// <summary>
        /// Gets the most recent pcsf datalog configuration data based on node id and datalog number.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="datalogNumber">The datalog number.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="PCSFDatalogConfigurationItemModel"/>.</returns>
        public PCSFDatalogConfigurationItemModel GetPCSFDatalogConfigurationData(string nodeId,
            int datalogNumber, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetPCSFDatalogConfigurationData)}",
                correlationId);

            PCSFDatalogConfigurationItemModel result = null;

            if (string.IsNullOrEmpty(nodeId))
            {
                return result;
            }

            using (var context = _contextFactory.GetContext())
            {
                var entity = context.PCSFDatalogConfiguration.AsNoTracking()
                    .FirstOrDefault(x => x.NodeId == nodeId && x.DatalogNumber == datalogNumber);

                if (entity != null)
                {
                    result = new PCSFDatalogConfigurationItemModel
                    {
                        NodeId = entity.NodeId,
                        DatalogNumber = entity.DatalogNumber,
                        ScheduledScanEnabled = entity.ScheduledScanEnabled,
                        OnDemandScanEnabled = entity.OnDemandScanEnabled,
                        LastSavedIndex = entity.LastSavedIndex,
                        LastSavedDateTime = entity.LastSavedDateTime,
                        DatalogName = entity.DatalogName,
                        Name1 = entity.Name1,
                        Name2 = entity.Name2,
                        Name3 = entity.Name3,
                        Name4 = entity.Name4,
                        Name5 = entity.Name5,
                        Name6 = entity.Name6,
                        Name7 = entity.Name7,
                        Name8 = entity.Name8,
                        Name9 = entity.Name9,
                        Name10 = entity.Name10,
                        Name11 = entity.Name11,
                        Name12 = entity.Name12,
                        CurrentTransactionCounter = entity.CurrentTransactionCounter,
                        IndexOfNewestRecord = entity.IndexOfNewestRecord,
                        IndexOfOldestRecord = entity.IndexOfOldestRecord,
                        NumberOfRecords = entity.NumberOfRecords,
                        MaximumNumberOfRecords = entity.MaximumNumberOfRecords,
                        Field2IsWellId = entity.Field2IsWellId,
                        NumberOfFields = entity.NumberOfFields,
                        IntervalMinutes = entity.IntervalMinutes,

                    };
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetPCSFDatalogConfigurationData)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets the most recent pcsf datalog record.
        /// </summary>
        /// <param name="wellNames">List of well names.</param>
        /// <param name="datalogNumber">The datalog number.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="PCSFDatalogRecordModel"/>.</returns>
        public PCSFDatalogRecordModel FindMostRecentPCSFDatalogRecord(IDictionary<int, string> wellNames,
            int datalogNumber, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(FindMostRecentPCSFDatalogRecord)}",
                correlationId);

            var mostRecent = new PCSFDatalogRecordModel();

            if (wellNames == null || wellNames.Count == 0)
            {
                return mostRecent;
            }

            string listOfWells = string.Empty;

            foreach (var wellName in wellNames)
            {
                string nodeId = wellName.Value;
                listOfWells = listOfWells + "'" + nodeId + "',";
            }

            listOfWells = listOfWells[..^1];

            using (var context = _contextFactory.GetContext())
            {
                var entity = context.PCSFDatalogRecord.AsNoTracking()
                    .Where(x => listOfWells.Contains(x.NodeId) && x.DatalogNumber == datalogNumber)
                    .OrderByDescending(x => x.LogDateTime).FirstOrDefault();

                mostRecent = new PCSFDatalogRecordModel
                {
                    NodeId = entity.NodeId,
                    DatalogNumber = datalogNumber,
                    LogDateTime = entity.LogDateTime,
                    RecordIndex = entity.RecordIndex,
                    Value2 = entity.Value2,
                    Value3 = entity.Value3,
                    Value4 = entity.Value4,
                    Value5 = entity.Value5,
                    Value6 = entity.Value6,
                    Value7 = entity.Value7,
                    Value8 = entity.Value8,
                    Value9 = entity.Value9,
                    Value10 = entity.Value10,
                    Value11 = entity.Value11,
                    Value12 = entity.Value12,
                };
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(FindMostRecentPCSFDatalogRecord)}", correlationId);

            return mostRecent;
        }

        /// <summary>
        /// Get MasterNodeID for input wellPortId.
        /// </summary>
        /// <param name="masterNodeAddress">Node address with "+offset" removed.</param>
        /// <param name="masterPocType">POCType of the Slave Deice.</param>
        /// <param name="wellPortId">PortID of the Slave Device, must be same as Master.</param>
        /// <param name="correlationId"></param>
        /// <returns>master controllers NodeID string.</returns>
        public string GetMasterNodeId(string masterNodeAddress,
            int masterPocType, int wellPortId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetMasterNodeId)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var entityNodeId = context.NodeMasters.AsNoTracking().FirstOrDefault(x =>
                    x.Node == masterNodeAddress && x.PocType == masterPocType && x.PortId == wellPortId).NodeId;
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
            $" {nameof(GetMasterNodeId)}", correlationId);

                return entityNodeId;
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{ProductionStatisticsTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The trend parameter name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ProductionStatisticsTrendDataModel}"/>.</returns>
        public IList<ProductionStatisticsTrendDataModel> GetProductionStatisticsTrendData(string nodeId,
            DateTime startDate, DateTime endDate, string name, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetProductionStatisticsTrendData)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var response = new List<ProductionStatisticsTrendDataModel>();
                var names = typeof(ProductionStatisticsEntity).GetProperties()
                            .Select(property => property.Name)
                            .ToArray();
                name = Regex.Replace(name, @"\s+", "");

                if (names.Contains(name))
                {
                    var result = context.ProductionStatistics.AsNoTracking().Where(x => x.NodeId == nodeId && x.ProcessedDate >= startDate
                          && x.ProcessedDate <= endDate).ToList();
                    response = result.Select(x => new ProductionStatisticsTrendDataModel
                    {
                        ProcessedDate = x.ProcessedDate,
                        Value = x.GetType().GetProperty(name).GetValue(x, null) == null ? null : float.Parse(x.GetType().GetProperty(name).GetValue(x).ToString())
                    }).OrderBy(x => x.ProcessedDate).ToList();
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetProductionStatisticsTrendData)}", correlationId);

                return response;
            }
        }

        /// <summary>
        /// Gets the PCSF Datalog Record data based on node id, datalog number that are saved between 
        /// the input start date and end date.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="datalogNumber">The datalog number.</param>
        /// <param name="startDateTime">The input start date.</param>
        /// <param name="endDateTime">The input end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{PCSFDatalogRecordModel}"/> records.</returns>
        public IList<PCSFDatalogRecordModel> GetPCSFDatalogRecordItems(string nodeId, int datalogNumber,
            DateTime startDateTime, DateTime endDateTime, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetPCSFDatalogRecordItems)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.PCSFDatalogRecord.AsNoTracking().Where(x => x.NodeId == nodeId &&
                    x.DatalogNumber == datalogNumber && x.LogDateTime >= startDateTime && x.LogDateTime <= endDateTime)
                    .Select(x => new PCSFDatalogRecordModel
                    {
                        NodeId = x.NodeId,
                        DatalogNumber = x.DatalogNumber,
                        LogDateTime = x.LogDateTime,
                        RecordIndex = x.RecordIndex,
                        Value2 = x.Value2,
                        Value3 = x.Value3,
                        Value4 = x.Value4,
                        Value5 = x.Value5,
                        Value6 = x.Value6,
                        Value7 = x.Value7,
                        Value8 = x.Value8,
                        Value9 = x.Value9,
                        Value10 = x.Value10,
                        Value11 = x.Value11,
                        Value12 = x.Value12,
                    }).ToList();
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetPCSFDatalogRecordItems)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets the ESPAnalysisResults data for a node in the given date range.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ESPAnalysisResultModel}"/> object.</returns>
        public IList<ESPAnalysisResultModel> SearchESPAnalysisResult(string nodeId,
            DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(SearchESPAnalysisResult)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.ESPAnalysisResults.AsNoTracking().Where(x => x.NodeId == nodeId &&
                    x.TestDate >= startDate && x.TestDate <= endDate)
                    .Select(x => new ESPAnalysisResultModel
                    {
                        NodeId = x.NodeId,
                        ProcessedDate = x.ProcessedDate,
                        VerticalPumpDepth = x.VerticalPumpDepth,
                        MeasuredPumpDepth = x.MeasuredPumpDepth,
                        OilRate = x.OilRate,
                        WaterRate = x.WaterRate,
                        GasRate = x.GasRate,
                        PumpIntakePressure = x.PumpIntakePressure,
                        GrossRate = x.GrossRate,
                        FluidLevelAbovePump = x.FluidLevelAbovePump,
                        TubingPressure = x.TubingPressure,
                        CasingPressure = x.CasingPressure,
                        Frequency = x.Frequency,
                        DischargeGaugePressure = x.DischargeGaugePressure,
                        DischargeGageDepth = x.DischargeGaugeDepth,
                        UseDischargeGageInAnalysis = x.UseDischargeGageInAnalysis,
                        EnableGasHandling = x.EnableGasHandling,
                        SpecificGravityOfGas = x.SpecificGravityOfGas,
                        BottomholeTemperature = x.BottomholeTemperature,
                        GasSeparatorEfficiency = x.GasSeparatorEfficiency,
                        OilApi = x.OilApi,
                        CasingId = x.CasingId,
                        TubingOd = x.TubingOd,
                        CasingValveClosed = x.CasingValveClosed,
                        ProductivityIndex = x.ProductivityIndex,
                        PressureAcrossPump = x.PressureAcrossPump,
                        PumpDischargePressure = x.PumpDischargePressure,
                        HeadAcrossPump = x.HeadAcrossPump,
                        FrictionalLossInTubing = x.FrictionalLossInTubing,
                        PumpEfficiency = x.PumpEfficiency,
                        CalculatedFluidLevelAbovePump = x.CalculatedFluidLevelAbovePump,
                        FluidSpecificGravity = x.FluidSpecificGravity,
                        PumpStaticPressure = x.PumpStaticPressure,
                        RateAtBubblePoint = x.RateAtBubblePoint,
                        RateAtMaxOil = x.RateAtMaxOil,
                        RateAtMaxLiquid = x.RateAtMaxLiquid,
                        Iprslope = x.IPRSlope,
                        WaterCut = x.WaterCut,
                        GasOilRatioAtPump = x.GasOilRatioAtPump,
                        SpecificGravityOfOil = x.SpecificGravityOfOil,
                        FormationVolumeFactor = x.FormationVolumeFactor,
                        GasCompressibilityFactor = x.GasCompressibilityFactor,
                        GasVolumeFactor = x.GasVolumeFactor,
                        ProducingGor = x.ProducingGor,
                        GasInSolution = x.GasInSolution,
                        FreeGasAtPump = x.FreeGasAtPump,
                        OilVolumeAtPump = x.OilVolumeAtPump,
                        GasVolumeAtPump = x.GasVolumeAtPump,
                        TotalVolumeAtPump = x.TotalVolumeAtPump,
                        FreeGas = x.FreeGas,
                        TurpinParameter = x.TurpinParameter,
                        CompositeTubingSpecificGravity = x.CompositeTubingSpecificGravity,
                        GasDensity = x.GasDensity,
                        LiquidDensity = x.LiquidDensity,
                        AnnularSeparationEfficiency = x.AnnularSeparationEfficiency,
                        TubingGas = x.TubingGas,
                        TubingGor = x.TubingGor,
                        Success = x.Success,
                        ResultMessage = x.ResultMessage,
                        ResultMessageTemplate = x.ResultMessageTemplate,
                        PumpIntakePressureSource = x.PumpIntakePressureSource,
                        FluidLevelAbovePumpSource = x.FluidLevelAbovePumpSource,
                        TubingPressureSource = x.TubingPressureSource,
                        CasingPressureSource = x.CasingPressureSource,
                        FrequencySource = x.FrequencySource,
                        WellHeadTemperatureSource = x.WellHeadTemperatureSource,
                        BottomholeTemperatureSource = x.BottomholeTemperatureSource,
                        OilSpecificGravitySource = x.OilSpecificGravitySource,
                        WaterSpecificGravitySource = x.WaterSpecificGravitySource,
                        GasSpecificGravitySource = x.GasSpecificGravitySource,
                        OilRateSource = x.OilRateSource,
                        WaterRateSource = x.WaterRateSource,
                        GasRateSource = x.GasRateSource,
                        DischargeGagePressureSource = x.DischargeGagePressureSource,
                        MaxRunningFrequency = x.MaxRunningFrequency,
                        MotorLoadPercentage = x.MotorLoadPercentage,
                        FlowingBhp = x.FlowingBhp,
                        WaterSpecificGravity = x.WaterSpecificGravity,
                        TubingId = x.TubingId,
                        WellHeadTemperature = x.WellHeadTemperature,
                        HeadRelativeToPumpCurve = x.HeadRelativeToPumpCurve,
                        HeadRelativeToWellPerformance = x.HeadRelativeToWellPerformance,
                        HeadRelativeToRecommendedRange = x.HeadRelativeToRecommendedRange,
                        FlowRelativeToRecommendedRange = x.FlowRelativeToRecommendedRange,
                        DesignScore = x.DesignScore,
                        PumpDegradation = x.PumpDegradation,
                        Id = x.Id,
                        MaxPotentialProductionRate = x.MaxPotentialProductionRate,
                        MaxPotentialFrequency = x.MaxPotentialFrequency,
                        ProductionIncreasePercentage = x.ProductionIncreasePercentage,
                        UseTVD = x.UseTVD,
                        NumberOfStages = x.NumberOfStages,
                        TestDate = x.TestDate,
                    });
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(SearchESPAnalysisResult)}", correlationId);

                return result.ToList();
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{PlungerLiftTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The name of the coulmn.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{PlungerLiftTrendDataModel}"/>.</returns>
        public IList<PlungerLiftTrendDataModel> GetPlungerLiftTrendData(string nodeId, DateTime startDate,
            DateTime endDate, string name, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetPlungerLiftTrendData)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var response = new List<PlungerLiftTrendDataModel>();
                var names = typeof(PlungerLiftDataHistoryEntity).GetProperties()
                            .Select(property => property.Name)
                            .ToArray();
                name = Regex.Replace(name, @"\s+", "");

                if (names.Contains(name))
                {
                    var result = context.PlungerLiftDataHistory.AsNoTracking().Where(x => x.NodeId == nodeId && x.Date >= startDate
                          && x.Date <= endDate).ToList();
                    response = result.Select(x => new PlungerLiftTrendDataModel
                    {
                        Date = x.Date,
                        Value = x.GetType().GetProperty(name).GetValue(x, null) == null ? null : float.Parse(x.GetType().GetProperty(name).GetValue(x).ToString())
                    }).OrderBy(x => x.Date).ToList();
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetPlungerLiftTrendData)}", correlationId);

                return response;
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{ProductionStatisticsTrendData}"/>.
        /// </summary>
        /// <param name="nodeId">The asset guid.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryItemModel"/>.</returns>
        public DataHistoryItemModel GetDataHistoryTrendDataItems(string nodeId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDataHistoryTrendDataItems)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var nodeData = GetNodeMasterData(new Guid(nodeId));

                if (nodeData == null)
                {
                    return null;
                }

                GetAllComponents(out var failureComponentTrendData, out var failureSubComponentTrendData);

                var eventTrendData = GetEventTrendDataItems();

                var dataHistoryTrendData = new DataHistoryItemModel
                {
                    NodeMasterData = nodeData,
                    EventsTrendData = eventTrendData,
                    FailureComponentTrendData = failureComponentTrendData,
                    FailureSubComponentTrendData = failureSubComponentTrendData,
                    MeterTrendData = GetMeterTrendData(),
                };
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
              $" {nameof(GetDataHistoryTrendDataItems)}", correlationId);

                return dataHistoryTrendData;
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{FailureModel}"/>.
        /// </summary>
        /// <param name="nodeId">The list of node ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="failureComponentId">The failure component id.</param>
        /// <param name="failureSubComponentId">The failure sub component id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryItemModel"/>.</returns>
        public IList<FailureModel> GetFailureComponentItems(string nodeId,
                    DateTime startDate, DateTime endDate,
                    int? failureComponentId, int? failureSubComponentId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetFailureComponentItems)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                IQueryable<FailureEntity> result = context.Failures;

                if (failureComponentId.HasValue)
                {
                    result = result.Where(f => f.ComponentId == failureComponentId);
                }
                if (failureSubComponentId.HasValue)
                {
                    result = result.Where(f => f.SubcomponentId == failureSubComponentId);
                }

                result = result.Where(f => f.NodeId == nodeId);

                result = result.Where(f => f.Date >= startDate);

                result = result.Where(f => f.Date <= endDate);

                var failureData = result.Select(a => new FailureModel
                {
                    Id = a.Id,
                    NodeId = a.NodeId,
                    Date = a.Date,
                    IdentifiedDate = a.IdentifiedDate,
                    RecoveryDate = a.RecoveryDate,
                    RigUpDate = a.RigUpDate,
                    RigDownDate = a.RigDownDate,
                    ComponentId = a.ComponentId,
                    SubcomponentId = a.SubcomponentId,
                    RequestedServiceId = a.RequestedServiceId,
                    MechanismId = a.MechanismId,
                    PullReasonId = a.PullReasonId,
                    ModeId = a.ModeId,
                    Notes = a.Notes,
                    TotalCost = a.TotalCost,
                    Locked = a.Locked,
                });

                foreach (var data in failureData)
                {
                    if (data.ComponentId.HasValue)
                    {
                        data.Component = context.FailureComponent.AsNoTracking().Where(f => f.Id == data.ComponentId)
                            .Select(a => new ComponentItemModel
                            {
                                Id = a.Id,
                                Application = a.ApplicationId,
                                Name = a.Name,
                            }).FirstOrDefault();
                    }

                    if (data.SubcomponentId.HasValue)
                    {
                        data.SubComponent = context.FailureSubComponent.AsNoTracking().Where(f => f.Id == data.SubcomponentId)
                            .Select(a => new ComponentItemModel
                            {
                                Id = a.Id,
                                ComponentId = a.ComponentId,
                                Name = a.Name,
                            }).FirstOrDefault();
                    }
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetFailureComponentItems)}", correlationId);

                return failureData.ToList();
            }
        }

        /// <summary>
        /// Gets the GLAnalysisResultModel data for a node in the given date range.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{GLAnalysisResultModel}"/> object.</returns>
        public IList<GLAnalysisResultModel> SearchGLAnalysisResult(string nodeId,
            DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(SearchGLAnalysisResult)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.GLAnalysisResults.AsNoTracking().Where(x => x.NodeId == nodeId &&
                    x.TestDate >= startDate && x.TestDate <= endDate)
                    .Select(x => new GLAnalysisResultModel
                    {
                        Id = x.Id,
                        NodeId = x.NodeId,
                        ProcessedDate = x.ProcessedDate,
                        TestDate = x.TestDate,
                        GasInjectionDepth = x.GasInjectionDepth,
                        VerticalWellDepth = x.VerticalWellDepth,
                        MeasuredWellDepth = x.MeasuredWellDepth,
                        OilRate = x.OilRate,
                        WaterRate = x.WaterRate,
                        GasRate = x.GasRate,
                        GrossRate = x.GrossRate,
                        WellheadPressure = x.WellheadPressure,
                        CasingPressure = x.CasingPressure,
                        OilSpecificGravity = x.OilSpecificGravity,
                        WaterSpecificGravity = x.WaterSpecificGravity,
                        GasSpecificGravity = x.GasSpecificGravity,
                        TubingId = x.TubingId,
                        TubingOD = x.TubingOD,
                        WellheadTemperature = x.WellheadTemperature,
                        BottomholeTemperature = x.BottomholeTemperature,
                        PercentH2S = x.PercentH2S,
                        PercentN2 = x.PercentN2,
                        PercentCO2 = x.PercentCO2,
                        ProductivityIndex = x.ProductivityIndex,
                        RateAtBubblePoint = x.RateAtBubblePoint,
                        RateAtMaxLiquid = x.RateAtMaxLiquid,
                        RateAtMaxOil = x.RateAtMaxOil,
                        IPRSlope = x.IPRSlope,
                        WaterCut = x.WaterCut,
                        FlowingBhp = x.FlowingBhp,
                        InjectedGLR = x.InjectedGLR,
                        InjectedGasRate = x.InjectedGasRate,
                        MaxLiquidRate = x.MaxLiquidRate,
                        InjectionRateForMaxLiquidRate = x.InjectionRateForMaxLiquidRate,
                        GLRForMaxLiquidRate = x.GLRForMaxLiquidRate,
                        OptimumLiquidRate = x.OptimumLiquidRate,
                        GlrforOptimumLiquidRate = x.GlrforOptimumLiquidRate,
                        MinimumFbhp = x.MinimumFbhp,
                        TubingCriticalVelocity = x.TubingCriticalVelocity,
                        ValveCriticalVelocity = x.ValveCriticalVelocity,
                        ResultMessage = x.ResultMessage,
                        ResultMessageTemplate = x.ResultMessageTemplate,
                        TubingPressureSource = x.TubingPressureSource,
                        CasingPressureSource = x.CasingPressureSource,
                        WellHeadTemperatureSource = x.WellHeadTemperatureSource,
                        BottomholeTemperatureSource = x.BottomholeTemperatureSource,
                        OilSpecificGravitySource = x.OilSpecificGravitySource,
                        WaterSpecificGravitySource = x.WaterSpecificGravitySource,
                        GasSpecificGravitySource = x.GasSpecificGravitySource,
                        OilRateSource = x.OilRateSource,
                        WaterRateSource = x.WaterRateSource,
                        GasRateSource = x.GasRateSource,
                        UseTVD = x.UseTVD,
                        FormationGor = x.FormationGor,
                        ZfactorCorrelationId = x.ZfactorCorrelationId,
                        AdjustedAnalysisToDownholeGaugeReading = x.AdjustedAnalysisToDownholeGaugeReading,
                        AnalysisType = x.AnalysisType,
                        BubblepointPressure = x.BubblepointPressure,
                        CasingId = x.CasingId,
                        DownholeGageDepth = x.DownholeGageDepth,
                        DownholeGagePressure = x.DownholeGagePressure,
                        DownholeGagePressureSource = x.DownholeGagePressureSource,
                        EstimateInjectionDepth = x.EstimateInjectionDepth,
                        FbhpforOptimumLiquidRate = x.FbhpforOptimumLiquidRate,
                        FlowCorrelationId = x.FlowCorrelationId,
                        FlowingBHPAtInjectionDepth = x.FlowingBHPAtInjectionDepth,
                        InjectedGasRateSource = x.InjectedGasRateSource,
                        InjectionRateForOptimumLiquidRate = x.InjectionRateForOptimumLiquidRate,
                        InjectionRateForTubingCriticalVelocity = x.InjectionRateForTubingCriticalVelocity,
                        IsInjectingBelowTubing = x.IsInjectingBelowTubing,
                        KillFluidLevel = x.KillFluidLevel,
                        MeasuredInjectionDepthFromAnalysis = x.MeasuredInjectionDepthFromAnalysis,
                        MultiphaseFlowCorrelationSource = x.MultiphaseFlowCorrelationSource,
                        ReservoirFluidLevel = x.ReservoirFluidLevel,
                        ReservoirPressure = x.ReservoirPressure,
                        VerticalInjectionDepthFromAnalysis = x.VerticalInjectionDepthFromAnalysis,
                    });
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(SearchGLAnalysisResult)}", correlationId);

                return result.ToList();
            }
        }

        /// <summary>
        /// Gets the first injecting flow control device depth.
        /// </summary>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The value of the first injecting flow control device depth.</returns>
        public float? GetFirstInjectingFlowControlDeviceDepth(int analysisResultId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetFirstInjectingFlowControlDeviceDepth)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var valveResult = from wv in context.GLWellValve.AsNoTracking()
                                  join vs in context.GLValveStatus.AsNoTracking() on wv.Id equals vs.GLWellValveId
                                  where vs.GLAnalysisResultId.Equals(analysisResultId)
                                      && vs.IsInjectingGas == true
                                      && vs.ValveState == 1
                                  orderby wv.VerticalDepth
                                  select new { wv.VerticalDepth };

                if (valveResult.FirstOrDefault() != null)
                {
                    return valveResult.First().VerticalDepth;
                }

                var orificeResult = (from wo in context.GLWellOrifice.AsNoTracking()
                                     join os in context.GLOrificeStatus.AsNoTracking() on wo.NodeId equals os.NodeId
                                     where os.GLAnalysisResultId.Equals(analysisResultId)
                                         && os.IsInjectingGas == true
                                     select new { wo.VerticalDepth });
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
              $" {nameof(GetFirstInjectingFlowControlDeviceDepth)}", correlationId);

                return orificeResult.FirstOrDefault() != null ? orificeResult.First().VerticalDepth : null;
            }
        }

        /// <summary>
        /// Gets the value of GLIncludeInjGasInTest system parameter.
        /// </summary>
        /// <returns>The value of GLIncludeInjGasInTest system parameter</returns>
        public string GetGLIncludeInjGasInTestValue(string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetGLIncludeInjGasInTestValue)}",
                correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
                        $" {nameof(GetGLIncludeInjGasInTestValue)}", correlationId);

            return GetSystemParameterData("GLIncludeInjGasInTest");
        }

        /// <summary>
        /// Gets the default trends based on <paramref name="viewId"/>.
        /// </summary>
        /// <param name="viewId">The view id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="List{GraphViewTrendsModel}"/>.</returns>
        public List<GraphViewTrendsModel> GetDefaultTrendsData(string viewId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDefaultTrendsData)}",
                correlationId);

            var resultData = new List<GraphViewTrendsModel>();

            using (var context = _contextFactory.GetContext())
            {
                var trendViewResult = context.GraphViewTrends.AsNoTracking()
                    .Where(gt => gt.ViewId == Convert.ToInt32(viewId))
                    .OrderBy(gt => gt.Axis)
                    .Select(gt => new GraphViewTrendsModel
                    {
                        Source = gt.Source,
                        PocType = gt.PocType,
                        Address = gt.Address,
                        ColumnName = gt.ColumnName,
                        Axis = gt.Axis
                    });

                if (trendViewResult != null)
                {
                    resultData = trendViewResult.ToList();
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
                        $" {nameof(GetDefaultTrendsData)}", correlationId);

            return resultData;
        }

        /// <summary>
        /// Gets the default trends based on <paramref name="viewId"/>.
        /// </summary>
        /// <param name="viewId">The view id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="GraphViewSettingsModel"/>.</returns>
        public GraphViewSettingsModel GetDefaultTrendViewSettings(string viewId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDefaultTrendViewSettings)}",
                correlationId);
            var resultData = new GraphViewSettingsModel();

            using (var context = _contextFactory.GetContext())
            {
                var setttings = context.GraphViewSetting.AsNoTracking()
                    .Where(gs => gs.ViewId == Convert.ToInt32(viewId))
                    .Select(gs => new
                    {
                        gs.ViewId,
                        gs.Property,
                        gs.Value
                    });
                if (setttings != null)
                {
                    resultData.ViewId = Convert.ToInt32(viewId);
                    foreach (var setting in setttings.ToList())
                    {
                        switch (setting.Property)
                        {
                            case "StartDate":
                                resultData.StartDate = setting.Value;
                                break;
                            case "EndDate":
                                resultData.EndDate = setting.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetDefaultTrendViewSettings)}", correlationId);

                return resultData;
            }
        }

        /// <summary>
        /// Gets the default trends views  based on <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="List{GraphViewsModel}"/>.</returns>
        public List<GraphViewsModel> GetDefaultTrendViews(string userId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDefaultTrendViews)}",
                correlationId);

            var resultData = new List<GraphViewsModel>();

            using (var context = _contextFactory.GetContext())
            {
                var defaultView = context.UserDefaults.AsNoTracking()
                    .Where(d => d.UserId == userId
                    && d.DefaultsGroup == "DataHistoryControl" && d.Property == "View");

                var defaultViewData = defaultView?.FirstOrDefault();

                var selectedView = defaultViewData != null ? defaultViewData?.Value : string.Empty;

                var views = context.GraphViews.AsNoTracking()
                    .Where(gs => gs.UserId == userId || gs.UserId.ToLower() == "global")
                    .Select(gs => new GraphViewsModel
                    {
                        UserId = gs.UserId,
                        ViewId = gs.ViewId,
                        ViewName = gs.ViewName,
                        IsSelected = gs.ViewId.ToString() == selectedView,
                        IsGlobal = (gs.UserId.ToLower() == "global"),
                    });

                foreach (var view in views)
                {
                    var settings = context.GraphViewSetting.AsNoTracking()
                        .Where(gs => gs.ViewId == view.ViewId)
                        .Select(gs => new
                        {
                            gs.ViewId,
                            gs.Property,
                            gs.Value
                        });
                    foreach (var setting in settings.ToList())
                    {
                        switch (setting.Property)
                        {
                            case "StartDate":
                                view.StartDate = !string.IsNullOrEmpty(setting.Value) ? Convert.ToDateTime(setting.Value) : null;
                                break;
                            case "EndDate":
                                view.EndDate = !string.IsNullOrEmpty(setting.Value) ? Convert.ToDateTime(setting.Value) : null;
                                break;
                            case "Days":
                                int? days = !string.IsNullOrEmpty(setting.Value) ? Convert.ToInt32(setting.Value) : null;
                                view.StartDate = days != null ? DateTime.UtcNow.AddDays((double)-days) : null;
                                view.EndDate = days != null ? DateTime.UtcNow : null;
                                break;
                            default:
                                break;
                        }
                    }

                    resultData.Add(view);
                }
                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetDefaultTrendViews)}", correlationId);

                return resultData.OrderBy(x => x.ViewName).ToList();
            }
        }

        /// <summary>
        /// Gets the <seealso cref="IList{ControllerTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="address">The address.</param>
        /// <param name="startDate">The Start Date.</param>
        /// <param name="endDate">The End Date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ControllerTrendDataModel}"/>.</returns>
        public async Task<IList<ControllerTrendDataModel>> GetControllerTrendData(string nodeId,
            int address, DateTime startDate, DateTime endDate, string correlationId)
        {

            bool isInfluxEnabled = _configuration.GetValue("EnableInflux", false);
            if (isInfluxEnabled)
            {
                return await _dataHistoryMongoStore.GetControllerTrendData(nodeId, address, startDate, endDate, correlationId);
            }
            else
            {
                return GetControllerTrendDataUsingSQL(nodeId, address, startDate, endDate, correlationId);
            }

        }

        /// <summary>
        /// Gets the downtime by wells.
        /// </summary>
        /// <param name="nodeIds">The node ids.</param>
        /// <param name="numberOfDays">The number of days.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="DowntimeByWellsModel"/>.</returns>
        public Task<DowntimeByWellsModel> GetDowntime(IList<string> nodeIds, int numberOfDays, string correlationId)
        {
            bool isInfluxEnabled = _configuration.GetValue("EnableInflux", false);
            if (isInfluxEnabled)
            {
                return _dataHistoryMongoStore.GetDowntime(nodeIds, numberOfDays, correlationId);
            }
            else
            {
                return GetDowntimeUsingSQL(nodeIds, numberOfDays, correlationId);
            }
        }

        #endregion

        #region Private Methods

        private IList<short> GetNodeID(string nodeId)
        {
            using (var context = _contextFactory.GetContext())
            {
                return context.NodeMasters.AsNoTracking()
                    .Where(x => x.NodeId == nodeId)
                    .Select(x => x.PocType).ToList();
            }

        }

        private int GetNodeIDCase(string nodeId)
        {
            using (var context = _contextFactory.GetContext())
            {
                return context.NodeMasters.AsNoTracking()
                    .Where(x => x.NodeId == nodeId)
                    .Select(x => x.PocType == 17 ? 8 : 0).SingleOrDefault();
            }
        }

        private void GetAllComponents(out IList<ComponentItemModel> failureComponentTrendData,
            out IList<ComponentItemModel> failureSubComponentTrendData)
        {
            using (var context = _contextFactory.GetContext())
            {
                failureComponentTrendData = new List<ComponentItemModel>();
                failureSubComponentTrendData = new List<ComponentItemModel>();

                var failureComponents = context.FailureComponent.AsNoTracking().ToList();

                foreach (var component in failureComponents)
                {
                    failureComponentTrendData.Add(new ComponentItemModel
                    {
                        Id = component.Id,
                        Application = component.ApplicationId,
                        Name = component.Name,
                    });
                }

                var failureSubComponents = context.FailureSubComponent.ToList();

                foreach (var component in failureSubComponents)
                {
                    failureSubComponentTrendData.Add(new ComponentItemModel
                    {
                        Id = component.Id,
                        ComponentId = component.ComponentId,
                        Name = component.Name,
                    });
                }
            }
        }

        private IList<MeterColumnItemModel> GetMeterTrendData()
        {
            var list = new List<MeterColumnItemModel>();
            int[] meterTypeId = { 1, 2, 3 };
            using (var context = _contextFactory.GetContext())
            {
                var result = context.MeterColumn.AsNoTracking().Where(x => meterTypeId.Contains(x.MeterTypeId))
                    .OrderBy(x => x.Name).ToList();

                foreach (var item in result)
                {
                    list.Add(new MeterColumnItemModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Alias = item.Alias,
                        MeterTypeId = item.MeterTypeId,
                        Width = item.Width,
                    });
                }

                return list;
            }
        }

        private IList<PCSFDatalogConfigurationItemModel> GetPCSFDatalogConfiguration(string inputNodeId)
        {
            var list = new List<PCSFDatalogConfigurationItemModel>();
            using (var context = _contextFactory.GetContext())
            {
                var result = context.PCSFDatalogConfiguration.AsNoTracking().Where(x => x.NodeId == inputNodeId).ToList();

                foreach (var item in result)
                {
                    list.Add(new PCSFDatalogConfigurationItemModel
                    {
                        NodeId = item.NodeId,
                        DatalogNumber = item.DatalogNumber,
                        ScheduledScanEnabled = item.ScheduledScanEnabled,
                        OnDemandScanEnabled = item.OnDemandScanEnabled,
                        LastSavedIndex = item.LastSavedIndex,
                        LastSavedDateTime = item.LastSavedDateTime,
                        DatalogName = item.DatalogName,
                        Name1 = item.Name1,
                        Name2 = item.Name2,
                        Name3 = item.Name3,
                        Name4 = item.Name4,
                        Name5 = item.Name5,
                        Name6 = item.Name6,
                        Name7 = item.Name7,
                        Name8 = item.Name8,
                        Name9 = item.Name9,
                        Name10 = item.Name10,
                        Name11 = item.Name11,
                        Name12 = item.Name12,
                        CurrentTransactionCounter = item.CurrentTransactionCounter,
                        IndexOfNewestRecord = item.IndexOfNewestRecord,
                        IndexOfOldestRecord = item.IndexOfOldestRecord,
                        NumberOfRecords = item.NumberOfRecords,
                        MaximumNumberOfRecords = item.MaximumNumberOfRecords,
                        Field2IsWellId = item.Field2IsWellId,
                        NumberOfFields = item.NumberOfFields,
                        IntervalMinutes = item.IntervalMinutes,
                    });
                }

                return list;
            }
        }

        private IList<EventTrendItem> GetEventTrendDataItems()
        {
            IList<EventTrendItem> returnList = new List<EventTrendItem>();

            using (var context = _contextFactory.GetContext())
            {
                var result = context.EventGroups.AsNoTracking()
                    .GroupJoin(context.LocalePhrases.AsNoTracking(), x => x.PhraseId, x => x.PhraseId, (tblEventGroup, tblLocalePhrase) =>
                        new
                        {
                            tblEventGroup,
                            tblLocalePhrase,
                        })
                    .SelectMany(x => x.tblLocalePhrase.DefaultIfEmpty(), (x, tblLocalePhrase) => new
                    {
                        x.tblEventGroup,
                        tblLocalePhrase,
                    })
                    .Select(x => new
                    {
                        Name = (x.tblLocalePhrase != null && x.tblLocalePhrase.English != null)
                            ? x.tblLocalePhrase.English
                            : x.tblEventGroup.Name,
                        x.tblEventGroup.EventTypeId
                    }).ToList();

                foreach (var item in result)
                {
                    returnList.Add(new EventTrendItem
                    {
                        Name = item.Name,
                        EventTypeId = item.EventTypeId
                    });
                }
            }
            return returnList;
        }

        private IList<MeasurementTrendDataModel> GetMeasurementTrendDataUsingSQL(string nodeId,
            int paramStandardType, DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistorySQLStore)} {nameof(GetMeasurementTrendData)}", correlationId);

            IList<MeasurementTrendDataModel> result;
            float maxDecimal = 9999999999999999999999999999f;
            using (var context = _contextFactory.GetContext())
            {
                result = (from d in (from p in context.Parameters.AsNoTracking()
                                     join t in context.FacilityTags.AsNoTracking() on p.Address equals t.Address into pt
                                     from t in pt.DefaultIfEmpty()
                                     where (p.Poctype == (from nm in context.NodeMasters.AsNoTracking()
                                                          where nm.NodeId == nodeId
                                                          select nm.PocType).FirstOrDefault()
                                                          || p.Poctype == 99 ||
                                                          p.Poctype == ((from nm in context.NodeMasters.AsNoTracking()
                                                                         where nm.NodeId == nodeId
                                                                         select nm.PocType).FirstOrDefault() == 17 ? 8 : null))
                                                                         && p.ParamStandardType == @paramStandardType
                                     select new
                                     {
                                         p.Address,
                                         IsManual = p.Address > 4000 && p.Address <= 5000 ? 1 : 0
                                     }).Union(from t in context.FacilityTags.AsNoTracking()
                                              where t.GroupNodeId == nodeId && t.ParamStandardType == @paramStandardType
                                              select new
                                              {
                                                  t.Address,
                                                  IsManual = 0
                                              })
                          join dh in (from dh in context.DataHistory.AsNoTracking()
                                      where dh.NodeID == nodeId && dh.Date >= @startDate && dh.Date <= @endDate
                                      select new
                                      {
                                          dh.Address,
                                          dh.Date,
                                          Value = dh.Value > @maxDecimal ? @maxDecimal : dh.Value
                                      }).Union(from dha in context.DataHistoryArchive.AsNoTracking()
                                               where dha.NodeID == nodeId && dha.Date >= @startDate && dha.Date <= @endDate
                                               select new
                                               {
                                                   dha.Address,
                                                   dha.Date,
                                                   Value = dha.Value > @maxDecimal ? @maxDecimal : dha.Value
                                               }) on d.Address equals dh.Address
                          where (from ft in context.FacilityTags.AsNoTracking()
                                 where ft.GroupNodeId == nodeId &&
                                 ft.ParamStandardType == @paramStandardType
                                 select ft.Address).Count() == 0 ||
                                 ((from ft in context.FacilityTags.AsNoTracking()
                                   where ft.GroupNodeId == nodeId
                                   && ft.ParamStandardType == @paramStandardType
                                   select ft.Address).Contains(d.Address)
                                   || (from p in context.Parameters.AsNoTracking()
                                       where p.Poctype == 99 && p.ParamStandardType == @paramStandardType
                                       select p.Address).Contains(d.Address))
                          orderby dh.Date
                          select new MeasurementTrendDataModel
                          {
                              Address = dh.Address,
                              Date = dh.Date,
                              Value = dh.Value,
                              IsManual = d.IsManual == 1
                          }).Distinct().ToList();

            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)} {nameof(GetMeasurementTrendData)}", correlationId);

            return result;
        }

        private IList<ControllerTrendDataModel> GetControllerTrendDataUsingSQL(string nodeId,
            int address, DateTime startDate, DateTime endDate, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistorySQLStore)} {nameof(GetControllerTrendData)}", correlationId);

            List<ControllerTrendDataModel> listControllerTrendDataModel
                = new List<ControllerTrendDataModel>();
            float maxDecimal = 9999999999999999999999999999f;

            using (var context = _contextFactory.GetContext())
            {
                listControllerTrendDataModel.AddRange(
                    context.DataHistory.AsNoTracking()
                        .Where(x => x.NodeID == nodeId &&
                            x.Address == address &&
                            x.Date >= @startDate && x.Date <= endDate)
                        .Select(x => new ControllerTrendDataModel
                        {
                            Date = x.Date,
                            Value = x.Value > maxDecimal ? maxDecimal : x.Value
                        }).ToList());

                listControllerTrendDataModel.AddRange(
                    context.DataHistoryArchive.AsNoTracking()
                        .Where(x => x.NodeID == nodeId &&
                            x.Address == address &&
                            x.Date >= @startDate && x.Date <= endDate)
                        .Select(x => new ControllerTrendDataModel
                        {
                            Date = x.Date,
                            Value = x.Value > maxDecimal ? maxDecimal : x.Value
                        }).ToList());
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)} {nameof(GetControllerTrendData)}", correlationId);

            return listControllerTrendDataModel.OrderBy(x => x.Date).ToList();
        }

        private IList<MeasurementTrendItemModel> GetMeasurementTrendItemsUsingSQL(string nodeId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistorySQLStore)} {nameof(GetMeasurementTrendItems)}", correlationId);

            IList<MeasurementTrendItemModel> dataModels =
                new List<MeasurementTrendItemModel>();

            string cacheKey = string.Empty;
            using (var context = _contextFactory.GetContext())
            {
                var assetGuid = context.NodeMasters.AsNoTracking()
                     .Where(x => x.NodeId == nodeId)
                     .Select(x => x.AssetGuid).FirstOrDefault();

                cacheKey = $"MeasurementTrendItems::{assetGuid}";
            }

            if (_cache.TryGetValue<IList<MeasurementTrendItemModel>>(cacheKey, out var measurementTrend))
            {
                dataModels = measurementTrend;
            }
            else
            {
                using (var context = _contextFactory.GetContext())
                {
                    var result = (from p in context.Parameters.AsNoTracking()
                                  join t in context.FacilityTags.AsNoTracking()
                                      on p.Address equals t.Address into tagGroup
                                  from t in tagGroup.DefaultIfEmpty()
                                  where (p.Poctype == 99
                                  || GetNodeID(nodeId).Contains((short)p.Poctype)
                                 || p.Poctype == GetNodeIDCase(nodeId)
                                 && t == null
                                 && p.ParamStandardType != null)
                                  select new MeasurementTrendItemModel
                                  {
                                      ParamStandardType = p.ParamStandardType,
                                      Address = p.Address,
                                      Description = p.Description,
                                      PhraseID = p.PhraseId,
                                      ParameterType = p.Poctype == 99 ? "1" : "2"
                                  }).AsEnumerable();

                    result = result.Union(context.FacilityTags.AsNoTracking()
                            .Where(x => x.GroupNodeId == nodeId
                            && x.ParamStandardType != null)
                            .Select(x => new MeasurementTrendItemModel
                            {
                                ParamStandardType = x.ParamStandardType,
                                Address = x.Address,
                                Description = x.Description,
                                PhraseID = null,
                                ParameterType = "2",
                            }).AsEnumerable());

                    var resultDataHistory = context.DataHistory.AsNoTracking().
                                        Where(x => x.NodeID == nodeId)
                                        .Select(x => new MeasurementTrendItemModel
                                        { Address = x.Address }).Distinct().AsEnumerable();

                    resultDataHistory = resultDataHistory.Union
                                        (context.DataHistoryArchive.AsNoTracking()
                                        .Where(x => x.NodeID == nodeId)
                                        .Select(x => new MeasurementTrendItemModel
                                        { Address = x.Address }).Distinct().AsEnumerable());
                    resultDataHistory = resultDataHistory.DistinctBy(x => x.Address);

                    var resultData = (from a in result
                                      join d in resultDataHistory
                                          on a.Address equals d.Address
                                      join p in context.ParamStandardTypes.AsNoTracking()
                                      on a.ParamStandardType equals p.ParamStandardType
                                      join LocalePhrases1 in context.LocalePhrases.AsNoTracking()
                                      on p.PhraseId equals LocalePhrases1.PhraseId
                                      into phraseResult
                                      join LocalePhrases2 in context.LocalePhrases.AsNoTracking()
                                      on a.PhraseID equals LocalePhrases2.PhraseId
                                      into resultPhrase
                                      select new MeasurementTrendItemModel
                                      {
                                          ParamStandardType = p.ParamStandardType,
                                          Name = p.Description,
                                          UnitTypeID = p.UnitTypeId,
                                          Address = a.Address,
                                          Description = a.Description,
                                      }).AsEnumerable();

                    var groupdata = from d in resultData
                                    group d by d.ParamStandardType into g
                                    select new
                                    {
                                        g.Key,
                                        Items = g.Select((item, index) => new { Item = item, Index = index + 1 }) // Assigning row numbers
                                    };

                    dataModels = groupdata.SelectMany(g => g.Items, (g, item) => new MeasurementTrendItemModel
                    {
                        Name = item.Item.Name,
                        ParamStandardType = item.Item.ParamStandardType,
                        UnitTypeID = item.Item.UnitTypeID,
                        Address = item.Item.Address,
                        Description = item.Item.Description,
                    }).DistinctBy(x => x.ParamStandardType)
                    .OrderBy(x => x.Name)
                    .ThenBy(p => p.ParamStandardType)
                    .ThenBy(p => p.Address)
                    .ToList();

                    _cache.Set<object>(cacheKey, dataModels);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)} {nameof(GetMeasurementTrendItems)}", correlationId);

            return dataModels;
        }

        private IList<ControllerTrendItemModel> GetControllerTrendItemsUsingSQL(string nodeId, int pocType, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(DataHistorySQLStore)} {nameof(GetControllerTrendItems)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var query = context.Parameters.AsNoTracking()
                    .Where(p => (pocType == 99 || p.Poctype == pocType) &&
                    p.ParamStandardType == null &&
                    !context.FacilityTags.AsNoTracking().Where(t => t.GroupNodeId == nodeId &&
                    t.Address == p.Address && t.Bit == 0).Any())
                    .Select(p => new ControllerTrendItemModel()
                    {
                        Name = p.Description,
                        Description = context.LocalePhrases
                        .Where(l => l.PhraseId == p.PhraseId)
                        .Select(l => l.English)
                        .FirstOrDefault() ?? p.Description,
                        Address = p.Address,
                        UnitType = p.UnitType,
                        FacilityTag = 0,
                        Tag = null
                    }).AsEnumerable();

                query = query.Union(context.FacilityTags.AsNoTracking()
                    .Where(t => t.Description != null && t.GroupNodeId == nodeId &&
                        t.ParamStandardType == null)
                        .Select(t => new ControllerTrendItemModel()
                        {
                            Name = t.Description,
                            Description = t.Description ?? string.Empty,
                            Address = t.Address,
                            UnitType = t.UnitType,
                            FacilityTag = 1,
                            Tag = t.Tag
                        })).AsEnumerable();

                var result = query.Join(context.DataHistory.AsNoTracking()
                        .Where(dh => dh.NodeID == nodeId)
                        .Select(dh => dh.Address)
                        .Union(context.DataHistoryArchive
                            .Where(dha => dha.NodeID == nodeId)
                            .Select(dha => dha.Address)),
                        p => p.Address,
                        h => h,
                        (p, h) => p)
                    .ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)}" +
               $" {nameof(GetControllerTrendItems)}", correlationId);

                return result;
            }
        }
        private Task<DowntimeByWellsModel> GetDowntimeUsingSQL(IList<string> nodeIds, int numberOfDays, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistorySQLStore)} {nameof(GetDowntime)}", correlationId);

            const int pstFrequency = 2;
            const int pstRunTime = 179;
            const int pstIdleTime = 180;
            const int pstCycles = 181;
            const int pstGasInjectionRate = 191;
            const int applicationRodPump = 3;
            const int applicationESP = 4;
            const int applicationGL = 7;

            var numberOfRecentDays = DateTime.UtcNow.Date.AddDays(-numberOfDays);

            using (var context = _contextFactory.GetContext())
            {
                var queryRunTime = context.DataHistory.AsNoTracking().Where(x => x.Value > 0)
                    .Join(context.Parameters.Where(x => x.ParamStandardType == pstRunTime), dh => dh.Address, p => p.Address, (dh, p) => new
                    {
                        DataHistoryRuntime = dh,
                    });

                var queryIdleTime = context.DataHistory.AsNoTracking()
                    .Join(context.Parameters.Where(x => x.ParamStandardType == pstIdleTime), dh => dh.Address, p => p.Address, (dh, p) => new
                    {
                        DataHistoryIdleTime = dh,
                    });

                var queryCycles = context.DataHistory.AsNoTracking()
                    .Join(context.Parameters.Where(x => x.ParamStandardType == pstCycles), dh => dh.Address, p => p.Address, (dh, p) => new
                    {
                        DataHistoryCycles = dh,
                    });

                var rodPumpResultPrimary = queryRunTime.Join(queryIdleTime, x => new
                {
                    x.DataHistoryRuntime.NodeID,
                    x.DataHistoryRuntime.Date
                }, x => new
                {
                    x.DataHistoryIdleTime.NodeID,
                    x.DataHistoryIdleTime.Date
                }, (x, dh) => new
                {
                    x.DataHistoryRuntime,
                    dh.DataHistoryIdleTime,
                })
                    .Join(queryCycles, x => new
                    {
                        x.DataHistoryRuntime.NodeID,
                        x.DataHistoryRuntime.Date,
                    }, x => new
                    {
                        x.DataHistoryCycles.NodeID,
                        x.DataHistoryCycles.Date,
                    }, (x, dh) => new
                    {
                        x.DataHistoryRuntime,
                        x.DataHistoryIdleTime,
                        dh.DataHistoryCycles
                    })
                    .Where(x => x.DataHistoryRuntime.Date > numberOfRecentDays)
                     .Join(context.NodeMasters.AsNoTracking().Where(x => x.ApplicationId == applicationRodPump && nodeIds.Contains(x.NodeId)), x => x.DataHistoryRuntime.NodeID, nm => nm.NodeId,
                        (x, nm) => new
                        {
                            x.DataHistoryRuntime,
                            x.DataHistoryIdleTime,
                            x.DataHistoryCycles,
                            NodeMaster = nm,
                        })
                    .Select(x => new
                    {
                        NodeId = x.DataHistoryRuntime.NodeID,
                        Runtime = x.DataHistoryRuntime.Value,
                        IdleTime = x.DataHistoryIdleTime.Value,
                        Cycles = x.DataHistoryCycles.Value,
                        x.DataHistoryRuntime.Date
                    })
                    .ToList();

                var rodPumpResult = rodPumpResultPrimary.Distinct().Select(x => new DowntimeByWellsRodPumpModel()
                {
                    Id = x.NodeId,
                    Runtime = x.Runtime,
                    IdleTime = x.IdleTime,
                    Cycles = x.Cycles,
                    Date = x.Date,
                });

                var espResult = context.DataHistory.AsNoTracking().Where(x => nodeIds.Contains(x.NodeID) && x.Date > numberOfRecentDays)
                    .Join(context.NodeMasters.AsNoTracking().Where(x => x.ApplicationId == applicationESP), dh => dh.NodeID, nm => nm.NodeId, (dh, nm) => new
                    {
                        DataHistory = dh,
                        NodeMaster = nm,
                    })
                    .Join(context.Parameters.AsNoTracking().Where(x => x.ParamStandardType == pstFrequency), x => x.DataHistory.Address, p => p.Address, (x, p) => new
                    {
                        x.DataHistory,
                    })
                    .ToList()
                    .Select(x => new DowntimeByWellsValueModel()
                    {
                        Id = x.DataHistory.NodeID,
                        Value = x.DataHistory.Value,
                        Date = x.DataHistory.Date,
                    });

                var glResult = context.DataHistory.AsNoTracking().Where(x => nodeIds.Contains(x.NodeID) && x.Date > numberOfRecentDays)
                    .Join(context.NodeMasters.AsNoTracking().Where(x => x.ApplicationId == applicationGL), dh => dh.NodeID, nm => nm.NodeId, (dh, nm) => new
                    {
                        DataHistory = dh,
                        NodeMaster = nm,
                    })
                    .Join(context.Parameters.AsNoTracking().Where(x => x.ParamStandardType == pstGasInjectionRate), x => x.DataHistory.Address, p => p.Address, (x, p) => new
                    {
                        x.DataHistory,
                    })
                    .ToList()
                    .Select(x => new DowntimeByWellsValueModel()
                    {
                        Id = x.DataHistory.NodeID,
                        Value = x.DataHistory.Value,
                        Date = x.DataHistory.Date,
                    });

                var result = new DowntimeByWellsModel()
                {
                    RodPump = rodPumpResult.ToList(),
                    ESP = espResult.ToList(),
                    GL = glResult.ToList(),
                };

                logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistorySQLStore)} {nameof(GetDowntime)}", correlationId);

                return Task.FromResult(result);
            }

        }
        #endregion
    }
}