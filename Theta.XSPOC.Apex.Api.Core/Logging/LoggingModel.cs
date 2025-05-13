using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Logging
{
    /// <summary>
    /// Class that will store all the information the <seealso cref="IThetaLoggerFactory"/> will need to create
    /// <seealso cref="IThetaLogger"/>. This will include the <seealso cref="Loggers"/>, verbosity key, and
    /// application service name. 
    /// </summary>
    public class LoggingModel : LoggingModelBase<Loggers>
    {

        private const string AppName = "API";

        #region Public Static Properties

        /// <summary>
        /// Gets the notification logging model.
        /// </summary>
        public static LoggingModel Notification { get; } = CreateValue(1, Loggers.Notification, "NotificationLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the rod lift analysis logging model.
        /// </summary>
        public static LoggingModel RodLiftAnalysis { get; } = CreateValue(2, Loggers.RodLiftAnalysis, "RodLiftAnalysisLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the esp analysis logging model.
        /// </summary>
        public static LoggingModel ESPAnalysis { get; } = CreateValue(3, Loggers.ESPAnalysis, "ESPAnalysisLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the Well Test logging model.
        /// </summary>
        public static LoggingModel WellTest { get; } = CreateValue(4, Loggers.WellTest, "WellTestLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the rod lift analysis logging model.
        /// </summary>
        public static LoggingModel GLAnalysis { get; } = CreateValue(5, Loggers.GLAnalysis, "GLAnalysisLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the group and asset logging model.
        /// </summary>
        public static LoggingModel GroupAndAsset { get; } = CreateValue(6, Loggers.GroupAndAsset, "GroupAndAssetLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the group and asset logging model.
        /// </summary>
        public static LoggingModel TrendData { get; } = CreateValue(7, Loggers.TrendData, "TrendDataLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the group Status logging model.
        /// </summary>
        public static LoggingModel GroupStatus { get; } = CreateValue(8, Loggers.GroupStatus, "GroupStatusLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the MongoDB service logging model.
        /// </summary>
        public static LoggingModel MongoDataStore { get; } =
            CreateValue(9, Loggers.MongoDataStore, "MongoStoreLogVerbosity", "WellControl API");

        /// <summary>
        /// Gets the Login logging model.
        /// </summary>
        public static LoggingModel Login { get; } = CreateValue(11, Loggers.Login, "LoginLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the user default logging model.
        /// </summary>
        public static LoggingModel UserDefault { get; } = CreateValue(12, Loggers.UserDefault, "UserDefaultLogVerbosity", "User API");

        /// <summary>
        /// Gets the api service logging model.
        /// </summary>
        public static LoggingModel APIService { get; } = CreateValue(13, Loggers.APIService, "APIServiceLogVerbosity",
            AppName);

        /// <summary>
        /// Gets the group and asset logging model.
        /// </summary>
        public static LoggingModel DashboardWidget { get; } = CreateValue(14, Loggers.DashboardWidget, "DashboardWidgetLogVerbosity",
            AppName);

        #endregion

        #region Constructors

        /// <summary>
        /// A private constructor that is only used by this class. No calling code can create an instance.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="loggers">The loggers.</param>
        /// <param name="verbosityKey">The verbosity key.</param>
        /// <param name="applicationName">The application name.</param>
        private LoggingModel(int key, Loggers loggers, string verbosityKey, string applicationName) : base(key,
            loggers, verbosityKey, applicationName)
        {
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Create logging model by provided parameters key, loggers, verbosityKey and applicationName.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="loggers">The loggers.</param>
        /// <param name="verbosityKey">The verbositykey</param>
        /// <param name="applicationName">The applicationname</param>
        /// <returns>The <seealso cref="LoggingModel"/>.</returns>
        private static LoggingModel CreateValue(int key, Loggers loggers, string verbosityKey, string applicationName)
        {
            var value = new LoggingModel(key, loggers, verbosityKey, applicationName);

            Register(value);

            return value;
        }

        #endregion

    }

}
