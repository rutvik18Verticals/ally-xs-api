using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This interface represents card data operations.
    /// </summary>
    public interface IESPPump
    {

        /// <summary>
        /// Fetches the ESP pump with a specified ID
        /// </summary>
        /// <param name="id">The ID of the ESP pump</param>
        /// <param name="correlationId"></param>
        /// <returns>The ESP pump with the specified ID if found; otherwise, null</returns>
        ESPPumpDataModel GetESPPumpData(object id, string correlationId);

    }
}
