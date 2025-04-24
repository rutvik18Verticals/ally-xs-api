using Theta.XSPOC.Apex.Kernel.Integration;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Describes the responsibilities that implementations of <seealso cref="IPublishMessage{T}"/> or
    /// <seealso cref="IConsumeMessage{T}"/> may take on.
    /// </summary>
    public enum Responsibility
    {
        /// <summary>
        /// Consume data updates from Microservices.
        /// </summary>
        ConsumeTransationUpdateFromMicroservices = 1,

        /// <summary>
        /// Consume data updates from Legacy DB Store.
        /// </summary>
        ConsumeStoreUpdateFromLegacyDBStore = 2,

        /// <summary>
        /// Publish data updates to Microservices.
        /// </summary>
        PublishTransationDataToMicroservices = 3,

        /// <summary>
        /// Publish store update data to Legacy DB Store.
        /// </summary>
        PublishStoreUpdateDataToLegacyDBStore = 4,

        /// <summary>
        /// Publish data updates to comms wrapper.
        /// </summary>
        PublishStoreUpdateDataToCommsWrapper = 6,

        /// <summary>
        /// Publish the transaction id that is to be monitored to the listener.
        /// </summary>
        PublishTransationIdToListener = 7,

    }
}
