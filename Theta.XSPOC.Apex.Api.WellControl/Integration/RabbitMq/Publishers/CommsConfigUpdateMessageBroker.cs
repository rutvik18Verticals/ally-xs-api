using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text.Json;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Integration.RabbitMq;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Contracts;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Api.WellControl.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.RabbitMq.Publishers
{
    /// <summary>
    /// The comms config update message broker.
    /// </summary>
    public class CommsConfigUpdateMessageBroker :
        ExchangePublisherBase<ProcessDataUpdateContract, Responsibility, Loggers, LoggingModel>
    {

        #region Private Members

        private const string TOPIC_EXCHANGE_NAME = "XSPOC.Topic.Exchange";

        private const string DEAD_LETTER_EXCHANGE_NAME = "XSPOC.Topic.Exchange.DeadLetter";
        private const string DEAD_LETTER_EXCHANGE_ROUTING_KEY = "#";
        private const string DEAD_LETTER_QUEUE_NAME = "XSPOC.DeadLetter.Process";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <seealso cref="CommsConfigUpdateMessageBroker"/>.
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <param name="connectionFactory">
        /// The <seealso cref="IConnectionFactory"/> used to establish a connection to Rabbit MQ.
        /// </param>
        /// <param name="loggerFactory">The logger factory.</param>
        public CommsConfigUpdateMessageBroker(IConfiguration configuration, IConnectionFactory connectionFactory,
            IThetaLoggerFactory loggerFactory) : base(configuration, connectionFactory, loggerFactory)
        {
        }

        #endregion

        #region Public Abstract Override Properties

        /// <summary>
        /// Describes the responsibility of this implementation in the context of
        /// <seealso cref="IPublishMessage{T}"/>. This responsibility is to send updates down to the comms wrapper.
        /// </summary>
        public override Responsibility Responsibility => Responsibility.PublishStoreUpdateDataToCommsWrapper;

        #endregion

        #region Protected Abstract Properties

        /// <summary>
        /// Gets the logger details for comms processing. This includes the application name, the logger enum,
        /// and the verbosity key to use.
        /// </summary>
        protected sealed override LoggingModel Logger => LoggingModel.WellControl;

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Prepares the provided input <paramref name="input"/> to be transmitted to RabbitMQ.
        /// </summary>
        /// <param name="input">A <seealso cref="WithCorrelationId{T}"/> object containing the data to transmit
        /// alongside a correlation id used to track a request through the system.</param>
        /// <param name="nodeId">The node id from the input if one exists.</param>
        /// <exception cref="ArgumentException">
        /// When <paramref name="input.Value"/> is null.
        /// </exception>
        /// <returns>
        /// The byte array to be transmitted, and the node id if the node id is not present
        /// the node id is an empty string.
        /// </returns>
        protected override byte[] PrepareMessage(WithCorrelationId<ProcessDataUpdateContract> input, out string nodeId)
        {
            var logger = LoggerFactory.Create(Logger);

            if (input?.Value == null)
            {
                logger.WriteCId(Level.Error, $"{nameof(input)} is null after preparing message for transmit.",
                    input?.CorrelationId);

                throw new ArgumentException(nameof(input.Value));
            }

            var correlationId = input.CorrelationId;
            nodeId = null;

            try
            {
                logger.WriteCNId(Level.Info, "Starting to prepare message", correlationId, nodeId);

                var messageBytes = JsonSerializer.SerializeToUtf8Bytes(DataUpdateEventMapper.Map(input.Value));

                logger.WriteCNId(Level.Info, "Finished preparing message", correlationId, nodeId);

                return messageBytes;
            }
            catch (Exception ex)
            {
                logger.WriteCNId(Level.Error, "Error attempting to transmit message", ex,
                    correlationId, nodeId);
            }

            return null;
        }

        /// <summary>
        /// Set any additional configuration that needs to be done during the initialize process
        /// such as the TopicExchangeName.
        /// </summary>
        protected override void SetConfigurationOnInit()
        {
            TopicExchangeName = Config.GetSection("TopicExchanges:DataStoreUpdatesExchange").Value ?? TOPIC_EXCHANGE_NAME;

            DeadLetterExchangeName = Config.GetSection("TopicExchanges:DeadLetterExchange").Value ??
                DEAD_LETTER_EXCHANGE_NAME;
            DeadLetterExchangeRoutingKey = Config.GetSection("TopicExchanges:DeadLetterExchangeRoutingKey").Value ??
                DEAD_LETTER_EXCHANGE_ROUTING_KEY;
            DeadLetterQueueName = Config.GetSection("TopicExchanges:DeadLetterQueue").Value ??
                DEAD_LETTER_QUEUE_NAME;
        }

        #endregion

    }
}