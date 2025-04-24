using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the tornado curve annotation.
    /// </summary>
    public class TornadoCurveAnnotation : IAnalysisCurveSetMemberAnnotationData
    {

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public double Frequency { get; set; }

    }
}
