using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Common;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Utilities;
using UnitCategory = Theta.XSPOC.Apex.Api.Core.Common.UnitCategory;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IGroupStatusService.
    /// </summary>
    public class GroupStatusProcessingService : IGroupStatusProcessingService
    {

        #region Private Dependencies

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IGroupStatus _groupStatus;
        private readonly IParameterDataType _parameterDataType;
        private readonly INodeMaster _nodeMasterStore;
        private readonly IColumnFormatterFactory _columnFormatterFactory;
        private readonly ILocalePhrases _phraseStore;
        private readonly IGroupAndAsset _groupAndAssetService;
        private readonly IDataHistorySQLStore _trendDataStore;
        private readonly IGetDataHistoryItemsService _dataHistoryInfluxStore;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IParameterMongoStore _parameterStore;
        private readonly ICommonService _commonService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GroupStatusProcessingService"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="groupStatus">The <seealso cref="IGroupStatus"/> service.</param>
        /// <param name="parameterDataType">The <seealso cref="IParameterDataType"/> service.</param>
        /// <param name="nodeMasterStore">The <see cref="INodeMaster"/>.</param>
        /// <param name="columnFormatterFactory">The <see cref="IColumnFormatterFactory"/>.</param>
        /// <param name="phraseStore">The <see cref="ILocalePhrases"/>.</param>
        /// <param name="groupAndAssetService">The <seealso cref="IGroupAndAsset"/> service.</param>
        /// <param name="trendDataStore">The <see cref="IDataHistorySQLStore"/>.</param>
        /// <param name="dataHistoryInfluxStore">The <see cref="IGetDataHistoryItemsService"/>.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory"/>.</param>
        /// <param name="parameterStore">The <seealso cref="IParameterMongoStore"/> service.</param>
        /// <param name="commonService">The <see cref="ICommonService"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="loggerFactory"/>, <paramref name="groupStatus"/>, <paramref name="parameterDataType"/>,
        /// <paramref name="nodeMasterStore"/>, <paramref name="columnFormatterFactory"/>, or <paramref name="phraseStore"/> is null,
        /// or <paramref name="trendDataStore"/> is null or <paramref name="dataHistoryInfluxStore"/> is null or
        /// <paramref name="groupAndAssetService"/> is null or <paramref name="parameterStore"/> is null.
        /// </exception>
        public GroupStatusProcessingService(IThetaLoggerFactory loggerFactory, IGroupStatus groupStatus,
            IParameterDataType parameterDataType, INodeMaster nodeMasterStore, IColumnFormatterFactory columnFormatterFactory,
            ILocalePhrases phraseStore, IGroupAndAsset groupAndAssetService, IDataHistorySQLStore trendDataStore,
            IGetDataHistoryItemsService dataHistoryInfluxStore, IServiceScopeFactory serviceScopeFactory,
            IParameterMongoStore parameterStore, ICommonService commonService)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _groupStatus = groupStatus ?? throw new ArgumentNullException(nameof(groupStatus));
            _parameterDataType = parameterDataType ?? throw new ArgumentNullException(nameof(parameterDataType));
            _nodeMasterStore = nodeMasterStore ?? throw new ArgumentNullException(nameof(nodeMasterStore));
            _columnFormatterFactory = columnFormatterFactory ?? throw new ArgumentNullException(nameof(columnFormatterFactory));
            _phraseStore = phraseStore ?? throw new ArgumentNullException(nameof(phraseStore));
            _groupAndAssetService = groupAndAssetService ?? throw new ArgumentNullException(nameof(groupAndAssetService));
            _trendDataStore = trendDataStore ?? throw new ArgumentNullException(nameof(trendDataStore));
            _dataHistoryInfluxStore = dataHistoryInfluxStore ?? throw new ArgumentNullException(nameof(dataHistoryInfluxStore));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _parameterStore = parameterStore ?? throw new ArgumentNullException(nameof(parameterStore));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
        }

        #endregion

        #region IGroupStatusService Implementation

        /// <summary>
        /// Processes the provided group status request based on that data.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="GroupStatusInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusOutput"/>.</returns>
        public GroupStatusOutput GetGroupStatus(WithCorrelationId<GroupStatusInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusProcessingService)} {nameof(GetGroupStatus)}", requestWithCorrelationId?.CorrelationId);

            GroupStatusOutput output = new GroupStatusOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (requestWithCorrelationId == null)
            {
                var message = $"Correlation Id is null.";
                logger.Write(Level.Info, message);
                output.Result.Status = false;
                output.Result.Value = message;

                return output;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get group status.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            if (string.IsNullOrWhiteSpace(requestWithCorrelationId.Value.ViewId))
            {
                var message = "Missing view id";

                logger.WriteCId(Level.Info, message, requestWithCorrelationId.CorrelationId);
                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            var groupStatusRequest = requestWithCorrelationId.Value;

            var assets = GetAssetsInGroup(groupStatusRequest.GroupName, requestWithCorrelationId.CorrelationId);

            var nodeIds = assets.Select(x => x.AssetName).ToArray();

            Dictionary<int, ParameterDataTypeModel> parameterDataType = new();
            SortedList<string, FacilityTag> facilityTagAddress = new();

            var dataTypesModels = _parameterDataType.GetItems(requestWithCorrelationId.CorrelationId);

            List<ParameterDataTypeModel> parameterDataTypeModel = new List<ParameterDataTypeModel>();

            foreach (var item in dataTypesModels)
            {
                parameterDataTypeModel.Add(new ParameterDataTypeModel(item.DataType, item.Description));
            }

            var num1 = parameterDataTypeModel.Count - 1;
            var index = 0;
            parameterDataType.Clear();
            while (index <= num1)
            {
                parameterDataType.Add(parameterDataTypeModel[index].DataType, parameterDataTypeModel[index]);

                index++;
            }

            var parameterList = _groupStatus.LoadViewParameters(groupStatusRequest.ViewId, requestWithCorrelationId.CorrelationId);
            var facilityTag = _groupStatus.GetItemsGroupStatus(nodeIds, requestWithCorrelationId.CorrelationId);

            facilityTagAddress.Clear();
            foreach (var item in facilityTag)
            {
                var domain = FacilityTag.Map(item);

                facilityTagAddress.Add(domain.KeyValue, domain);
            }

            var conditionalFormatsData = _groupStatus.GetConditionalFormats(groupStatusRequest.ViewId, requestWithCorrelationId.CorrelationId);

            var conditionalFormats = MapConditionalFormats(conditionalFormatsData);

            var groupStatusColumnsModels = _groupStatus.LoadViewColumns(groupStatusRequest.ViewId, requestWithCorrelationId.CorrelationId);

            SortedList<int, GroupStatusColumns> viewColumns = MapGroupStatusColumnsModels(groupStatusColumnsModels,
                parameterList, parameterDataType, facilityTagAddress, conditionalFormats, requestWithCorrelationId.CorrelationId);

            var result = LoadViewRows(nodeIds.ToArray(), viewColumns, requestWithCorrelationId.CorrelationId);

            output = new GroupStatusOutput()
            {
                Values = new GroupStatusValues()
                {
                    Columns = result.ColumnOverrides.Join(result.GroupStatusColumns, co => co.ColumnId, gsc => gsc.ColumnId,
                            (columnOverrides, groupStatusColumn) => new
                            {
                                columnOverrides,
                                groupStatusColumn,
                            }).OrderBy(x => x.groupStatusColumn.Position)
                        .Select(x => new GroupStatusColumn()
                        {
                            Id = x.columnOverrides.ColumnId,
                            Name = x.columnOverrides.Caption ?? x.groupStatusColumn.Name,
                        }).ToList(),
                    Rows = new List<GroupStatusRow>()
                }
            };
            //add asset guid group status column
            output.Values.Columns.Add(new GroupStatusColumn()
            {
                Id = -1,
                Name = "AssetGuid",
            });

            output.Values.Rows.AddRange(result.Rows.Select(row => new GroupStatusRow
            {
                Columns = row.Columns.Select(x => new GroupStatusRowColumn
                {
                    ColumnId = x.ColumnId,
                    Value = x.Value,
                    BackColor = x.BackColor,
                    ForeColor = x.ForeColor
                }).ToList()
            }));

            //add asset guid value for each if the row 
            foreach (var row in output.Values.Rows)
            {
                var nodeId = row.Columns.FirstOrDefault(a => a.ColumnId == 1)?.Value;

                var node = assets.FirstOrDefault(a => a.AssetName == nodeId);
                if (node != null)
                {
                    row.Columns.Add(new GroupStatusRowColumn
                    {
                        ColumnId = -1,
                        Value = node.AssetId.ToString(),
                        BackColor = null,
                        ForeColor = null,
                    });
                }
                else
                {
                    row.Columns.Add(new GroupStatusRowColumn
                    {
                        ColumnId = -1,
                        Value = null,
                        BackColor = null,
                        ForeColor = null,
                    });
                }
            }

            output.Result = new MethodResult<string>(true, string.Empty);
            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusProcessingService)} {nameof(GetGroupStatus)}", requestWithCorrelationId?.CorrelationId);

            return output;
        }

        /// <summary>
        /// Processes the provided user id request and generates available views  based on that data.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="AvailableViewInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="AvailableViewOutput"/>.</returns>
        public AvailableViewOutput GetAvailableViews(WithCorrelationId<AvailableViewInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusProcessingService)} {nameof(GetAvailableViews)}", requestWithCorrelationId?.CorrelationId);
            AvailableViewOutput availableViewData = new AvailableViewOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };
            if (requestWithCorrelationId == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get available views.";
                logger.Write(Level.Info, message);
                availableViewData.Result.Status = false;
                availableViewData.Result.Value = message;

                return availableViewData;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get available views.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                availableViewData.Result = new MethodResult<string>(false, message);

                return availableViewData;
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;
            var request = requestWithCorrelationId.Value;

            if (string.IsNullOrEmpty(request.UserId))
            {
                var message = $"Missing {nameof(request.UserId)}.";

                logger.WriteCId(Level.Info, message, correlationId);
                availableViewData.Result = new MethodResult<string>(false, message);

                return availableViewData;
            }

            var result = _groupStatus.GetAvailableViewsByUserId(request.UserId, requestWithCorrelationId.CorrelationId);

            availableViewData.Values = new List<AvailableViewData>();
            foreach (var view in result)
            {
                availableViewData.Values.Add(AvailableViewDataMapper.MapToDomainObject(view));
            }

            logger.WriteCId(Level.Debug, $"Retrieved available views for {request.UserId}", correlationId);
            availableViewData.Result = new MethodResult<string>(true, string.Empty);
            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusProcessingService)} {nameof(GetAvailableViews)}", requestWithCorrelationId?.CorrelationId);

            return availableViewData;
        }

        /// <summary>
        /// Gets the classification data for the group.
        /// </summary>
        /// <param name="request">The <seealso cref="GroupStatusInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusWidgetOutput"/></returns>
        public GroupStatusWidgetOutput GetClassificationWidgetData(WithCorrelationId<GroupStatusInput> request)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusProcessingService)}" +
                $" {nameof(GetClassificationWidgetData)}", request?.CorrelationId);

            var output = new GroupStatusWidgetOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (request == null)
            {
                var message = $"{nameof(request)} is null, cannot get group widgets.";
                logger.Write(Level.Info, message);
                output.Result.Status = false;
                output.Result.Value = message;

                return output;
            }

            if (request?.Value == null)
            {
                var message = $"{nameof(request)} is null, cannot get group widgets.";
                logger.WriteCId(Level.Info, message, request?.CorrelationId);
                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            var groupStatusRequest = request.Value;

            var assetIds = GetAssetIdsInGroup(groupStatusRequest.GroupName, request.CorrelationId);

            if (assetIds == null)
            {
                var message = $"{nameof(assetIds)} is null, cannot get group widgets.";
                logger.WriteCId(Level.Info, message, request?.CorrelationId);
                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            if (assetIds != null && assetIds.Length >= 0)
            {
                var nodeIds = _nodeMasterStore.GetNodeIdsByAssetGuid(assetIds, request.CorrelationId);

                if (nodeIds == null)
                {
                    var message = $"{nameof(nodeIds)} is null, cannot get group widgets.";
                    logger.WriteCId(Level.Info, message, request?.CorrelationId);
                    output.Result = new MethodResult<string>(false, message);

                    return output;
                }

                var classificationsData = _groupStatus.GetClassificationsData(nodeIds.Select(x => x.NodeId).ToList(),
                    request.CorrelationId, out int assetCount);

                if (classificationsData == null)
                {
                    var message = $"{nameof(classificationsData)} is null, cannot get group widgets.";
                    logger.WriteCId(Level.Info, message, request?.CorrelationId);
                    output.Result = new MethodResult<string>(false, message);

                    return output;
                }

                output = new GroupStatusWidgetOutput();

                if (classificationsData != null)
                {
                    output.ClassificationValues =
                        classificationsData?.OrderByDescending(x => x.Count).Select(x => new GroupStatusClassification()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Count = x.Count,
                            Percent = Common.MathUtility.RoundToSignificantDigits(x.Percent, 2),
                            Priority = x.Priority
                        }).OrderBy(x => x.Priority).ToList();
                    output.AssetCount = assetCount;
                }

                output.Result = new MethodResult<string>(true, string.Empty);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusProcessingService)}" +
                $" {nameof(GetClassificationWidgetData)}", request?.CorrelationId);

            return output;
        }

        /// <summary>
        /// Gets the alarms data for the group.
        /// </summary>
        /// <param name="request">The <seealso cref="GroupStatusInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusKPIOutput"/></returns>
        public async Task<GroupStatusKPIOutput> GetAlarmsWidgetDataAsync(WithCorrelationId<GroupStatusInput> request)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusProcessingService)}" +
                $" {nameof(GetAlarmsWidgetDataAsync)}", request?.CorrelationId);

            var output = new GroupStatusKPIOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (request == null)
            {
                var message = $"{nameof(request)} is null, cannot get group widgets.";
                logger.Write(Level.Info, message);
                output.Result.Status = false;
                output.Result.Value = message;

                return output;
            }

            if (request?.Value == null)
            {
                var message = $"{nameof(request)} is null, cannot get group widgets.";
                logger.WriteCId(Level.Info, message, request?.CorrelationId);
                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            var groupstatusRequest = request.Value;

            var assetIds = GetAssetIdsInGroup(groupstatusRequest.GroupName, request.CorrelationId);

            if (assetIds == null)
            {
                var message = $"{nameof(assetIds)} is null, cannot get group widgets.";
                logger.WriteCId(Level.Info, message, request?.CorrelationId);
                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            if (assetIds != null && assetIds.Length >= 0)
            {
                var nodeIds = _nodeMasterStore.GetNodeIdsByAssetGuid(assetIds, request.CorrelationId);

                if (nodeIds == null)
                {
                    var message = $"{nameof(nodeIds)} is null, cannot get group widgets.";
                    logger.WriteCId(Level.Info, message, request?.CorrelationId);
                    output.Result = new MethodResult<string>(false, message);

                    return output;
                }

                var alarmData = await _groupStatus.GetAlarmsData(nodeIds.Select(x => x.NodeId).ToList(), request.CorrelationId);

                if (alarmData == null)
                {
                    var message = $"{nameof(alarmData)} is null, cannot get group widgets.";
                    logger.WriteCId(Level.Info, message, request?.CorrelationId);
                    output.Result = new MethodResult<string>(false, message);

                    return output;
                }

                output = new GroupStatusKPIOutput();
                if (alarmData != null)
                {
                    output.Values = new List<GroupStatusKPIValues>
                    {
                        new() {
                            Id = alarmData.Id.ToString(),
                            Name = alarmData.Name,
                            Count = alarmData.Count,
                            Percent = Common.MathUtility.RoundToSignificantDigits(alarmData.Percent, 2)
                        }
                    };
                }

                output.Result = new MethodResult<string>(true, string.Empty);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusProcessingService)}" +
                $" {nameof(GetAlarmsWidgetDataAsync)}", request?.CorrelationId);

            return output;
        }

        /// <summary>
        /// Gets the downtime data for the wells in the group.
        /// </summary>
        /// <param name="input">The <seealso cref="GroupStatusInput"/> with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusKPIOutput"/></returns>
        public async Task<GroupStatusDowntimeByWellOutput> GetDowntimeByWellsAsync(WithCorrelationId<GroupStatusInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusProcessingService)}" +
                $" {nameof(GetDowntimeByWellsAsync)}", input?.CorrelationId);

            var output = new GroupStatusDowntimeByWellOutput();

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get downtime by wells.";

                logger.Write(Level.Info, message);

                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            if (input.Value == null)
            {
                var message = $"{nameof(input)} value is null, cannot get downtime by wells.";

                logger.WriteCId(Level.Info, message, input.CorrelationId);

                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(input.CorrelationId);

            const int numberOfDays = 7;
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-numberOfDays);

            var assets = GetAssetsInGroup(input.Value.GroupName, input.CorrelationId);

            logger.WriteCId(Level.Debug, $"Number of assets in group ({input.Value.GroupName}): {assets.Count}",
                input.CorrelationId);

            if (assets.Count == 0)
            {
                output.Result = new MethodResult<string>(true, string.Empty);

                return output;
            }

            var assetsWithLegacyWells = await _nodeMasterStore.GetLegacyWellAsync(assets.Select(x => x.AssetId).ToList(), input.CorrelationId);

            var groupStatusAssets = assets.Select(x => new GroupStatusAssetModel()
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                IndustryApplicationId = x.IndustryApplicationId,
            }).ToList();

            foreach (var asset in groupStatusAssets)
            {
                if (assetsWithLegacyWells.TryGetValue(asset.AssetId, out var isLegacyWell))
                {
                    asset.IsLegacyWell = isLegacyWell;
                }
            }

            var legacyWellAssets = groupStatusAssets.Where(x => x.IsLegacyWell).ToList();
            var downTimeByWells = _trendDataStore.GetDowntime(legacyWellAssets.Select(x => x.AssetName).ToList(), numberOfDays, input.CorrelationId);

            var nonLegacyWellAssets = groupStatusAssets.Where(x => x.IsLegacyWell == false &&
                (x.IndustryApplicationId == IndustryApplication.RodArtificialLift.Key ||
                    x.IndustryApplicationId == IndustryApplication.ESPArtificialLift.Key ||
                    x.IndustryApplicationId == IndustryApplication.GasArtificialLift.Key)).ToList();

            var nonLegacyWellsNodeMasterData =
                await _nodeMasterStore.GetByAssetIdsAsync(nonLegacyWellAssets.Select(x => x.AssetId).ToList(), input.CorrelationId);

            if (nonLegacyWellsNodeMasterData == null)
            {
                output.Result = new MethodResult<string>(true, string.Empty);

                return output;
            }

            var applicationPOCTypeData = nonLegacyWellsNodeMasterData.Select(x => new
            {
                x.ApplicationId,
                x.PocType,
            }).Distinct().ToList();

            const int pstFrequency = 2;
            const int pstRunTime = 179;
            const int pstIdleTime = 180;
            const int pstCycles = 181;
            const int pstGasInjectionRate = 191;

            var measurementDataWithChannelIdForInflux = new List<DowntimeFiltersWithChannelIdInflux>();

            foreach (var item in applicationPOCTypeData)
            {
                var assetsIds = nonLegacyWellsNodeMasterData.Where(x => x.PocType == item.PocType).Select(x => x.AssetGuid)
                    .ToList();
                var customerIds = nonLegacyWellsNodeMasterData.Where(x => x.PocType == item.PocType)
                    .Select(x => x.CompanyGuid ?? Guid.Empty).Distinct().ToList();

                if (assetsIds.Count == 0 || customerIds.Count == 0)
                {
                    continue;
                }

                var data = new DowntimeFiltersInflux()
                {
                    POCType = item.PocType.ToString(),
                    AssetIds = assetsIds,
                    CustomerIds = customerIds,
                };

                if (item.ApplicationId == IndustryApplication.RodArtificialLift.Key)
                {
                    data.ParamStandardType = new List<string>()
                    {
                        pstRunTime.ToString(),
                        pstIdleTime.ToString(),
                        pstCycles.ToString(),
                    };
                }
                else if (item.ApplicationId == IndustryApplication.ESPArtificialLift.Key)
                {
                    data.ParamStandardType = new List<string>()
                    {
                        pstFrequency.ToString(),
                    };
                }
                else if (item.ApplicationId == IndustryApplication.GasArtificialLift.Key)
                {
                    data.ParamStandardType = new List<string>()
                    {
                        pstGasInjectionRate.ToString(),
                    };
                }

                measurementDataWithChannelIdForInflux.Add(GetChannelId(data, input.CorrelationId));
            }

            var resultInflux = await _dataHistoryInfluxStore.GetDowntimeAsync(measurementDataWithChannelIdForInflux,
                startDate.ToString("yyyy-MM-ddTHH:mm:ss"), endDate.ToString("yyyy-MM-ddTHH:mm:ss"));

            foreach (var assetId in resultInflux.Select(x => x.Id).Distinct().ToList())
            {
                var applicationId = nonLegacyWellsNodeMasterData.First(x => x.AssetGuid == Guid.Parse(assetId)).ApplicationId;

                if (applicationId.HasValue == false)
                {
                    continue;
                }

                if (applicationId.Value == IndustryApplication.RodArtificialLift.Key)
                {
                    var dataHistoryRuntime = resultInflux
                        .Where(x => x.Id == assetId && x.ParamStandardType == pstRunTime.ToString()).ToList();
                    var dataHistoryIdleTime = resultInflux
                        .Where(x => x.Id == assetId && x.ParamStandardType == pstIdleTime.ToString()).ToList();
                    var dataHistoryCycles = resultInflux
                        .Where(x => x.Id == assetId && x.ParamStandardType == pstCycles.ToString()).ToList();

                    var downTimeRodPump = dataHistoryRuntime.Where(x => x.Value > 0).Join(dataHistoryIdleTime,
                            dh179 => dh179.Date,
                            dh180 => dh180.Date,
                            (dh179, dh180) => new
                            {
                                DataHistoryRunTime = dh179,
                                DataHistoryIdleTime = dh180,
                            })
                        .Join(dataHistoryCycles, dh => dh.DataHistoryRunTime.Date, dh181 => dh181.Date, (x, dh181) => new
                        {
                            x.DataHistoryRunTime,
                            x.DataHistoryIdleTime,
                            DataHistoryCycles = dh181,
                        })
                        .Select(x => new DowntimeByWellsRodPumpModel()
                        {
                            Id = groupStatusAssets.FirstOrDefault(gsa =>
                                    Guid.TryParse(x.DataHistoryRunTime.Id, out var parsedAssetId) && gsa.AssetId == parsedAssetId)
                                ?.AssetName,
                            Runtime = x.DataHistoryRunTime.Value,
                            IdleTime = x.DataHistoryIdleTime.Value,
                            Cycles = x.DataHistoryCycles.Value,
                            Date = x.DataHistoryRunTime.Date,
                        })
                        .Distinct()
                        .ToList();

                    downTimeByWells.RodPump.AddRange(downTimeRodPump);
                }
                else if (applicationId.Value == IndustryApplication.ESPArtificialLift.Key)
                {
                    var dataHistoryFrequency = resultInflux
                        .Where(x => x.Id == assetId && x.ParamStandardType == pstFrequency.ToString()).ToList();

                    foreach (var item in dataHistoryFrequency)
                    {
                        item.Id = groupStatusAssets.FirstOrDefault(gsa =>
                                Guid.TryParse(item.Id, out var parsedAssetId) && gsa.AssetId == parsedAssetId)
                            ?.AssetName;
                    }

                    downTimeByWells.ESP.AddRange(dataHistoryFrequency.Select(x => new DowntimeByWellsValueModel()
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Value = x.Value,
                    }));
                }
                else if (applicationId.Value == IndustryApplication.GasArtificialLift.Key)
                {
                    var dataHistoryGasInjectionRate = resultInflux
                        .Where(x => x.Id == assetId && x.ParamStandardType == pstGasInjectionRate.ToString()).ToList();

                    foreach (var item in dataHistoryGasInjectionRate)
                    {
                        item.Id = groupStatusAssets.FirstOrDefault(gsa =>
                                Guid.TryParse(item.Id, out var parsedAssetId) && gsa.AssetId == parsedAssetId)
                            ?.AssetName;
                    }

                    downTimeByWells.GL.AddRange(dataHistoryGasInjectionRate.Select(x => new DowntimeByWellsValueModel()
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Value = x.Value,
                    }));
                }
            }

            var list = new GroupStatusDowntimeByWell()
            {
                Assets = new List<GroupStatusKPIValues>(),
                GroupByDuration = new List<GroupStatusKPIValues>(),
            };

            UpdateDowntimeByWellsKPIValues(numberOfDays, assets, downTimeByWells.RodPump, ref list);
            UpdateDowntimeByWellsKPIValues(numberOfDays, assets, downTimeByWells.ESP, ref list);
            UpdateDowntimeByWellsKPIValues(numberOfDays, assets, downTimeByWells.GL, ref list);
            AddDowntimeByWellsGrouping(ref list, digits);

            list.Assets = list.Assets.OrderByDescending(x => x.Count).ToList();

            output = new GroupStatusDowntimeByWellOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                Assets = list.Assets,
                GroupByDuration = list.GroupByDuration,
            };
            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusProcessingService)}" +
                $" {nameof(GetDowntimeByWellsAsync)}", input?.CorrelationId);

            return output;
        }

        /// <summary>
        /// Gets the downtime data for the run status in the group.
        /// </summary>
        /// <param name="input">The <seealso cref="GroupStatusInput"/> with a correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusKPIOutput"/></returns>
        public async Task<GroupStatusKPIOutput> GetDowntimeByRunStatusAsync(WithCorrelationId<GroupStatusInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusProcessingService)}" +
               $" {nameof(GetDowntimeByRunStatusAsync)}", input?.CorrelationId);

            var output = new GroupStatusKPIOutput();

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get downtime by run status.";

                logger.Write(Level.Info, message);

                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            if (input.Value == null)
            {
                var message = $"{nameof(input)} value is null, cannot get downtime by run status.";

                logger.WriteCId(Level.Info, message, input.CorrelationId);

                output.Result = new MethodResult<string>(false, message);

                return output;
            }

            var assets = GetAssetsInGroup(input.Value.GroupName, input.CorrelationId);

            logger.WriteCId(Level.Debug, $"Number of assets in group ({input.Value.GroupName}): {assets.Count}", input.CorrelationId);

            if (assets.Count == 0)
            {
                output.Result = new MethodResult<string>(true, string.Empty);

                return output;
            }

            var nodeMasterData = await _nodeMasterStore.GetByAssetIdsAsync(assets.Select(x => x.AssetId).ToList(), input.CorrelationId);

            if (nodeMasterData == null)
            {
                output.Result = new MethodResult<string>(true, string.Empty);

                return output;
            }

            var result = nodeMasterData.Where(x => !string.IsNullOrWhiteSpace(x.RunStatus) && x.RunStatus.Contains("Shutdown", StringComparison.InvariantCultureIgnoreCase))
                .GroupBy(x => x.RunStatus)
                .Select(g => new
                {
                    RunStatus = g.Key,
                    Count = g.Count()
                }).ToList();

            var list = result.OrderByDescending(x => x.Count).Select(x => new GroupStatusKPIValues()
            {
                Id = x.RunStatus,
                Name = x.RunStatus,
                Count = x.Count,
                Percent = Common.MathUtility.RoundToSignificantDigits((double)x.Count / nodeMasterData.Count * 100, 2),
            }).ToList();

            output = new GroupStatusKPIOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = list,
            };
            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusProcessingService)}" +
                           $" {nameof(GetDowntimeByRunStatusAsync)}", input?.CorrelationId);

            return output;
        }

        #endregion

        private class GroupStatusAssetModel : AssetModel
        {
            public bool IsLegacyWell { get; set; }
        }

        #region Private Methods

        /// <summary>
        /// Map Data.Models.ConditionalFormats to core.common.ConditionalFormats
        /// </summary>
        /// <param name="conditionalFormatData">A list of conditional format data.</param>
        /// <returns>A list of <seealso cref="ConditionalFormat"/>s.</returns>
        private IList<ConditionalFormat> MapConditionalFormats(IList<ConditionalFormatModel> conditionalFormatData)
        {
            if (conditionalFormatData == null)
            {
                return new List<ConditionalFormat>();
            }

            var conditionalFormats = new List<ConditionalFormat>();

            conditionalFormats.AddRange(conditionalFormatData.Select(item => new ConditionalFormat(item.ColumnId,
                item.OperatorId, item.Value, item.MinValue, item.MaxValue, item.BackColor, item.ForeColor,
                item.StringValue)));

            return conditionalFormats;
        }

        /// <summary>
        /// Map from Data.Models.GroupStatusColumnsModels to Core.Common.GroupStatusColumnsModels.
        /// </summary>
        /// <param name="groupStatusColumnsModels">A list of group status columns data.</param>
        /// <param name="parameterList">A list of ParameterItem.</param>
        /// <param name="parameterDataType">A list of ParameterDataTypeModel.</param>
        /// <param name="facilityTagAddress">A list of FacilityTag.</param>
        /// <param name="conditionalFormats">A list of <see cref="ConditionalFormat"/>.</param>
        /// <param name="correlationId"></param>
        /// <returns>A sorted list containing int to <seealso cref="GroupStatusColumns"/> pairs.</returns>
        private SortedList<int, GroupStatusColumns> MapGroupStatusColumnsModels(IList<GroupStatusColumnsModels>
                groupStatusColumnsModels, SortedList<string, ParameterItem> parameterList,
            IDictionary<int, ParameterDataTypeModel> parameterDataType,
            SortedList<string, FacilityTag> facilityTagAddress, IList<ConditionalFormat> conditionalFormats, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);

            var groupStatusTables = _groupStatus.GetViewTables(correlationId);
            var groupStatusColumns = _groupStatus.LoadCommonColumns(correlationId);

            var lst = new SortedList<int, GroupStatusColumns>();
            var position = 0;

            if (parameterList != null && parameterDataType != null && facilityTagAddress != null)
            {
                foreach (var (item, oColumn) in from item in groupStatusColumnsModels
                                                let oColumn = new GroupStatusColumns(groupStatusTables, groupStatusColumns)
                                                select (item, oColumn))
                {
                    oColumn.Name = item.ColumnName;
                    oColumn.ColumnId = item.ColumnId;
                    oColumn.ColumnAlias = item.Alias ?? oColumn.Name;
                    oColumn.Align = oColumn.ConvertDBAlign(item.Align);
                    oColumn.Formula = item.Formula;
                    var sourceType = (GroupStatusColumns.SourceType)item.SourceId;

                    switch (sourceType)
                    {
                        case GroupStatusColumns.SourceType.Formula:
                            oColumn.FieldType = GroupStatusColumns.DataType.Numeric;

                            break;
                        case GroupStatusColumns.SourceType.ParamStandard:
                            oColumn.ParamStandardType = item.ParamStandardType ?? oColumn.ParamStandardType;

                            break;
                        case GroupStatusColumns.SourceType.Common:
                            if (oColumn.Name == "TIS")
                            {
                                oColumn.FieldType = GroupStatusColumns.DataType.MinutesHoursDays;
                            }

                            break;
                        case GroupStatusColumns.SourceType.Parameter:
                            var paramStateId = 0;
                            var paramDataType = 0;

                            if (parameterList.TryGetValue(oColumn.Name, out var parameterItem))
                            {
                                paramStateId = parameterItem.StateID;
                                paramDataType = parameterItem.DataType;
                                oColumn.UnitMeasureParameter = parameterItem.UnitType;
                            }
                            else
                            {
                                var message = $"Error retrieving parameter - {{oColumn.Name}}";
                                logger.WriteCId(Level.Info, message, correlationId);
                            }

                            if (paramDataType > 0 && paramStateId == 0)
                            {
                                if (paramDataType == (int)UnitCategory.Type.TimeEnron)
                                {
                                    oColumn.FieldType = GroupStatusColumns.DataType.HoursMinutesSeconds;
                                }
                                else
                                {
                                    if (parameterDataType.TryGetValue(paramDataType, out var value) && value.IsNumeric)
                                    {
                                        oColumn.FieldType = GroupStatusColumns.DataType.Numeric;
                                    }
                                    else
                                    {
                                        oColumn.FieldType = GroupStatusColumns.DataType.Alpha;
                                    }
                                }
                            }
                            else
                            {
                                oColumn.FieldType = GroupStatusColumns.DataType.Alpha;
                            }

                            oColumn.UnitMeasureFacilityTag = FacilityUnitType(facilityTagAddress, oColumn.Name);
                            break;
                        case GroupStatusColumns.SourceType.Facility:
                            try
                            {
                                var facilityDataType = GetFacilityDataType(facilityTagAddress, oColumn.Name);

                                if (facilityDataType == UnitCategory.Type.TimeEnron)
                                {
                                    oColumn.FieldType = GroupStatusColumns.DataType.HoursMinutesSeconds;
                                }
                                else if (facilityDataType == UnitCategory.Type.Discrete)
                                {
                                    oColumn.FieldType = GroupStatusColumns.DataType.Alpha;
                                }
                                else if (parameterDataType[(int)facilityDataType].IsNumeric)
                                {
                                    oColumn.FieldType = GroupStatusColumns.DataType.Numeric;
                                }
                                else
                                {
                                    oColumn.FieldType = GroupStatusColumns.DataType.Alpha;
                                }
                            }
                            catch (Exception ex)
                            {
                                oColumn.FieldType = GroupStatusColumns.DataType.Alpha;
                                logger.Write(Level.Error, ex.ToString());
                            }

                            oColumn.UnitMeasureFacilityTag = FacilityUnitType(facilityTagAddress, oColumn.Name);

                            break;
                        default:
                            break;
                    }

                    oColumn.Position = position;

                    oColumn.SourceId = (int)sourceType;
                    oColumn.Visible = item.Visible.Value;
                    oColumn.Width = item.Width.Value;
                    oColumn.UnitMeasure = item.UnitType;
                    oColumn.Measure = item.Measure;
                    oColumn.FormatId = item.FormatId;
                    oColumn.FormatMask = item.FormatMask;
                    oColumn.Orientation = item.Orientation;

                    var columnConditionalFormats = new List<ConditionalFormat>();

                    foreach (var viewModel in conditionalFormats)
                    {
                        if (viewModel.ColumnId == oColumn.ColumnId)
                        {
                            columnConditionalFormats.Add(viewModel);
                        }
                    }

                    oColumn.ConditionalFormats = columnConditionalFormats;

                    if (oColumn.HasAliasTable)
                    {
                        oColumn.ConvertTableAlias();
                    }

                    lst.Add(position, oColumn);
                    position++;
                }
            }

            return lst;
        }

        private UnitCategory.Type GetFacilityDataType(SortedList<string, FacilityTag> facilityTagAddress, string address)
        {
            var dataType = UnitCategory.Type.None;
            var dataTypeFound = false;

            foreach (var item in facilityTagAddress.Values)
            {
                if (item.Description == address)
                {
                    if (dataTypeFound == false)
                    {
                        dataType = (UnitCategory.Type)item.DataType.Value;
                    }

                    dataTypeFound = true;
                }
            }

            return dataType;
        }

        private int FacilityUnitType(SortedList<string, FacilityTag> facilityTagAddress, string address)
        {
            var unitType = 0;

            foreach (var facilityTag in facilityTagAddress.Values)
            {
                if (facilityTag.Description == address && facilityTag.UnitType != null && facilityTag.UnitType != 0)
                {
                    var unitCategory = EnhancedEnumBase.GetValue<UnitCategory>((int)facilityTag.UnitType);
                    if (unitCategory != UnitCategory.None)
                    {
                        unitType = unitCategory.Key;

                        break;
                    }
                }
            }

            return unitType;
        }

        /// <summary>Get the particular View's data from the database.</summary>
        /// <param name="nodeList">A list of nodes that comprise the group.</param>
        /// <param name="viewColumns">A list of GroupStatusColumns.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A DataTable that represents the columns of the current view and the rows for the group's nodes.</returns>
        /// <remarks>If there is an error, the datatable returned will contain one Well column
        /// with the cell contents of LocalePhrase #861 'Error in SQL Text'</remarks>
        private LoadViewRowResult LoadViewRows(string[] nodeList,
            SortedList<int, GroupStatusColumns> viewColumns, string correlationId)
        {
            var bFacility = false;
            var bParameter = false;
            var bParamStandardType = false;
            var hasFacilityTagAlarms = false;

            var loadViewRowResult = new LoadViewRowResult()
            {
                GroupStatusColumns = viewColumns.Select(p => p.Value).ToList(),
                ColumnOverrides = viewColumns.OrderBy(p => p.Key).Select(x => new ColumnOverride()
                {
                    ColumnId = x.Value.ColumnId,
                }).ToList(),
                Rows = new List<RowModel>(),
            };

            if (nodeList.Length == 0 || viewColumns.Count == 0)
            {
                UpdateColumnOverrides(viewColumns, ref loadViewRowResult, correlationId);

                return loadViewRowResult;
            }

            foreach (var oCol in viewColumns.OrderBy(p => p.Key))
            {
                if (oCol.Value.HasAliasTable)
                {
                    oCol.Value.ConvertTableAlias();
                }

                switch (oCol.Value.SourceId)
                {
                    case (int)GroupStatusColumns.SourceType.Common:
                        if (oCol.Value.Name == "FacilityTagAlarms")
                        {
                            hasFacilityTagAlarms = true;
                        }

                        break;
                    case (int)GroupStatusColumns.SourceType.Parameter:
                        bParameter = true;

                        break;
                    case (int)GroupStatusColumns.SourceType.Facility:
                        bFacility = true;

                        break;
                    case (int)GroupStatusColumns.SourceType.ParamStandard:
                        bParamStandardType = true;

                        break;
                    default:
                        break;
                }
            }

            var commonList = BuildSQLCommonResult(nodeList, viewColumns, correlationId);

            IList<ParameterTypeResult> parameterTypeResultList = new List<ParameterTypeResult>();
            IList<CurrRawScanDataTypeResult> currRawScanDataList = new List<CurrRawScanDataTypeResult>();
            IList<FacilityTypeResult> facilityTagList = new List<FacilityTypeResult>();
            IList<ParamStandardTypeSumResult> paramStandardList = new List<ParamStandardTypeSumResult>();
            IList<ParamStandardTypeMaxResult> paramStandardStatesList = new List<ParamStandardTypeMaxResult>();

            if (bParameter)
            {
                parameterTypeResultList = _groupStatus.BuildSQLParameterResult(nodeList, correlationId);

                currRawScanDataList = _groupStatus.BuildSQLCurrRawScanData(nodeList, correlationId);
            }

            if (hasFacilityTagAlarms | bParameter | bFacility)
            {
                facilityTagList = _groupStatus.BuildSQLFacilityResult(nodeList, correlationId);
            }

            if (bParamStandardType)
            {
                paramStandardList = BuildSQLParamStandardTypeResult(nodeList, viewColumns, correlationId);
                paramStandardStatesList = BuildSQLParamStandardTypeStateResult(nodeList, viewColumns, correlationId);
            }

            foreach (var item in commonList)
            {
                loadViewRowResult.Rows.Add(new RowModel()
                {
                    Common = item,
                    Columns = viewColumns.OrderBy(p => p.Key).Select(x => new RowColumnModel()
                    {
                        ColumnId = x.Value.ColumnId,
                    }).ToList(),
                });
            }

            foreach (var row in loadViewRowResult.Rows)
            {
                foreach (var column in row.Columns)
                {
                    var groupStatusColumn = loadViewRowResult.GroupStatusColumns.First(x => x.ColumnId == column.ColumnId);

                    var key = row.Common.FirstOrDefault(x => x.Key.Equals(groupStatusColumn.FieldHeading, StringComparison.OrdinalIgnoreCase)).Key;

                    if (key == null || row.Common.TryGetValue(key, out var valueFromCommon) == false)
                    {
                        continue;
                    }

                    column.Value = valueFromCommon.ToString();
                }
            }

            var phrases = _phraseStore.GetAll(correlationId, new int[] { 208, 209 });

            if (viewColumns != null)
            {
                CombineTables(parameterTypeResultList, facilityTagList, currRawScanDataList, paramStandardList,
                    paramStandardStatesList, nodeList, ref loadViewRowResult, viewColumns, phrases, correlationId);

                AddFormulaColumns(viewColumns);
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var columnFormatterCacheService = scope.ServiceProvider.GetRequiredService<IColumnFormatterCacheService>();

                foreach (var row in loadViewRowResult.Rows)
                {
                    foreach (var column in row.Columns)
                    {
                        var groupStatusColumn = loadViewRowResult.GroupStatusColumns.First(x => x.ColumnId == column.ColumnId);

                        var cache = columnFormatterCacheService.GetData(groupStatusColumn.Name.ToUpper(), nodeList, correlationId);

                        if (groupStatusColumn.Measure != string.Empty || groupStatusColumn.RequiresCalculation())
                        {
                            var columnFormatter =
                                _columnFormatterFactory.Create(groupStatusColumn.SourceId, groupStatusColumn.Name);
                            columnFormatter?.CalculateValue(row.Common, column, correlationId, cache: cache);
                        }

                        if (groupStatusColumn.RequiresFormatting() || groupStatusColumn.HasConditionalFormat)
                        {
                            var columnFormatter = _columnFormatterFactory.Create(groupStatusColumn.SourceId,
                                groupStatusColumn.Name,
                                groupStatusColumn.ConditionalFormats);
                            columnFormatter?.PerformFormat(row.Common, column, groupStatusColumn, correlationId, cache);
                        }
                    }
                }
            }

            try
            {
                CreateFacilityTagAlarmList(facilityTagList, nodeList, phrases, correlationId);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            return loadViewRowResult;
        }

        /// <summary>
        /// Build SQL Param Standard Type State Result.
        /// </summary>
        /// <param name="nodeList">A list of node.</param>
        /// <param name="viewColumns">A list of GroupStatusColumns.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of <seealso cref="ParamStandardTypeMaxResult"/>s.</returns>
        private IList<ParamStandardTypeMaxResult> BuildSQLParamStandardTypeStateResult(string[] nodeList,
            SortedList<int, GroupStatusColumns> viewColumns, string correlationId)
        {
            var list = new List<FieldParamStandardTypeNameValues>();

            foreach (var oCol in viewColumns.OrderBy(p => p.Key))
            {
                if (oCol.Value.SourceId != (int)SQLType.ParamStandard)
                {
                    continue;
                }

                list.Add(new FieldParamStandardTypeNameValues()
                {
                    ParamStandardTypeId = oCol.Value.ParamStandardType,
                    FieldName = oCol.Value.FieldName,
                });
            }

            return _groupStatus.BuildSQLParamStandardTypeStateResult(list, nodeList, correlationId);
        }

        /// <summary>
        /// Build SQL Param Standard Type Result.
        /// </summary>
        /// <param name="nodeList">A list of node.</param>
        /// <param name="viewColumns">A list of GroupStatusColumns.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of <seealso cref="ParamStandardTypeSumResult"/>s.</returns>
        private IList<ParamStandardTypeSumResult> BuildSQLParamStandardTypeResult(string[] nodeList,
            SortedList<int, GroupStatusColumns> viewColumns, string correlationId)
        {
            var list = new List<FieldParamStandardTypeNameValues>();

            foreach (var oCol in viewColumns.OrderBy(p => p.Key))
            {
                if (oCol.Value.SourceId != (int)SQLType.ParamStandard)
                {
                    continue;
                }

                list.Add(new FieldParamStandardTypeNameValues()
                {
                    ParamStandardTypeId = oCol.Value.ParamStandardType,
                    FieldName = oCol.Value.FieldName,
                });
            }

            return _groupStatus.BuildSQLParamStandardTypeResult(list, nodeList, correlationId);
        }

        /// <summary>
        /// Build SQL Common Result.
        /// </summary>
        /// <param name="nodeList">A list of node.</param>
        /// <param name="viewColumns">A list of GroupStatusColumns.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of <seealso cref="CommonTypeResult"/>s.</returns>
        private IList<Dictionary<string, object>> BuildSQLCommonResult(string[] nodeList,
            SortedList<int, GroupStatusColumns> viewColumns, string correlationId)
        {
            var tables = new List<string>();
            var sColumns = string.Empty;
            var hasCameraAlarm = false;
            var hasOperationalScore = false;
            var hasRuntimeAverage = false;

            foreach (var oCol in viewColumns.OrderBy(p => p.Key))
            {
                if (!string.IsNullOrEmpty(oCol.Value.FieldSql)
                    && !oCol.Value.FieldSql.Contains("tblParamStandardTypes")
                    && oCol.Value.FieldSql.Trim().Length != 0)
                {
                    if (sColumns.Trim().Length == 0)
                    {
                        sColumns = oCol.Value.FieldSql;
                    }
                    else if (!sColumns.Contains(oCol.Value.FieldSql, StringComparison.CurrentCulture))
                    {
                        sColumns = sColumns + ", " + oCol.Value.FieldSql;
                    }

                    if (oCol.Value.Name?.ToUpper() == "CAMERAALARMS")
                    {
                        hasCameraAlarm = true;
                    }

                    if (oCol.Value.Name?.ToUpper() == "OPERATIONALSCORE")
                    {
                        hasOperationalScore = true;
                    }

                    if (oCol.Value.Name?.ToUpper() == "%RT 30D")
                    {
                        hasRuntimeAverage = true;
                    }
                }

                if (oCol.Value.SourceId == (int)GroupStatusColumns.SourceType.Parameter)
                {
                    sColumns += ", CONVERT(Float, 0) AS [formula." + oCol.Value.FieldHeading + "], '' as [" +
                        oCol.Value.FieldHeading + ".BackColor], '' as [" + oCol.Value.FieldHeading + ".ForeColor], 0 as [" +
                        oCol.Value.FieldHeading + ".Align], '' AS [" + oCol.Value.FieldHeading + ".Decimals] ";
                }

                if (oCol.Value.SourceId == (int)GroupStatusColumns.SourceType.Facility)
                {
                    sColumns += ", '' as [" + oCol.Value.Name + ".Value], 0 as [" + oCol.Value.Name +
                        ".CurrentValue], '' as [" +
                        oCol.Value.Name + ".Text], '' as [" + oCol.Value.Name + ".BackColor], '' as [" + oCol.Value.Name +
                        ".ForeColor], 0 as [" + oCol.Value.Name + ".Align], CONVERT(Float, 0) AS [formula." +
                        oCol.Value.Name +
                        "], '' AS [" + oCol.Value.Name + ".Decimals] ";
                }

                if (oCol.Value.SourceId == (int)GroupStatusColumns.SourceType.ParamStandard)
                {
                    sColumns += ", '' as [" + oCol.Value.FieldHeading + ".BackColor] " +
                        ", '' as [" + oCol.Value.FieldHeading + ".ForeColor] ";
                }

                if (oCol.Value.ParentTable != null &&
                    oCol.Value.ParentTable != "Frequently Used" &&
                    oCol.Value.ParentTable != "Formula" &&
                    oCol.Value.ParentTable != "tblNodeMaster" &&
                    oCol.Value.ParentTable != "tblParameters" &&
                    oCol.Value.ParentTable != "tblFacilityTags" &&
                    oCol.Value.ParentTable != "tblWellDetails" &&
                    oCol.Value.ParentTable != "tblWellStatistics" &&
                    oCol.Value.ParentTable != "Param Standard Type")
                {
                    var table = oCol.Value.HasAliasTable ? oCol.Value.AliasTable : oCol.Value.ParentTable;

                    if (!tables.Contains(table))
                    {
                        tables.Add(oCol.Value.ParentTable);
                    }
                }
            }

            // Now add the supplemental columns
            foreach (var oCol in viewColumns.OrderBy(p => p.Key))
            {
                if (oCol.Value.SupplementalFields != null)
                {
                    var supplFields = oCol.Value.SupplementalFields.Split(',');

                    foreach (var supplField in supplFields)
                    {
                        if (!sColumns.Contains(supplField, StringComparison.CurrentCulture))
                        {
                            sColumns += ", " + supplField;
                        }
                    }
                }
            }

            return _groupStatus.BuildSQLCommonResult(nodeList, hasCameraAlarm, hasOperationalScore, hasRuntimeAverage, sColumns,
                tables, correlationId);
        }

        private LoadViewRowResult CombineTables(
            IList<ParameterTypeResult> dtParam,
            IList<FacilityTypeResult> dtFacility, IList<CurrRawScanDataTypeResult> dtCurrString,
            IList<ParamStandardTypeSumResult> dtParamStandard, IList<ParamStandardTypeMaxResult> dtParamStandardStates,
            string[] nodeList, ref LoadViewRowResult loadViewRowResult,
            SortedList<int, GroupStatusColumns> viewColumns, IDictionary<int, string> phrases, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupStatus);

            string description;
            SortedList<string, int?> facilityParamStandardTypes = null;

            if (dtParam != null)
            {
                foreach (var paramItem in dtParam)
                {
                    var row = loadViewRowResult.Rows.FirstOrDefault(x =>
                    {
                        if (x.Common.TryGetValue("Well", out var wellValue))
                        {
                            return wellValue.ToString() == paramItem.NodeId;
                        }

                        return false;
                    });

                    if (row == null)
                    {
                        continue;
                    }

                    var oCol = viewColumns.Values.FirstOrDefault(x =>
                        x.SourceId == (int)GroupStatusColumns.SourceType.Parameter &&
                        x.FieldHeading == $"tblParameters.{paramItem.Description}");

                    if (oCol == null)
                    {
                        continue;
                    }

                    var dbValue = string.Empty;

                    if (!string.IsNullOrWhiteSpace(paramItem.Text))
                    {
                        dbValue = paramItem.Text;
                        if (paramItem.BackColor.HasValue && paramItem.BackColor.ToString() != "")
                        {
                            try
                            {
                                row.Columns.First(x => x.ColumnId == oCol.ColumnId).BackColor =
                                    paramItem.BackColor?.ToString();
                            }
                            catch (Exception ex)
                            {
                                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                            }
                        }

                        if (paramItem.ForeColor.HasValue && paramItem.ForeColor.ToString() != "")
                        {
                            try
                            {
                                row.Columns.First(x => x.ColumnId == oCol.ColumnId).ForeColor =
                                    paramItem.ForeColor?.ToString();
                            }
                            catch (Exception ex)
                            {
                                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                            }
                        }
                    }
                    else
                    {
                        if (oCol.FieldType == GroupStatusColumns.DataType.Alpha)
                        {
                            description = paramItem.Description;

                            var dataRowCurrString = dtCurrString.FirstOrDefault(x =>
                                x.NodeId == paramItem.NodeId && x.Description == description);

                            dbValue = dataRowCurrString == null ? string.Empty : dataRowCurrString.StringValue;
                        }
                        else if (oCol.FieldType == GroupStatusColumns.DataType.HoursMinutesSeconds)
                        {
                            var value = float.Parse(paramItem.V1.ToString());
                            dbValue = CreateTimeStamp(Convert.ToInt32(value));
                        }
                        else if (paramItem.V1 != null)
                        {
                            dbValue = paramItem.V1.ToString() ?? string.Empty;
                        }
                    }

                    row.Columns.First(x => x.ColumnId == oCol.ColumnId).Value = dbValue;

                    row.Columns.First(x => x.ColumnId == oCol.ColumnId).Decimals = paramItem.Decimal;
                }
            }

            if (dtFacility != null)
            {
                if (dtFacility.Count > 0)
                {
                    facilityParamStandardTypes = _groupStatus.GetFacilityParamStandardTypes(nodeList, correlationId);
                }
            }

            _ = int.TryParse(_groupStatus.GroupStatusFacilityTag("GroupStatusFacilityTagSelectionMethod", correlationId),
                out var matchingMethod);

            if (dtFacility != null)
            {
                foreach (var item in dtFacility)
                {
                    foreach (var oCol in viewColumns.Values)
                    {
                        if (oCol.SourceId == (int)GroupStatusColumns.SourceType.Facility && IsMatchingFacilityTag(
                                matchingMethod,
                                oCol.FieldHeading, item, facilityParamStandardTypes))
                        {
                            var row = loadViewRowResult.Rows.FirstOrDefault(x =>
                            {
                                if (x.Common.TryGetValue("Well", out var wellValue))
                                {
                                    return wellValue.ToString() == item.GroupNodeId;
                                }

                                return false;
                            });

                            if (row == null)
                            {
                                continue;
                            }

                            try
                            {
                                var dbValue = "";
                                var alarmState = Convert.ToInt32(item.AlarmState);

                                int? dataType = item.DataType != null ? Convert.ToInt32(item.DataType) : null;

                                if (item.Decimals != null)
                                {
                                    row.Columns.First(x => x.ColumnId == oCol.ColumnId).Decimals = item.Decimals;
                                }

                                if (item.StateId == null)
                                {
                                    switch (alarmState)
                                    {
                                        case 1:
                                        case 2:
                                            row.Columns.First(x => x.ColumnId == oCol.ColumnId).BackColor = "Red";
                                            row.Columns.First(x => x.ColumnId == oCol.ColumnId).ForeColor = "White";

                                            break;
                                        default:
                                            // Don't set BackColor to white, keep default back color
                                            row.Columns.First(x => x.ColumnId == oCol.ColumnId).ForeColor = "Black";

                                            break;
                                    }

                                    if (item.CurrentValue != null)
                                    {
                                        if (dataType == 9 && item.CurrentValue.All(char.IsDigit))
                                        {
                                            dbValue = DateTime.FromOADate(Convert.ToDouble(item.CurrentValue)).ToString();
                                        }
                                        else
                                        {
                                            dbValue = item.CurrentValue;
                                            row.Columns.First(x => x.ColumnId == oCol.ColumnId).ForeColor = "Black";
                                            row.Columns.First(x => x.ColumnId == oCol.ColumnId).Align =
                                                TextHAlign.Right;
                                        }
                                    }
                                }
                                else // use status-only formatting rules
                                {
                                    // Don't set BackColor to white, keep default back color
                                    row.Columns.First(x => x.ColumnId == oCol.ColumnId).ForeColor = "Black";
                                    var showStateId = _groupStatus.GroupStatusFacilityTag("FacilityStatusShowValueWithText", correlationId);

                                    var includeStateId = string.Empty;

                                    if (showStateId == "1")
                                    {
                                        includeStateId = $" ({item.CurrentValue})";
                                    }

                                    dbValue = item.Text + includeStateId;

                                    if (item.BackColor != null)
                                    {
                                        row.Columns.First(x => x.ColumnId == oCol.ColumnId).BackColor =
                                            item.BackColor.ToString();
                                    }

                                    if (item.ForeColor != null)
                                    {
                                        row.Columns.First(x => x.ColumnId == oCol.ColumnId).ForeColor =
                                            item.ForeColor.ToString();
                                    }

                                    row.Columns.First(x => x.ColumnId == oCol.ColumnId).Align =
                                        TextHAlign.Left;
                                }

                                var column = row.Columns.First(x => x.ColumnId == oCol.ColumnId);

                                column.Value = dbValue;

                                if (int.TryParse(dbValue, out var dbValueInt))
                                {
                                    column.Value = dbValueInt.ToString();
                                }
                                else if (float.TryParse(dbValue, out var dbValueFloat))
                                {
                                    column.Value = dbValueFloat.ToString();
                                }
                                else
                                {
                                    column.Value = "0";
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                            }
                        }
                        else if (oCol.FieldHeading == "tblParameters." + item.Description)
                        {
                            // If Facility Tags are set to Trend data to another well, get the value using GroupNodeID and Description.

                            var row = loadViewRowResult.Rows.First(x => x.Common["Well"].ToString() == item.GroupNodeId);
                            var currentValue = "";

                            try
                            {
                                if (item.CurrentValue != null)
                                {
                                    currentValue = item.CurrentValue;
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                            }

                            if (currentValue != string.Empty)
                            {
                                row.Columns.First(x => x.ColumnId == oCol.ColumnId).Value = currentValue;
                            }
                        }
                        else if (oCol.FieldHeading == "FacilityTagAlarms")
                        {
                            var row = loadViewRowResult.Rows.First(x => x.Common["Well"].ToString() == item.GroupNodeId);

                            try
                            {
                                if (Convert.ToInt32(item.AlarmState) > 0 &&
                                    item.FacilityTagAlarm != null && item.FacilityTagAlarm == item.Description)
                                {
                                    var state = Convert.ToInt32(item.AlarmState);
                                    var hiOrLo = "";
                                    try
                                    {
                                        switch (state)
                                        {
                                            case 1:
                                                hiOrLo = phrases[208];
                                                break;
                                            case 2:
                                                hiOrLo = phrases[209];
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                                    }

                                    row.Columns.First(x => x.ColumnId == oCol.ColumnId).Value = string.Format("{0}-{1}", item.FacilityTagAlarm, hiOrLo);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                            }
                        }
                    }
                }
            }

            var paramStandardTypeDistinctNodeIds = dtParamStandard?.Select(x => x.NodeId)?.Distinct()?.ToList();

            foreach (var distinctNodeId in paramStandardTypeDistinctNodeIds)
            {
                var row = loadViewRowResult.Rows.First(x => x.Common["Well"].ToString() == distinctNodeId);

                foreach (var oCol in viewColumns.Values?.Where(x => x.SourceId == (int)SQLType.ParamStandard) ??
                         Enumerable.Empty<GroupStatusColumns>())
                {
                    var column = row.Columns.First(x => x.ColumnId == oCol.ColumnId);

                    column.ValueType = typeof(float);

                    foreach (var item in dtParamStandard?.Where(x =>
                                 x.NodeId == distinctNodeId && oCol.ParamStandardType == x.ParamStandardTypeId) ??
                             Enumerable.Empty<ParamStandardTypeSumResult>())
                    {
                        column.Value = item.SumValue.ToString();
                    }
                }
            }

            foreach (var item in dtParamStandardStates ?? Enumerable.Empty<ParamStandardTypeMaxResult>())
            {
                var row = loadViewRowResult.Rows.First(x => x.Common["Well"].ToString() == item.NodeId);

                foreach (var oCol in (IEnumerable<GroupStatusColumns>)viewColumns.Values)
                {
                    if (oCol.SourceId == (int)SQLType.ParamStandard && oCol.ParamStandardType == item.ParamStandardTypeId)
                    {
                        row.Columns.First(x => x.ColumnId == oCol.ColumnId).Value = item.MaxValue;
                    }
                }
            }

            UpdateColumnOverrides(viewColumns, ref loadViewRowResult, correlationId);

            return loadViewRowResult;
        }

        private void UpdateColumnOverrides(SortedList<int, GroupStatusColumns> viewColumns, ref LoadViewRowResult apiResult, string correlationId)
        {
            foreach (var oCol in viewColumns.Values)
            {
                try
                {
                    apiResult.ColumnOverrides.First(x => x.ColumnId == oCol.ColumnId).Caption =
                        oCol.ColumnAlias;

                    if (oCol.SourceId == (int)GroupStatusColumns.SourceType.Common)
                    {
                        apiResult.ColumnOverrides.First(x => x.ColumnId == oCol.ColumnId).ColumnName =
                            oCol.Name;
                    }
                }
                catch (Exception ex)
                {
                    var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
                    logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                }
            }
        }

        private void AddFormulaColumns(SortedList<int, GroupStatusColumns> viewColumns)
        {
            IList<string> formulaColumns = new List<string>();

            foreach (var oCol in viewColumns.Values)
            {
                if (oCol.SourceId == (int)GroupStatusColumns.SourceType.Formula)
                {
                    var formula = oCol.Formula;
                    var columnName = Between(formula, "[", "]");

                    while (columnName != string.Empty)
                    {
                        if (!formulaColumns.Contains(columnName))
                        {
                            formulaColumns.Add(columnName);
                        }

                        formula = formula.Replace("[" + columnName + "]", "");
                        columnName = Between(formula, "[", "]");
                    }
                }
            }
        }

        /// <summary>
        /// Creates the facility tag alarm list.
        /// </summary>
        /// <param name="dtFacility">The list of <see cref="FacilityTypeResult"/>.</param>
        /// <param name="nodeList">The list of node ids.</param>
        /// <param name="phrases"></param>
        /// <param name="correlationId"></param>
        /// <remarks>This code is related to displaying the facility tag alarm list on the UI.
        /// We will need this in the future.</remarks>
        private void CreateFacilityTagAlarmList(IList<FacilityTypeResult> dtFacility, string[] nodeList, IDictionary<int, string> phrases, string correlationId)
        {
            IDictionary<string, IList<string>> facilityTagAlarmList = new Dictionary<string, IList<string>>();

            if (nodeList == null)
            {
                return;
            }

            facilityTagAlarmList.Clear();

            foreach (var nodeId in nodeList)
            {
                var alarms = new List<string>();

                if (dtFacility != null)
                {
                    foreach (var row in dtFacility)
                    {
                        if (nodeId == row.GroupNodeId &&
                            Convert.ToInt32(row.AlarmState) > 0)
                        {
                            var state = Convert.ToInt32(row.AlarmState);
                            var hiOrLo = "";
                            try
                            {
                                switch (state)
                                {
                                    case 1:
                                        hiOrLo = phrases[208];
                                        break;
                                    case 2:
                                        hiOrLo = phrases[209];
                                        break;
                                    default:
                                        break;
                                }

                                alarms.Add(
                                    string.Format("{0}-{1}", RuntimeHelpers.GetObjectValue(row.Description), hiOrLo));
                                alarms.Sort();
                            }
                            catch (Exception ex)
                            {
                                var logger = _loggerFactory.Create(LoggingModel.GroupStatus);
                                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                            }
                        }
                    }
                }

                facilityTagAlarmList.Add(nodeId, alarms);
            }
        }

        private string CreateTimeStamp(int numberOfSeconds)
        {
            string hhmmssTime;

            var hours = numberOfSeconds / 3600;
            var minutes = (numberOfSeconds % 3600) / 60;
            var seconds = numberOfSeconds % 60;

            hhmmssTime = $"{hours:00}:{minutes:00}:{seconds:00}";

            return hhmmssTime;
        }

        private bool IsMatchingFacilityTag(int method, string fieldHeading, FacilityTypeResult row,
            SortedList<string, int?> facilityParamStandardTypes)
        {
            var isMatch = false;

            if (method > 2)
            {
                method = 0;
            }

            // method 0: Require standard measurements be used in facility tags
            // method 1: Match facility tags based on description
            // method 2: Hybrid - match standard measurement first, then allow match on Description if necessary
            if ((method == 0 || method == 2) && row.ParamStandardType != null)
            {
                facilityParamStandardTypes.TryGetValue(fieldHeading, out var paramStandardType);
                isMatch = paramStandardType == Convert.ToInt32(row.ParamStandardType);
            }

            if (!isMatch && (method == 1 || method == 2))
            {
                isMatch = fieldHeading == $"tblFacilityTags.{row.Description}";
            }

            return isMatch;
        }

        private string Between(string value, string begDelimiter, string endDelimiter)
        {
            var indexLeftBracket = value.IndexOf(begDelimiter);
            var indexRightBracket = value.IndexOf(endDelimiter);

            if (indexLeftBracket == -1 && indexRightBracket == -1)
            {
                return string.Empty;
            }

            return value.Substring(indexLeftBracket + 1, indexRightBracket - indexLeftBracket - 1);
        }

        private string[] GetAssetIdsInGroup(string groupName, string correlationId)
        {
            var groups = _groupAndAssetService.GetGroupAssetAndRelationshipData(correlationId, groupName);

            if (groups?.Assets == null)
            {
                return Array.Empty<string>();
            }

            return groups.Assets.Select(x => (x).AssetId.ToString()).ToArray();
        }

        private IList<AssetModel> GetAssetsInGroup(string groupName, string correlationId)
        {
            var groups = _groupAndAssetService.GetGroupAssetAndRelationshipData(correlationId, groupName);

            if (groups?.Assets == null)
            {
                return new List<AssetModel>();
            }

            return groups.Assets.ToList();
        }

        private void UpdateDowntimeByWellsKPIValues(int numberOfDays, IList<AssetModel> assets,
            IList<DowntimeByWellsRodPumpModel> resultData, ref GroupStatusDowntimeByWell values)
        {
            var downtimeRodPumpDictionary = new Dictionary<string, double>();

            foreach (var item in resultData)
            {
                // calculate downtime
                var downtime = 24d - item.Runtime - (item.Cycles * item.IdleTime / 60d);

                if (downtime <= 0)
                {
                    downtime = 0;
                }

                if (downtimeRodPumpDictionary.ContainsKey(item.Id))
                {
                    downtimeRodPumpDictionary[item.Id] += downtime;
                }
                else
                {
                    downtimeRodPumpDictionary[item.Id] = downtime;
                }
            }

            foreach (var item in downtimeRodPumpDictionary)
            {
                if (item.Value > 0)
                {
                    // calculate percent
                    var percent = item.Value / (24 * numberOfDays) * 100;

                    values.Assets.Add(new GroupStatusKPIValues()
                    {
                        Id = assets.FirstOrDefault(x => x.AssetName == item.Key)?.AssetId.ToString() ?? string.Empty,
                        Name = item.Key,
                        Count = Common.MathUtility.RoundToSignificantDigits(item.Value, 1),
                        Percent = Common.MathUtility.RoundToSignificantDigits(percent, 2)
                    });
                }
            }
        }

        private void UpdateDowntimeByWellsKPIValues(int numberOfDays, IList<AssetModel> assets,
            IList<DowntimeByWellsValueModel> resultData, ref GroupStatusDowntimeByWell values)
        {
            foreach (var nodeId in resultData.Select(x => x.Id).Distinct().ToList())
            {
                double points = resultData.Count(x => x.Id == nodeId);
                double downPoints = resultData.Count(x => x.Id == nodeId && x.Value == 0);

                var percent = (downPoints / points) * 100;
                var downtime = (downPoints / points) * 24 * numberOfDays;
                if (downtime > 0)
                {
                    values.Assets.Add(new GroupStatusKPIValues()
                    {
                        Id = assets.FirstOrDefault(x => x.AssetName == nodeId)?.AssetId.ToString() ?? string.Empty,
                        Name = nodeId,
                        Count = Common.MathUtility.RoundToSignificantDigits(downtime, 1),
                        Percent = Common.MathUtility.RoundToSignificantDigits(percent, 2)
                    });
                }
            }
        }

        private void AddDowntimeByWellsGrouping(ref GroupStatusDowntimeByWell values, int digits)
        {
            if (values.Assets.Count == 0)
            {
                return;
            }

            var lessThan6Hours = values.Assets.Count(x => x.Count < 6);
            var lessThan12Hours = values.Assets.Count(x => x.Count > 6 && x.Count <= 12);
            var lessThan24Hours = values.Assets.Count(x => x.Count > 12 && x.Count <= 24);
            var lessThan48Hours = values.Assets.Count(x => x.Count > 24 && x.Count <= 48);
            var lessThan72Hours = values.Assets.Count(x => x.Count > 48 && x.Count <= 72);
            var lessThan96Hours = values.Assets.Count(x => x.Count > 72 && x.Count <= 96);
            var greaterThan96Hours = values.Assets.Count(x => x.Count > 96);

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Less6",
                Name = "< 6 hours",
                Count = lessThan6Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)lessThan6Hours / values.Assets.Count) * 100, digits)
            });

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Less12",
                Name = "6-12 hours",
                Count = lessThan12Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)lessThan12Hours / values.Assets.Count) * 100, digits)
            });

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Less24",
                Name = "12-24 hours",
                Count = lessThan24Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)lessThan24Hours / values.Assets.Count) * 100, digits)
            });

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Less48",
                Name = "24-48 hours",
                Count = lessThan48Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)lessThan48Hours / values.Assets.Count) * 100, digits)
            });

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Less72",
                Name = "48-72 hours",
                Count = lessThan72Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)lessThan72Hours / values.Assets.Count) * 100, digits)
            });

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Less96",
                Name = "72-96 hours",
                Count = lessThan96Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)lessThan96Hours / values.Assets.Count) * 100, digits)
            });

            values.GroupByDuration.Add(new GroupStatusKPIValues()
            {
                Id = "Greater96",
                Name = "> 96 hours",
                Count = greaterThan96Hours,
                Percent = Common.MathUtility.RoundToSignificantDigits(((double)greaterThan96Hours / values.Assets.Count) * 100, digits)
            });
        }

        private DowntimeFiltersWithChannelIdInflux GetChannelId(DowntimeFiltersInflux data, string correlationId)
        {
            var result = new DowntimeFiltersWithChannelIdInflux
            {
                AssetIds = data.AssetIds,
                CustomerIds = data.CustomerIds,
                POCType = data.POCType,
                ChannelIds = new List<string>()
            };

            if (data != null)
            {
                var parameters = _parameterStore.GetParameterByParamStdType(data.POCType,
                        data.ParamStandardType.Distinct().ToList(), correlationId);

                result.ChannelIds = parameters.Select(a => a.ChannelId).Distinct().ToList();
            }

            return result;
        }

        #endregion

    }
}
