using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the gas liquid ratio curve.
    /// </summary>
    public class GasLiquidRatioCurve : AnalysisCurveSetMemberBase
    {

        private GasLiquidRatioCurveAnnotation _data;

        /// <summary>
        /// The annotations that help describe the curve 
        /// </summary>
        public override IAnalysisCurveSetMemberAnnotationData AnnotationData
        {
            get => _data;
            set => _data = value as GasLiquidRatioCurveAnnotation;
        }

    }
}
