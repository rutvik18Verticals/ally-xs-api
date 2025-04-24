using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Logging
{
    /// <summary>
    /// Class that will store all the information the <seealso cref="IThetaLoggerFactory"/> will need to create
    /// <seealso cref="IThetaLogger"/>. This will include the <seealso cref="Loggers"/>, verbosity key, and
    /// application service name. 
    /// </summary>
    public class LoggingModel : LoggingModelBase<Loggers>
    {

        private const string AppName = "WellControl API";

        #region Public Static Properties

        /// <summary>
        /// Gets the data store service logging model.
        /// </summary>
        public static LoggingModel DataStoreService { get; } =
            CreateValue(1, Loggers.DataStoreService, "DbStoreLogVerbosity", AppName);

        /// <summary>
        /// Gets the MongoDB service logging model.
        /// </summary>
        public static LoggingModel MongoDataStore { get; } =
            CreateValue(2, Loggers.MongoDataStore, "MongoStoreLogVerbosity", AppName);

        /// <summary>
        /// Gets the well control logging model.
        /// </summary>
        public static LoggingModel WellControl { get; } =
            CreateValue(3, Loggers.WellControl, "WellControlLogVerbosity", AppName);

        #endregion

        #region Constructors

        /// <summary>
        /// A private constructor that is only used by this class. No calling code can create an instance.
        /// </summary>
        private LoggingModel(int key, Loggers loggers, string verbosityKey, string applicationName) : base(key,
            loggers, verbosityKey, applicationName)
        {
        }

        #endregion

        #region Private Static Methods

        private static LoggingModel CreateValue(int key, Loggers loggers, string verbosityKey, string applicationName)
        {
            var value = new LoggingModel(key, loggers, verbosityKey, applicationName);

            Register(value);

            return value;
        }

        #endregion

    }
}