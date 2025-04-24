using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Services
{
    /// <summary>
    /// This is the interface for processing data updates service.
    /// </summary>
    public interface IProcessingDataUpdatesService
    {

        /// <summary>
        /// Acts on a <paramref name="messageWithCorrelationId"/> from Rabbit MQ and validate the message, 
        /// do any pre processing of the data and send the transaction to the device.
        /// </summary>
        /// <param name="messageWithCorrelationId">The message to process.</param>
        /// <param name="assetGuid">The asset guid.</param>
        /// <returns>Task<seealso cref="ConsumerBaseAction" />></returns>
        public Task<ConsumerBaseAction> ProcessDataUpdatesAsync(
            WithCorrelationId<DataUpdateEvent> messageWithCorrelationId, Guid assetGuid);

        /// <summary>
        /// Creates the transaction payload for the setpoints read and call a method to send the 
        /// Rabbit MQ message to microservices.
        /// </summary>
        /// <param name="assetGuid">The asset guid.</param>
        /// <param name="addressValues">The string array of address values.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>The method result of the rabbit mq opearation.</returns>
        public Task<MethodResult<string>> SendReadRegisterTransaction(Guid assetGuid, string[] addressValues,
            string socketId);

        /// <summary>
        /// Creates the transaction payload for the setpoints write and call a method to send the 
        /// Rabbit MQ message to microservices to process the setpoints.
        /// </summary>
        /// <param name="assetGuid">The asset guid.</param>
        /// <param name="addressValues">The address and value dictionary.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>The method result of the rabbit mq opearation.</returns>
        public Task<MethodResult<string>> SendWriteRegisterTransaction(Guid assetGuid,
            IDictionary<string, string> addressValues,
            string socketId);

        /// <summary>
        /// Acts on a <paramref name="message"/> from Rabbit MQ and validate the message, 
        /// do any pre processing of the data and send the transaction to the device.
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <param name="logger">The <seealso cref="IThetaLogger"/></param>
        /// <returns>Task<seealso cref="ConsumerBaseAction" />></returns>
        public Task<ConsumerBaseAction> ProcessMessageAsync(
            WithCorrelationId<DataUpdateEvent> message, IThetaLogger logger);
    }
}
