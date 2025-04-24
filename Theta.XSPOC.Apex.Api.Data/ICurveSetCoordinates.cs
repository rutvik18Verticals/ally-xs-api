using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents curve set coordinate.
    /// </summary>
    public interface ICurveSetCoordinates
    {

        /// <summary>
        /// Get the curve set coordinate by curve id.
        /// </summary>
        /// <param name="curveId">The curve id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="CurveSetCoordinatesModel"/>.</returns>
        IList<CurveSetCoordinatesModel> GetCurveSetCoordinates(int curveId, string correlationId);

    }
}
