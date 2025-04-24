using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a generic analysis result curve for the tornado curve.
    /// </summary>
    public class AnalysisCurveSet : IdentityBase
    {

        /// <summary>
        /// Gets or sets the analysis result source.
        /// </summary>
        /// <value>
        /// The analysis result source.
        /// </value>
        public AnalysisResultSource AnalysisResultSource { get; set; }

        /// <summary>
        /// Gets or sets the analysis result identifier.
        /// </summary>
        /// <value>
        /// The analysis result identifier.
        /// </value>
        public int AnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets the type of the curve set.
        /// </summary>
        /// <value>
        /// The type of the curve set.
        /// </value>
        public CurveSetType CurveSetType { get; set; }

        /// <summary>
        /// Gets or sets the curve set identifier.
        /// </summary>
        /// <value>
        /// The curve set identifier.
        /// </value>
        public int CurveSetId { get; set; }

        /// <summary>
        /// Gets or sets the curves within this set.
        /// </summary>
        /// <value>
        /// The curves.
        /// </value>
        public IList<AnalysisCurveSetMemberBase> Curves { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new AnalysisResult with a default identifier.
        /// </summary>
        public AnalysisCurveSet()
        {
        }

        /// <summary>
        /// Initializes a new AnalysisResult with a specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public AnalysisCurveSet(object id)
            : base(id)
        {
        }

        #endregion

    }
}
