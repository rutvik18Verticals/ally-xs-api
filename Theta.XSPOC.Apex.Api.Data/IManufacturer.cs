using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents manufacturer operations.
    /// </summary>
    public interface IManufacturer
    {
        /// <summary>
        /// Retrieves a manufacturer by its Id.
        /// </summary>
        /// <param name="id">The Id of the manufacturer.</param>
        /// <param name="correlationId">The correlation Id for tracking purposes.</param>
        /// <returns>The manufacturer model.</returns>
        public GLManufacturerModel Get(int id, string correlationId);
    }
}
