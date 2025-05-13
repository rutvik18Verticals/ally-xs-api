using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using ParameterMongo = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Impletments the IRodLiftAnalysis interface
    /// </summary>
    public class RodLiftAnalysisSQLStore : SQLStoreBase, IRodLiftAnalysis
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private const string UPLIFT_OPP_MINIMUM_PRODUCTION_THRESHOLD = "UpliftOppMinimumProductionThreshold";
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IDateTimeConverter _dateTimeConverter;
        private readonly IConfiguration _config;
        private readonly IAllyTimeSeriesNodeMaster _allyNodeMasterStore;
        private readonly IGetDataHistoryItemsService _dataHistoryInfluxStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="RodLiftAnalysisSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="localePhrases">The <seealso cref="ILocalePhrases"/>.</param>
        /// <param name="dateTimeConverter">The date time converter.</param>
        ///  <param name="config">The Configuration</param>
        ///  <param name="allyNodeMasterStore">The mongoDB Store</param>
        ///  <param name="dataHistoryInfluxStore">The Influx Store</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// or
        /// <paramref name="localePhrases"/> is null.
        /// or
        /// </exception>
        public RodLiftAnalysisSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            ILocalePhrases localePhrases, IThetaLoggerFactory loggerFactory, IDateTimeConverter dateTimeConverter, IConfiguration config, IAllyTimeSeriesNodeMaster allyNodeMasterStore,
            IGetDataHistoryItemsService dataHistoryInfluxStore) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _ = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _allyNodeMasterStore = allyNodeMasterStore ?? throw new ArgumentNullException(nameof(allyNodeMasterStore));
            _dataHistoryInfluxStore = dataHistoryInfluxStore ?? throw new ArgumentNullException(nameof(dataHistoryInfluxStore));
        }

        #endregion

        #region IRodLiftAnalysis Implementation

        /// <summary>
        /// Get the rod left analysis data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="cardDateString">The card date.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="RodLiftAnalysisResponse"/>.</returns>
        public RodLiftAnalysisResponse GetRodLiftAnalysisData(Guid assetId, string cardDateString, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisSQLStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

            var cardDate = DateTime.Parse(cardDateString);

            RodLiftAnalysisResponse response = new RodLiftAnalysisResponse();

            using (var context = _contextFactory.GetContext())
            {
                var wellDetail = GetWellDetails(assetId);

                if (wellDetail == null)
                {
                    logger.WriteCId(Level.Info, "Missing well details", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisSQLStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

                    return null;
                }

                response.WellDetails = wellDetail;

                var cardData = GetCardData(assetId, cardDate);

                var wellTest = GetWellTestData(assetId, cardDate);

                var node = GetNodeMasterData(assetId);

                response.NodeMasterData = node;
                response.CardData = cardData;
                response.WellTestData = wellTest;

                var pumpingUnitManufacturer = string.Empty;
                var pumpingUnitAPIDesignation = string.Empty;

                if (wellDetail.PumpingUnitId != null)
                {
                    if (HasCustomPumpingUnit(wellDetail.PumpingUnitId))
                    {
                        var customPumpingUnit = GetCustomPumpingUnits(wellDetail.PumpingUnitId);
                        if (customPumpingUnit != null)
                        {
                            pumpingUnitManufacturer = customPumpingUnit.Manufacturer;
                            pumpingUnitAPIDesignation = customPumpingUnit.APIDesignation;
                        }
                    }
                    else
                    {
                        var result = context.PumpingUnits.AsNoTracking()
                            .GroupJoin(context.PumpingUnitManufacturer.AsNoTracking(),
                                pu => pu.ManufacturerId, pum => pum.ManufacturerAbbreviation,
                                (pumpingUnit, pumpingUnitManufacturer) => new
                                {
                                    PumpingUnit = pumpingUnit,
                                    PumpingUnitManufacturer = pumpingUnitManufacturer,
                                })
                            .SelectMany(x => x.PumpingUnitManufacturer.DefaultIfEmpty(),
                                (x, pumpingUnitManufacturer) => new
                                {
                                    x.PumpingUnit,
                                    PumpingUnitManufacturer = pumpingUnitManufacturer,
                                })
                            .FirstOrDefault(x => x.PumpingUnit.UnitId == wellDetail.PumpingUnitId);

                        if (result?.PumpingUnit != null)
                        {
                            pumpingUnitManufacturer = result.PumpingUnitManufacturer?.ManufacturerAbbreviation;
                            pumpingUnitAPIDesignation = result.PumpingUnit.APIDesignation;
                        }
                    }
                }

                response.PumpingUnitManufacturer = pumpingUnitManufacturer;
                response.PumpingUnitAPIDesignation = pumpingUnitAPIDesignation;

                response.XDiagResults = GetXDiagResultData(assetId, cardDate);

                // Influx enabled
                if (bool.TryParse(_config.GetSection("EnableInflux").Value, out bool isInfluxEnabled) && isInfluxEnabled)
                {
                    response.CurrentRawScanData = GetCurrentRawScanData(assetId, node.NodeId, correlationId);
                }
                else
                {
                    response.CurrentRawScanData = GetCurrentRawScanData(assetId);
                }

                var upliftOppMinimumProductionThresholdValue
                    = GetSystemParameterData(UPLIFT_OPP_MINIMUM_PRODUCTION_THRESHOLD);
                response.SystemParameters =
                    string.IsNullOrEmpty(upliftOppMinimumProductionThresholdValue)
                        ? "3"
                        : upliftOppMinimumProductionThresholdValue;

                response.CardType = cardData != null ? cardData.CardType : string.Empty;
                response.CauseId = cardData?.CauseId;
                response.PocType = node.PocType;

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisSQLStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

                return response;
            }
        }

        /// <summary>
        /// Get the card dates by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{CardDateModel}"/>.</returns>
        public IList<CardDateModel> GetCardDatesByAssetId(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisSQLStore)} {nameof(GetCardDatesByAssetId)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                List<CardDateModel> result = context.NodeMasters.AsNoTracking()
                    .Join(context.CardData.AsNoTracking(),
                        nm => nm.NodeId, cd => cd.NodeId, (nm, cd) => new
                        {
                            NodeMaster = nm,
                            CardDate = cd
                        })
                    .Where(x => x.NodeMaster.AssetGuid == assetId)
                    .Select(x => new CardDateModel()
                    {
                        Date = x.CardDate.CardDate,
                        CauseId = x.CardDate.CauseId,
                        CardTypeId = x.CardDate.CardType,
                        PocType = x.NodeMaster.PocType
                    })
                    .OrderByDescending(x => x.Date)
                    .ToList();

                foreach (var card in result)
                {
                    card.Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(card.Date, correlationId, LoggingModel.SQLStore);
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisSQLStore)} {nameof(GetCardDatesByAssetId)}", correlationId);

                return result;
            }
        }

        #endregion

        #region Private Methods

        private bool HasCustomPumpingUnit(string pumpingUnitId)
        {
            if (string.IsNullOrWhiteSpace(pumpingUnitId))
            {
                return false;
            }

            return pumpingUnitId.IndexOf("~X") == 0;
        }

        /// <summary>
        /// Create Same Trend Data Response from Influx OSS/Enterprise.
        /// </summary>
        /// <param name="trendData"></param>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        private List<DataPointModel> CreateTrendDataResponseFromInflux(IList<DataPointModel> trendData, List<string> channelIds = null)
        {
            var responseData = new List<DataPointModel>();
            if (trendData != null && trendData.Count > 0)
            {
                if (trendData.Any(a => a.ColumnValues != null))
                {
                    // If channelIds is null, get all keys from ColumnValues
                    var channelsToProcess = channelIds ?? trendData
                        .Where(a => a.ColumnValues != null)
                        .SelectMany(a => a.ColumnValues.Keys)
                        .Distinct()
                        .ToList();

                    foreach (var channel in channelsToProcess)
                    {
                        var data = trendData
                            .Where(a => a.ColumnValues != null &&
                                                           a.ColumnValues[channel]?.ToString() != null)
                            .Select(x => new DataPointModel()
                            {
                                Time = x.Time,
                                Value = decimal.Parse(x.ColumnValues[channel]),
                                TrendName = channel,
                                POCTypeId = x.POCTypeId
                            }).ToList();
                        responseData.AddRange(data);
                    }
                }
                else
                {
                    responseData = trendData
                    .Where(a => a.Value != null)
                    .Select(x => new DataPointModel()
                    {
                        Time = x.Time,
                        Value = decimal.Parse(x.Value.ToString()),
                        TrendName = x.TrendName,
                        POCTypeId = x.POCTypeId
                    }).ToList();
                }
            }

            return responseData.OrderBy(a => a.Time).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crsdi"></param>
        /// <param name="nodeId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private IList<CurrentRawScanDataModel> MapCurrentRawScanData(List<DataPointModel> crsdi, string nodeId,
            IDictionary<(int POCType, string ChannelId), ParameterMongo.Parameters> parameters)
        {
            IList<CurrentRawScanDataModel> currentRawScanData = new List<CurrentRawScanDataModel>();

            foreach (var item in crsdi)
            {
                var key = (POCType: int.Parse(item.POCTypeId), item.TrendName);
                var parameter = parameters.TryGetValue(key, out var param) ? param : null;

                var crsm = new CurrentRawScanDataModel
                {
                    NodeId = nodeId,
                    Value = float.TryParse(item.Value?.ToString(), out var floatVal) ? floatVal : null,
                    DateTimeUpdated = item?.Time,
                    Address = parameter != null ? parameter.Address : 0,
                    StringValue = string.Empty // Need to Check
                };

                currentRawScanData.Add(crsm);
            }

            return currentRawScanData;
        }

        /// <summary>
        /// Get the current raw scan data based on asset id
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="nodeId"></param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataModel}" /></returns>
        private IList<CurrentRawScanDataModel> GetCurrentRawScanData(Guid assetId, string nodeId, string correlationId)
        {
            // Declare variable.
            IList<CurrentRawScanDataModel> currentRawScanData = new List<CurrentRawScanDataModel>();

            // Get asset data.
            var assetData = _allyNodeMasterStore.GetAssetAsync(assetId, correlationId).GetAwaiter().GetResult();
            if (assetData != null)
            {
                var customerObjId = assetData?.CustomerId;
                var customerData = _allyNodeMasterStore.GetCustomerAsync(customerObjId, correlationId).GetAwaiter().GetResult();

                // Get customer data.
                var customerGUID = Guid.TryParse(customerData?.LegacyId?["CustomerGUID"], out var parsedGuid)
                    ? parsedGuid
                    : Guid.Empty;

                var latestTrendDataRecords = _dataHistoryInfluxStore.GetCurrentRawScanData(assetId, customerGUID).Result;
                if (latestTrendDataRecords != null && latestTrendDataRecords.Count > 0)
                {
                    // Trend data Response updated.
                    var trendDataResultSet = CreateTrendDataResponseFromInflux(latestTrendDataRecords);

                    // Get Parameter key based on poctype and ChannelId/TrendName.
                    var parameterKeys = trendDataResultSet.Select(item => (POCType: int.Parse(item.POCTypeId), ChannelId: item.TrendName)).Distinct().ToList();

                    // Get parameters from MongoDB.
                    var parameters = _allyNodeMasterStore.GetParametersBulk(parameterKeys, correlationId).GetAwaiter().GetResult();

                    // Map the trend data to the current raw scan data.
                    currentRawScanData = MapCurrentRawScanData(trendDataResultSet, nodeId, parameters);
                }
            }

            return currentRawScanData;
        }
        #endregion

    }
}