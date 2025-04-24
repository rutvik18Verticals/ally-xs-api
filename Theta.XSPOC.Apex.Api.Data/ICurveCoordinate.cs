using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents curve coordinate.
    /// </summary>
    public interface ICurveCoordinate
    {

        /// <summary>
        /// Get the curve coordinate by curve id.
        /// </summary>
        /// <param name="curveId">The curve id.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The <seealso cref="CurveCoordinatesModel"/>.</returns>
        IList<CurveCoordinatesModel> GetCurvesCoordinates(int curveId, string correlationId);

    }
}
