using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents an ESP tornado curve type.
    /// </summary>
    public class TornadoCurve : AnalysisCurveSetMemberBase
    {

        private TornadoCurveAnnotation _data;

        /// <summary>
        /// The annotations that help describe the curve 
        /// </summary>
        public override IAnalysisCurveSetMemberAnnotationData AnnotationData
        {
            get => _data;
            set => _data = (TornadoCurveAnnotation)value;
        }

    }
}
