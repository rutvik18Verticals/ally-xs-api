using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the result of an analyzing an well test.
    /// </summary>
    public class ArtificalLiftAnalysisResultBase : IdentityBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the date and time that the well test was processed (analyzed).
        /// </summary>
        public DateTime ProcessedDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the analysis was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a message summarizing the result of the analysis.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets a localized message summarizing the result of the analysis.
        /// </summary>
        public string ResultMessageLocalized { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new AnalysisResult with a default identifier.
        /// </summary>
        public ArtificalLiftAnalysisResultBase()
        {
        }

        /// <summary>
        /// Initializes a new AnalysisResult with a specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public ArtificalLiftAnalysisResultBase(object id)
            : base(id)
        {
        }

        #endregion

    }
}
