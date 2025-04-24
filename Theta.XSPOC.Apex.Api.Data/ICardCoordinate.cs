using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents Card Coordinates.
    /// </summary>
    public interface ICardCoordinate
    {

        /// <summary>
        /// Get the card coordinate data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset id/node id.</param>
        /// <param name="cardDate">The card date.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The <seealso cref="CardCoordinateModel"/>.</returns>
        CardCoordinateModel GetCardCoordinateData(Guid assetId, string cardDate, string correlationId);

    }
}
