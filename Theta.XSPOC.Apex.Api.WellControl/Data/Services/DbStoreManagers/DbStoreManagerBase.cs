using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers
{
    /// <summary>
    /// A base class that provides common functionality, such as logging and a basic method for persisting data,
    /// to DbStoreManager implementations.
    /// </summary>
    public abstract class DbStoreManagerBase
    {

        #region Private Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets the <see cref="IThetaLoggerFactory"/>.
        /// </summary>
        protected IThetaLoggerFactory LoggerFactory { get; set; }

        #endregion

        #region Protected Virtual Properties

        /// <summary>
        /// Gets the logging model. The default is <see cref="LoggingModel"/>.MongoDataStore.
        /// </summary>
        protected virtual LoggingModel StoreManagerLoggingModel => LoggingModel.MongoDataStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DbStoreManagerBase"/>.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="IThetaLoggerFactory"/>.</param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null OR
        /// <paramref name="configuration"/> is null.
        /// </exception>
        public DbStoreManagerBase(IThetaLoggerFactory loggerFactory, IConfiguration configuration)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Override this method in order to leverage UpdateAsync.
        /// </summary>
        protected abstract TDocument Map<TPayload, TDocument>(TPayload payload) where TDocument : class;

        /// <summary>
        /// Override this method in order to leverage UpdateAsync.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        protected abstract Task<bool> UpdateDbStore<TDocument>(TDocument document);

        #endregion

        #region Public Methods

        /// <summary>
        /// Manages exactly one DbStore to asynchronously update the underlying MongoDB data store. This method can be
        /// used to quickly implement DbStoreManagers that has only 1 DbStore to manage. It can be extended in future
        /// to possibly allow more, or can be called more than once, or, the DbStoreManager needing to handle more than
        /// one DbStore can implement its logic directly and not leverage this functionality.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
        protected async Task<DbStoreResult> UpdateAsync<TPayload, TDocument>(string payload,
            string correlationId)
            where TDocument : class
        {
            var logger = LoggerFactory.Create(StoreManagerLoggingModel);
            var payloadType = typeof(TPayload).Name;

            logger.WriteCId(Level.Info, $"Updating {payloadType}.", correlationId);

            var mongoDbRetriesCount = 0;
            var mongoDbIntervalMilliseconds = 0;
            var isCompleted = false;
            var result = new DbStoreResult();
            if (_configuration.GetSection("MongodbSettings:Retries").Value != null)
            {
                if (int.TryParse(_configuration.GetSection("MongodbSettings:Retries").Value, out int retriesCount))
                {
                    mongoDbRetriesCount = retriesCount;
                }
            }

            if (_configuration.GetSection("MongodbSettings:IntervalMilliseconds").Value != null)
            {
                if (int.TryParse(_configuration.GetSection("MongodbSettings:IntervalMilliseconds").Value, out int intervalMilliseconds))
                {
                    mongoDbIntervalMilliseconds = intervalMilliseconds;
                }
            }

            if (mongoDbRetriesCount < 0)
            {
                mongoDbRetriesCount = 0;
            }

            if (mongoDbIntervalMilliseconds < 0)
            {
                mongoDbIntervalMilliseconds = 0;
            }

            while (mongoDbRetriesCount >= 0 && !isCompleted)
            {
                TPayload deserializedPayload;
                try
                {
                    logger.WriteCId(Level.Debug, $"Deserializing to {payloadType}.", correlationId);
                    logger.WriteCId(Level.Trace, $"Serialized payload received {payload}.", correlationId);

                    deserializedPayload = JsonConvert.DeserializeObject<TPayload>(payload);

                    logger.WriteCId(Level.Debug, $"Successfully deserialized to {payloadType}.", correlationId);
                }
                catch (Exception ex)
                {
                    logger.WriteCId(Level.Error, "Deserialization failed.", ex, correlationId);

                    result = new DbStoreResult()
                    {
                        Message = ex.Message,
                        KindOfError = ErrorType.NotRecoverable,
                    };

                    return result;
                }

                if (deserializedPayload == null)
                {
                    logger.WriteCId(Level.Error, "Deserialization returned an empty result.", correlationId);

                    result = new DbStoreResult()
                    {
                        Message = "Null payload received",
                        KindOfError = ErrorType.NotRecoverable,
                    };

                    return result;
                }

                var documentType = typeof(TDocument).Name;

                TDocument dataModel;
                try
                {

                    logger.WriteCId(Level.Debug, $"Mapping to {documentType}.", correlationId);

                    dataModel = Map<TPayload, TDocument>(deserializedPayload);

                    logger.WriteCId(Level.Debug, $"Successfully mapped to {documentType}.", correlationId);
                }
                catch (Exception ex)
                {
                    logger.WriteCId(Level.Error, "Mapping failed.", ex, correlationId);

                    result = new DbStoreResult()
                    {
                        Message = ex.Message,
                        KindOfError = ErrorType.NotRecoverable,
                    };

                    return result;
                }

                if (dataModel == null)
                {
                    logger.WriteCId(Level.Error, "Mapping failed: null returned from mapper.", correlationId);

                    result = new DbStoreResult()
                    {
                        Message = "At least one required identifier was missing from the input. Mapping failed.",
                        KindOfError = ErrorType.NotRecoverable,
                    };

                    return result;
                }

                try
                {
                    logger.WriteCId(Level.Debug, $"Making database call to persist {documentType}.", correlationId);

                    await UpdateDbStore(dataModel);

                    logger.WriteCId(Level.Debug, $"{documentType} persisted.", correlationId);
                }
                catch (Exception ex)
                {
                    logger.WriteCId(Level.Error, "Saving to database failed.", ex, correlationId);

                    result = new DbStoreResult()
                    {
                        Message = ex.Message,
                        KindOfError = ErrorType.NotRecoverable,
                    };

                    return result;
                }
                if (result.KindOfError == ErrorType.LikelyRecoverable)
                {
                    await Task.Delay(mongoDbIntervalMilliseconds);
                    isCompleted = false;
                }

                if (result.KindOfError == ErrorType.None)
                {
                    isCompleted = true;
                }

                mongoDbRetriesCount--;
            }

            logger.WriteCId(Level.Info, $"Successfully updated {payloadType}.", correlationId);

            return new DbStoreResult()
            {
                KindOfError = ErrorType.None,
            };
        }

        #endregion

    }
}
