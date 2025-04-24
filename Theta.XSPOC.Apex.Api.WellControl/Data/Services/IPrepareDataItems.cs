using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Integration;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services
{
    /// <summary>
    /// This interface defines the operations needed to prepare data items to be stored to the data store.
    /// </summary>
    public interface IPrepareDataItems
    {

        /// <summary>
        /// Acts on a <paramref name="messageWithCorrelationId"/> from Rabbit MQ and validate the message, 
        /// do any pre processing of the data and send the data to the db store for operation.
        /// </summary>
        /// <param name="messageWithCorrelationId">The message to process.</param>
        /// <returns>A <seealso cref="ConsumerBaseAction"/> based on the operation results.</returns>
        Task<ConsumerBaseAction> PrepareDataItemsAsync(WithCorrelationId<DataUpdateEvent> messageWithCorrelationId);

    }
}
