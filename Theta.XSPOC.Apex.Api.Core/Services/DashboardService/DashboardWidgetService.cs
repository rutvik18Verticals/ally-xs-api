using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models.Dashboard;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services.DashboardService
{
    /// <summary>
    /// Service for managing dashboard widgets.
    /// </summary>
    public class DashboardWidgetService : IDashboardWidgetService
    {

        #region Private Members

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IDashboardStore _dashboardStore;
        private readonly IAllyTimeSeriesNodeMaster _allyNodeMasterStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <seealso cref="DashboardWidgetService"/>.
        /// </summary>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="dashboardStore">
        /// The <seealso cref="IDashboardStore"/> service.</param>
        /// <param name="allyNodeMasterStore"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DashboardWidgetService(IThetaLoggerFactory loggerFactory, IDashboardStore dashboardStore, IAllyTimeSeriesNodeMaster allyNodeMasterStore)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dashboardStore = dashboardStore ?? throw new ArgumentNullException(nameof(dashboardStore));
            _allyNodeMasterStore = allyNodeMasterStore ?? throw new ArgumentNullException(nameof(dashboardStore));
        }

        #endregion

        #region IDashboardWidgetService Implementation

        /// <summary>
        /// Gets the list of all chart widgets for ESP Well.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="dashboardType">The dashboard type.</param>
        /// <param name="correlationId">The correlation id .</param>
        /// <param name="isStacked">True to get Stacked Widgets</param>
        /// <returns></returns>
        public DashboardWidgetDataOutput GetESPWellChartWidgets(string dashboardType, string userId,
            string correlationId, bool isStacked)
        {
            var logger = _loggerFactory.Create(LoggingModel.DashboardWidget);

            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardWidgetService)}" +
                $" {nameof(GetESPWellChartWidgets)}", correlationId);

            var result = _dashboardStore.GetDashboardWidgets(dashboardType, userId, correlationId, isStacked);

            if (result == null)
            {
                return new DashboardWidgetDataOutput
                {
                    ErrorCode = "204",
                    Result = new MethodResult<string>(false, "No dashboard widgets found.")
                };
            }

            return new DashboardWidgetDataOutput()
            {
                Widgets = Map(result.Result),
                Result = new MethodResult<string>(true, "Request successfully completed!")
            };

        }

        /// <summary>
        /// Updates the user preferences for the dashboard widget
        /// </summary>
        /// <param name="input">The dashboard widget preferences to be updated.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public async Task<bool> SaveWidgetUserPreferences(DashboardWidgetUserPreferencesInput input, string userId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.DashboardWidget);

            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardWidgetService)}" +
                $" {nameof(SaveWidgetUserPreferences)}", correlationId);

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "User Preferences cannot be null.");
            }

            var data = new DashboardWidgetPreferenceDataModel()
            {
                DashboardName = input.DashboardName,
                WidgetName = input.WidgetName,
                Preferences = this.MapWidgetProperties(input, userId, correlationId)
            };
            var result = await _dashboardStore.SaveDashboardWidgetUserPrefeernces(data, userId, correlationId);

            logger.WriteCId(Level.Trace, $"Ending {nameof(DashboardWidgetService)}" +
               $" {nameof(SaveWidgetUserPreferences)}", correlationId);

            return result;
        }

        /// <summary>
        /// Resets the user preferences for the dashboard widget
        /// </summary>
        /// <param name="input">The dashboard widget preferences to be reset.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public async Task<DashboardWidgetDataOutput> ResetWidgetUserPreferences(DashboardWidgetResetUserPreferencesInput input, string userId,
            string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.DashboardWidget);

            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardWidgetService)}" +
                $" {nameof(ResetWidgetUserPreferences)}", correlationId);

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(input.DashboardName) || string.IsNullOrWhiteSpace(input.WidgetName))
            {
                throw new ArgumentNullException(nameof(input), "Invalid dashboard or widget name, name cannot be empty.");
            }

            var data = new DashboardWidgetPreferenceDataModel()
            {
                DashboardName = input.DashboardName,
                WidgetName = input.WidgetName
            };
            var result = await _dashboardStore.ResetDashboardWidgetUserPrefeernces(data, userId, correlationId);

            if (result == null)
            {
                return new DashboardWidgetDataOutput
                {
                    ErrorCode = "204",
                    Result = new MethodResult<string>(false, "No dashboard widgets found.")
                };
            }

            logger.WriteCId(Level.Trace, $"Ending {nameof(DashboardWidgetService)}" +
               $" {nameof(ResetWidgetUserPreferences)}", correlationId);

            return new DashboardWidgetDataOutput()
            {
                Widgets = Map(result),
                Result = new MethodResult<string>(true, "Request successfully completed!")
            };
        }

        /// <summary>
        /// Gets the list of all default parameters for the given lift type.
        /// </summary>
        /// <param name="liftType">list type to be searched for</param>
        /// <param name="correlationId">correlation id</param>
        /// <returns></returns>
        public async Task<DefaultParameterDataOutput> GetAllDefaultParameters(string liftType, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.DashboardWidget);

            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardWidgetService)}" +
                $" {nameof(GetAllDefaultParameters)}", correlationId);

            var defaultParamsList = await _allyNodeMasterStore.GetAllDefaultParametersAsync(liftType, correlationId);

            if (defaultParamsList == null)
            {
                logger.WriteCId(Level.Trace, $"Ending {nameof(DashboardWidgetService)}" +
                    $" {nameof(GetAllDefaultParameters)}", correlationId);

                return new DefaultParameterDataOutput
                {
                    ErrorCode = "204",
                    Result = new MethodResult<string>(false, "No default parameters found.")
                };
            }

            logger.WriteCId(Level.Trace, $"Ending {nameof(DashboardWidgetService)}" +
                    $" {nameof(GetAllDefaultParameters)}", correlationId);

            return new DefaultParameterDataOutput()
            {
                DefaultParameters = MapDefaultParameters(defaultParamsList),
                Result = new MethodResult<string>(true, "Request successfully completed!")
            };

        }

        #endregion

        #region Private Methods

        private List<DashboardWidget> Map(DashboardWidgetDataModel result)
        {
            var lstWidgets = new List<DashboardWidget>();

            foreach (var widget in result.Widgets)
            {
                var dashboardWidget = new DashboardWidget
                {
                    Key = widget.Key,
                    Show = widget.Show,
                    Layout = widget.Layout,
                    WidgetProperty = widget.WidgetProperty
                };

                lstWidgets.Add(dashboardWidget);
            }
            return lstWidgets;
        }

        private WidgetType MapWidgetProperties(DashboardWidgetUserPreferencesInput input, string userId, string correlationId)
        {
            var widgetData = _dashboardStore.GetDashboardWidgetData(input.DashboardName, input.WidgetName, userId, correlationId).Result;

            if (widgetData == null)
            {
                throw new ValidationException("Invalid widget name or dashboard name.");
            }
            //if widget is time series chart
            if (input.WidgetName.ToLower() == "timeserieschart")
            {
                var widgetProperty = widgetData.WidgetProperties as TimeSeriesChart;

                if (input.PropertyType.ToLower() == nameof(widgetProperty.ChartType).ToLower())
                {
                    widgetProperty.ChartType = input.Preferences.ToString();
                }
                else if (input.PropertyType.ToLower() == nameof(widgetProperty.DateSelector).ToLower())
                {
                    widgetProperty.DateSelector = (input.Preferences as JObject).ToObject<DateSelector>();
                }
                else if (input.PropertyType.ToLower() == nameof(widgetProperty.ViewOptions).ToLower())
                {
                    widgetProperty.ViewOptions = (input.Preferences as JObject).ToObject<ViewOptions>();
                }
                else if (input.PropertyType.ToLower() == nameof(widgetProperty.ParameterConfig).ToLower())
                {
                    var parameterConfig = (input.Preferences as JObject).ToObject<ParameterConfig>();
                    var existingConfig = widgetProperty.ParameterConfig.FirstOrDefault(x => x.Name.ToLower() == parameterConfig.Name.ToLower());
                    if (existingConfig != null)
                    {
                        existingConfig.Id = parameterConfig.Id;
                        existingConfig.Selected = parameterConfig.Selected;
                        existingConfig.Default = parameterConfig.Default;
                        existingConfig.Parameters = parameterConfig.Parameters;
                    }
                    else
                    {
                        widgetProperty.ParameterConfig.Add(parameterConfig);
                    }
                }

                return widgetProperty;
            }

            //if widget is well test table
            else if (input.WidgetName.ToLower() == "welltesttable")
            {
                var widgetProperty = widgetData.WidgetProperties as WellTest;
                if (input.PropertyType.ToLower() == nameof(widgetProperty.FixedColumns).ToLower())
                {
                    widgetProperty.FixedColumns = (input.Preferences as JArray).ToObject<List<FixedColumns>>();
                }
                else if (input.PropertyType.ToLower() == nameof(widgetProperty.Columns).ToLower())
                {
                    widgetProperty.Columns = (input.Preferences as JArray).ToObject<List<Columns>>();
                }

                return widgetProperty;
            }
            //if widget is well tasks 
            else if (input.WidgetName.ToLower() == "taskdetails")
            {
                var widgetProperty = widgetData.WidgetProperties as TaskDetail;
                if (input.PropertyType.ToLower() == nameof(widgetProperty.FixedColumns).ToLower())
                {
                    widgetProperty.FixedColumns = (input.Preferences as JArray).ToObject<List<FixedColumns>>();
                }
                else if (input.PropertyType.ToLower() == nameof(widgetProperty.Columns).ToLower())
                {
                    widgetProperty.Columns = (input.Preferences as JArray).ToObject<List<Columns>>();
                }

                return widgetProperty;
            }

            throw new ValidationException("Invalid widget name or widget type preference is not defined.");
        }

        private List<DefaultParameterDataModel> MapDefaultParameters(IEnumerable<DefaultParameters> result)
        {
            var paramList = new List<DefaultParameterDataModel>();

            foreach (var param in result)
            {
                var paramData = new DefaultParameterDataModel
                {
                    Name = param.Name,
                    DisplayOrder = param.DisplayOrder,
                    HighParamType = param.HighParamType,
                    LowParamType = param.LowParamType,
                    LiftType = param.LiftType,
                    Pst = param.Pst,
                    Units = param.Units,
                    Selected = param.Selected
                };

                paramList.Add(paramData);
            }
            return paramList;
        }

        #endregion

    }
}
