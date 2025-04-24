using System;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Get the analysis curve set member utility.
    /// </summary>
    public static class AnalysisCurveSetMemberUtility
    {

        /// <summary>
        /// Returns an instance of the AnalysisCurvesetMemberBase that represents the type of curve set provided
        /// </summary>
        /// <param name="curveSetType">The curve set type</param>
        /// <returns>
        /// A class derived from AnalysisCurveSetMemberBase that represents the given curve set type
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static AnalysisCurveSetMemberBase GetAnalysisCurveSetMember(CurveSetType curveSetType)
        {
            if (curveSetType == CurveSetType.FBHP)
            {
                return new GasLiquidRatioCurve();
            }
            else if (curveSetType == CurveSetType.Tornado)
            {
                return new TornadoCurve();
            }

            throw new ArgumentException("The given curveSetType is not supported", nameof(curveSetType));
        }

    }
}
