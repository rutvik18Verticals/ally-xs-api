using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the gas liquid ratio curve annotation.
    /// </summary>
    public class GasLiquidRatioCurveAnnotation : IAnalysisCurveSetMemberAnnotationData
    {

        /// <summary>
        /// The Gas Liquid Ratio used to generate this curve
        /// </summary>
        public double GasLiquidRatio { get; set; }

        /// <summary>
        /// Indicates whether this curve is considered a primary curve
        /// </summary>
        public bool IsPrimaryCurve { get; set; }

    }
}
