using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Integration.RabbitMq;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.RabbitMq.Consumers
{
    /// <summary>
    /// The implementation to receive and process data update messages from RabbitMQ.
    /// </summary>
    public class ProcessDataUpdates : ExchangeConsumerBase<DataUpdateEvent, Responsibility, Loggers, LoggingModel>
    {

        #region Private Members

        private readonly IProcessingDataUpdatesService _processingDataUpdatesService;

        private const string EXCHANGE_TYPE = "fanout";
        private const string TOPIC_EXCHANGE_NAME = "XSPOC.Api.Exchange";
        private const string QUEUE_PREFIX = "XSPOC.Api.WellControl.Response";

        // Empty routing key for fanout exchange.
        private const string BASE_ROUTING_KEY = "";
        
        private const string DEAD_LETTER_EXCHANGE_NAME = "XSPOC.Topic.Exchange.DeadLetter";
        private const string DEAD_LETTER_EXCHANGE_ROUTING_KEY = "#";
        private const string DEAD_LETTER_QUEUE_NAME = "XSPOC.DeadLetter.Process";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an instance of <seealso cref="ProcessDataUpdates"/>.
        /// </summary>
        /// <param name="config">The <seealso cref="IConfiguration"/>.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> to obtain loggers from.</param>
        /// <param name="connectionFactory">
        /// The <seealso cref="IConnectionFactory"/> used to establish a connection to Rabbit MQ.
        /// </param>
        /// <param name="processingDataUpdatesService">The <seealso cref="IProcessingDataUpdatesService"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="config"/> is null
        /// OR
        /// when <paramref name="loggerFactory"/> is null
        /// OR
        /// when <paramref name="connectionFactory"/> is null.
        /// </exception>
        public ProcessDataUpdates(IConfiguration config,
            IThetaLoggerFactory loggerFactory, IConnectionFactory connectionFactory,
            IProcessingDataUpdatesService processingDataUpdatesService)
            : base(config, connectionFactory, loggerFactory)
        {
            _processingDataUpdatesService = processingDataUpdatesService ??
                throw new ArgumentNullException(nameof(processingDataUpdatesService));
        }

        #endregion

        #region Protected Abstract Override Properties

        /// <summary>
        /// Gets the logger details for comm data updates. This includes the application name, the logger enum, and the
        /// verbosity key to use.
        /// </summary>
        protected sealed override LoggingModel Logger => LoggingModel.WellControl;

        #endregion

        #region Public Abstract Override Properties

        /// <summary>
        /// Describes the responsibility of this implementation in the context of
        /// <seealso cref="IConsumeMessage{TResponsibility}"/>. This responsibility is to consume updates from  XSPOC.
        /// </summary>
        public sealed override Responsibility Responsibility => Responsibility.ConsumeTransationUpdateFromMicroservices;

        #endregion

        #region Protected Abstract Override Methods

        /// <summary>
        /// Method to process the message from microservices regarding transaction processing from comms processing.
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <param name="basicProperties">
        /// The basic properties provided by RabbitMQ via the <seealso cref="IBasicProperties"/> interface. This
        /// contains items like the correlation id, reply queue, ect.
        /// </param>
        /// <returns>ConsumerBaseAction</returns>
        protected override async Task<ConsumerBaseAction> ProcessMessage(WithCorrelationId<DataUpdateEvent> message,
            IBasicProperties basicProperties)
        {
            var logger = LoggerFactory.Create(Logger);
            logger.WriteCId(Level.Debug, "Starting processing data  updates service", message?.CorrelationId);

            return await _processingDataUpdatesService.ProcessMessageAsync(message, logger);
        }

        /// <summary>
        /// Set any additional configuration that needs to be done during the initialize process
        /// such as the TopicExchangeName and BaseRoutingKey.
        /// </summary>
        protected override void SetConfigurationOnInit()
        {
            ExchangeType = Config.GetSection("TopicExchanges:ApiExchangeType").Value ??
                EXCHANGE_TYPE;
            QueuePrefix = Config.GetSection("TopicExchanges:ApiExchangeQueuePrefix").Value ?? QUEUE_PREFIX;
            TopicExchangeName = Config.GetSection("TopicExchanges:ApiExchange").Value ??
                TOPIC_EXCHANGE_NAME;
            BaseRoutingKey = Config.GetSection("TopicExchanges:WellControlResponseRoutingKey").Value ?? BASE_ROUTING_KEY;

            DeadLetterExchangeName = Config.GetSection("TopicExchanges:DeadLetterExchange").Value ??
                DEAD_LETTER_EXCHANGE_NAME;
            DeadLetterExchangeRoutingKey = Config.GetSection("TopicExchanges:DeadLetterExchangeRoutingKey").Value ??
                DEAD_LETTER_EXCHANGE_ROUTING_KEY;
            DeadLetterQueueName = Config.GetSection("TopicExchanges:DeadLetterQueue").Value ??
                DEAD_LETTER_QUEUE_NAME;
        }

        /// <summary>
        /// Method to add route keys for a consumer listening to a topic exchange.
        /// </summary>
        protected sealed override void AddRouteKeys()
        {

        }

        #endregion

    }
}
