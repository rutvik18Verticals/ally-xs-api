using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the id generator class.
    /// </summary>
    public static class IdGenerator
    {

        #region Methods

        /// <summary>
        /// Generates an id for a specified ESP curve point
        /// </summary>
        /// <param name="curvePoint">The curve point to generate and ID for</param>
        /// <returns>The generated ID</returns>
        /// <exception cref="ArgumentNullException">curvePoint is null</exception>
        public static int GenerateId(this ESPCurvePointModel curvePoint)
        {
            if (curvePoint == null)
            {
                throw new ArgumentNullException(nameof(curvePoint));
            }

            return MathUtility.GenerateHashCode(curvePoint.ESPPumpID, curvePoint.FlowRate);
        }

        /// <summary>
        /// Generates an ID for pump coefficients with a specified pump ID and index
        /// </summary>
        /// <param name="pumpId">The ID of the pump</param>
        /// <param name="source">The source of the coefficients</param>
        /// <returns>The generated ID</returns>
        public static int GenerateIdForPumpCoefficients(int pumpId, PumpCoefficientSource source)
        {
            return MathUtility.GenerateHashCode(pumpId, source);
        }

        #endregion

    }
}
