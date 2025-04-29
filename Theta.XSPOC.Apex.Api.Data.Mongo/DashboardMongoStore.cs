using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.Dashboard;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{

    /// <summary>
    /// Represents a Mongo store implemetation for Dashboard and Dashboard Widgets entities.
    /// </summary>
    public class DashboardMongoStore : MongoOperations, IDashboardStore
    {

        #region Private Constants

        private const string DASHBOARD_COLLECTION = "Dashboards";
        private const string DASHBOARDWIDGETS_COLLECTION = "DashboardWidgets";
        private const string DASHBOARDWIDGETUSERSETTINGS_COLLECTION = "DashboardWidgetUserSettings";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DashboardMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public DashboardMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IDashboardStore implementations

        /// <summary>
        /// Gets the dashboard widgets for the <paramref name="dashboardType"/> and user <paramref name="userId"/>.
        /// </summary>
        /// <param name="dashboardType">The dashhboard type.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <param name="isStacked">isStacked flag.</param>
        public Task<DashboardWidgetDataModel> GetDashboardWidgets(string dashboardType, string userId,
            string correlationId, bool isStacked)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardMongoStore)}" +
                $" {nameof(GetDashboardWidgets)}", correlationId);

            var filter = Builders<Dashboards>.Filter.And(
                    Builders<Dashboards>.Filter.Eq(x => x.Dashboard, dashboardType)
                );

            var dashboardData = Find<Dashboards>(DASHBOARD_COLLECTION, filter, correlationId);

            if (dashboardData == null || !dashboardData.Any())
            {
                logger.WriteCId(Level.Info, "Missing dashboard data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} {nameof(GetDashboardWidgets)}", correlationId);

                return null;
            }

            if (dashboardData.Count > 0)
            {
                var dashboardUserData = dashboardData.Where(x => x.UserId == userId)?.FirstOrDefault();
                if (dashboardUserData == null)
                {
                    logger.WriteCId(Level.Info, "Missing dashboard user data", correlationId);
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} " +
                    $"{nameof(GetDashboardWidgets)}", correlationId);

                var data = dashboardUserData ?? dashboardData.FirstOrDefault();

                var lstWidgets = isStacked ? data.Widgets.Stacked.Select(x => x.Key).ToArray()
                    : data.Widgets.Default.Select(x => x.Key).ToArray();

                var filterWidgetProperties = Builders<DashboardWidgets>.Filter.And(
                        Builders<DashboardWidgets>.Filter.Eq(x => x.Dashboard, dashboardType),
                        Builders<DashboardWidgets>.Filter.In(x => x.Key, lstWidgets),
                        Builders<DashboardWidgets>.Filter.Eq(x => x.IsDeleted, false)
                    );

                var widgetProperty = Find<DashboardWidgets>(DASHBOARDWIDGETS_COLLECTION, filterWidgetProperties, correlationId);

                var filterUserSettings = Builders<DashboardWidgetUserSettings>.Filter.And(
                        Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.DashboardName, dashboardType),
                        Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.UserId, userId),
                        Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.IsDeleted, false)
                    );

                var userSettings = Find<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION,
                    filterUserSettings, correlationId);

                // Map the dashboard data to DashboardWidgetDataModel
                return Map(data, widgetProperty, userSettings, isStacked);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<bool> SaveDashboardWidgetUserPrefeernces(DashboardWidgetPreferenceDataModel input, string userId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardMongoStore)}" +
                $" {nameof(SaveDashboardWidgetUserPrefeernces)}", correlationId);

            if (input == null)
                throw new ArgumentNullException(nameof(input), "User Preferences cannot be null.");

            var filterUserSettings = Builders<DashboardWidgetUserSettings>.Filter.And(
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.DashboardName, input.DashboardName),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.WidgetName, input.WidgetName),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.UserId, userId),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.PropertyName, "WidgetProperties"),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.IsDeleted, false)
                   );

            var existing = Find<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION, filterUserSettings, correlationId).FirstOrDefault();

            //if exists update the preferences
            if (existing != null)
            {
                logger.WriteCId(Level.Info, "Updating dashboard widget user preferences data.", correlationId);

                UpdateDefinition<DashboardWidgetUserSettings> updateDefinition = Builders<DashboardWidgetUserSettings>.Update
                        .Set(x => x.LastModifiedBy, userId)
                        .Set(x => x.LastModifiedDate, DateTime.UtcNow)
                        .Set(x => x.PropertyValue, input.Preferences);

                await _database.GetCollection<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION).UpdateOneAsync(x => x.Id == existing.Id, updateDefinition);

                logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} {nameof(SaveDashboardWidgetUserPrefeernces)}", correlationId);

                return true;
            }
            //else add the preferences for the user
            else
            {
                logger.WriteCId(Level.Info, "Adding dashboard widget user preferences data.", correlationId);

                var newUserPreferences = new DashboardWidgetUserSettings()
                {
                    DashboardName = input.DashboardName,
                    WidgetName = input.WidgetName,
                    CreatedBy = userId,
                    CreatedOnDate = DateTime.UtcNow,
                    IsDeleted = false,
                    LiftType = "ESP",
                    PropertyName = "WidgetProperties",
                    PropertyValue = input.Preferences,
                    UserId = userId
                };

                await _database.GetCollection<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION).InsertOneAsync(newUserPreferences);

                logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} {nameof(SaveDashboardWidgetUserPrefeernces)}", correlationId);

                return true;
            }
        }

        /// <summary>
        /// Gets the dashboard widget properties for the <paramref name="dashboardName"/> and <paramref name="widgetName"/>.
        /// </summary>
        /// <param name="dashboardName">dashboard name. </param>
        /// <param name="widgetName">widget name.</param>
        /// <param name="userId">lgged in user id.</param>
        /// <param name="correlationId">correlation id.</param>
        /// <returns></returns>
        public Task<WidgetPropertyDataModel> GetDashboardWidgetData(string dashboardName, string widgetName, string userId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardMongoStore)}" +
                $" {nameof(GetDashboardWidgetData)}", correlationId);

            var filterWidget = Builders<DashboardWidgets>.Filter.And(
                    Builders<DashboardWidgets>.Filter.Eq(x => x.Dashboard, dashboardName),
                    Builders<DashboardWidgets>.Filter.Eq(x => x.Key, widgetName),
                    Builders<DashboardWidgets>.Filter.Eq(x => x.IsDeleted, false)
                );

            var widgetData = Find<DashboardWidgets>(DASHBOARDWIDGETS_COLLECTION, filterWidget, correlationId)?.FirstOrDefault();

            if (widgetData == null)
            {
                logger.WriteCId(Level.Info, "Missing dashboard widget data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} {nameof(GetDashboardWidgetData)}", correlationId);

                return null;
            }

            var filterUserSettings = Builders<DashboardWidgetUserSettings>.Filter.And(
                    Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.DashboardName, dashboardName),
                    Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.WidgetName, widgetName),
                    Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.UserId, userId),
                    Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.PropertyName, "WidgetProperties"),
                    Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.IsDeleted, false)
                );

            var userSettings = Find<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION,
                filterUserSettings, correlationId)?.FirstOrDefault();

            // Map the widget data to WidgetPropertyDataModel
            var data = new WidgetPropertyDataModel
            {
                Dashboard = widgetData.Dashboard,
                Description = widgetData.Description,
                Key = widgetData.Key,
                Expandable = widgetData.Expandable,
                Widget = widgetData.Widget,
                ExternalUri = widgetData.ExternalUri,
                HttpMethod = widgetData.HttpMethod,
                HttpBody = widgetData.HttpBody,
                WidgetProperties = userSettings != null ? userSettings.PropertyValue : widgetData.WidgetProperties
            };

            logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} {nameof(GetDashboardWidgetData)}", correlationId);

            return Task.FromResult(data);
        }

        /// <summary>
        /// Resets the user preferences for the dashboard widget
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<bool> ResetDashboardWidgetUserPrefeernces(DashboardWidgetPreferenceDataModel input, string userId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DashboardMongoStore)}" +
                $" {nameof(ResetDashboardWidgetUserPrefeernces)}", correlationId);

            if (input == null)
                throw new ArgumentNullException(nameof(input), "User Preferences cannot be null.");

            if (string.IsNullOrWhiteSpace(input.DashboardName) || string.IsNullOrWhiteSpace(input.WidgetName))
                throw new ArgumentNullException(nameof(input), "Invalid dashboard or widget name, name cannot be empty.");

            var filterUserSettings = Builders<DashboardWidgetUserSettings>.Filter.And(
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.DashboardName, input.DashboardName),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.WidgetName, input.WidgetName),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.UserId, userId),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.PropertyName, "WidgetProperties"),
                       Builders<DashboardWidgetUserSettings>.Filter.Eq(x => x.IsDeleted, false)
                   );

            var existing = Find<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION, filterUserSettings, correlationId).FirstOrDefault();

            //if exists delete the preferences
            if (existing != null)
            {
                logger.WriteCId(Level.Info, "Deleting dashboard widget user preferences data.", correlationId);

                await _database.GetCollection<DashboardWidgetUserSettings>(DASHBOARDWIDGETUSERSETTINGS_COLLECTION).DeleteOneAsync(x => x.Id == existing.Id);

                logger.WriteCId(Level.Trace, $"Finished {nameof(DashboardMongoStore)} {nameof(ResetDashboardWidgetUserPrefeernces)}", correlationId);

                return true;
            }
            return true;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Maps a Dashboard entity to a <seealso cref="DashboardWidgetDataModel"/>.
        /// </summary>
        /// <param name="dashboard">The dashboard entity to map.</param>
        /// <param name="widgetProperty">The widget properties.</param>
        /// <param name="userSettings">User overrides.</param>
        /// <param name="isStacked">Is Stacked view dashboard.</param>
        /// <returns>A mapped DashboardWidgetDataModel.</returns>
        private Task<DashboardWidgetDataModel> Map(Dashboards dashboard, IList<DashboardWidgets> widgetProperty,
            IList<DashboardWidgetUserSettings> userSettings, bool isStacked)
        {
            var widgetDataModel = new DashboardWidgetDataModel
            {
                DashboardName = dashboard.Dashboard,
                Editable = dashboard.Editable,
                Widgets = MapWidget(isStacked == true ? dashboard.Widgets.Stacked : dashboard.Widgets.Default,
                            widgetProperty, userSettings)
            };

            return Task.FromResult(widgetDataModel);
        }

        private List<WidgetLayoutDataModel> MapWidget(List<WidgetLayout> widgetLayout,
            IList<DashboardWidgets> widgetProperty, IList<DashboardWidgetUserSettings> userSettings)
        {
            if (widgetLayout == null)
            {
                return null;
            }

            return widgetLayout.Select(x => new WidgetLayoutDataModel
            {
                Show = x.Show,
                Key = x.Key,
                Layout = MapWidgetLayout(x.Layout),
                WidgetProperty = MapWidgetProperty(widgetProperty?.Where(y => y.Key == x.Key), userSettings?.Where(y => y.WidgetName == x.Key))
            }).ToList();
        }

        /// <summary>
        /// Maps a Layout entity to a LayoutDataModel.
        /// </summary>
        /// <param name="layout">The layout entity to map.</param>
        /// <returns>A mapped LayoutDataModel.</returns>
        private LayoutDataModel MapWidgetLayout(Layout layout)
        {
            if (layout == null)
            {
                return null;
            }

            return new LayoutDataModel
            {
                Lg = MapLg(layout.lg)
            };
        }

        /// <summary>
        /// Maps an Lg entity to an LgDataModel.
        /// </summary>
        /// <param name="lg">The Lg entity to map.</param>
        /// <returns>A mapped LgDataModel.</returns>
        private LgDataModel MapLg(Lg lg)
        {
            if (lg == null)
            {
                return null;
            }

            return new LgDataModel
            {
                Height = lg.H,
                Width = lg.W,
                MinH = lg.MinH,
                MinW = lg.MinW,
                X = lg.X,
                Y = lg.Y
            };
        }

        /// <summary>
        /// Maps a collection of DashboardWidgets to a WidgetPropertyDataModel.
        /// </summary>
        /// <param name="widgetProperties">The collection of DashboardWidgets to map.</param>
        /// <param name="userSettings"></param>
        /// <returns>A mapped WidgetPropertyDataModel.</returns>
        private WidgetPropertyDataModel MapWidgetProperty(IEnumerable<DashboardWidgets> widgetProperties,
            IEnumerable<DashboardWidgetUserSettings> userSettings)
        {
            if (widgetProperties == null || !widgetProperties.Any())
            {
                return null;
            }

            var widget = widgetProperties.FirstOrDefault();
            var userSetting = userSettings?.FirstOrDefault();
            object widgetProperty;
            if (widget.Key == "wellTestTable")
            {
                WellTest wellTestProperty;
                if (userSetting != null)
                {
                    wellTestProperty = (WellTest)userSetting.PropertyValue;
                    wellTestProperty.Columns = wellTestProperty.Columns.OrderBy(x => x.Order).ToList();
                    widgetProperty = wellTestProperty;
                }
                else
                {
                    wellTestProperty = (WellTest)widget.WidgetProperties;
                    wellTestProperty.Columns = wellTestProperty.Columns.OrderBy(x => x.Order).ToList();
                    widgetProperty = wellTestProperty;
                }
            }
            else
            {
                widgetProperty = userSetting != null ? userSetting.PropertyValue : widget.WidgetProperties;
            }

            return new WidgetPropertyDataModel
            {
                Dashboard = widget.Dashboard,
                Description = widget.Description,
                Key = widget.Key,
                Expandable = widget.Expandable,
                Widget = widget.Widget,
                ExternalUri = widget.ExternalUri,
                HttpMethod = widget.HttpMethod,
                HttpBody = widget.HttpBody,
                WidgetProperties = widgetProperty
            };
        }

        #endregion

    }
}