namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The GasLiquidRatioCurveAnnotationModel.
    /// </summary>
    public class GasLiquidRatioCurveAnnotationModel
    {

        /// <summary>
        /// The curve set member id.
        /// </summary>
        public int CurveSetMemberId { get; set; }

        /// <summary>
        /// The Gas Liquid Ratio used to generate this curve.
        /// </summary>
        public double GasLiquidRatio { get; set; }

        /// <summary>
        /// Indicates whether this curve is considered a primary curve.
        /// </summary>
        public bool IsPrimaryCurve { get; set; }

    }
}
