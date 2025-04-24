using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Logging
{
    /// <summary>
    /// Class that will store all the information the <seealso cref="IThetaLoggerFactory"/> will need to create
    /// <seealso cref="IThetaLogger"/>. This will include the <seealso cref="Loggers"/>, verbosity key, and
    /// application service name. 
    /// </summary>
    public class LoggingModel : LoggingModelBase<Loggers>
    {

        private const string AppName = "SQL Store";

        #region Public Static Properties

        /// <summary>
        /// Gets the SQL store logging model.
        /// </summary>
        public static LoggingModel SQLStore { get; } = CreateValue(1, Loggers.SQLStore, "SQLStoreLogVerbosity",
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
        /// <param name="verbosityKey">The verbosity key.</param>
        /// <param name="applicationName">The application name.</param>
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
