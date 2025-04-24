using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a correlation used in a calculation.
    /// </summary>
    public class AnalysisCorrelation : IdentityBase
    {

        /// <summary>
        /// The name of the correlation
        /// </summary>
        public Correlation Correlation { get; set; }

        /// <summary>
        /// The type of correlation
        /// </summary>
        public CorrelationType CorrelationType { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new CorrelationType with a specified correlation and correlation type
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="correlation">The correlation</param>
        /// <param name="correlationType">The correlation type</param>
        public AnalysisCorrelation(object id, Correlation correlation, CorrelationType correlationType) : base(id)
        {
            Correlation = correlation;
            CorrelationType = correlationType;
        }

        /// <summary>
        /// Initializes a new CorrelationType with a specified id
        /// </summary>
        /// <param name="id">The id</param>
        public AnalysisCorrelation(object id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new CorrelationType with a <seealso cref="AnalysisCorrelationModel"/> values.
        /// </summary>
        /// <param name="model">The <see cref="AnalysisCorrelationModel"/></param>
        public AnalysisCorrelation(AnalysisCorrelationModel model)
        {
            if (model != null)
            {
                SetId(model.Id);
                Correlation = EnhancedEnumBase.GetValue<Correlation>(model.CorrelationId);
                CorrelationType = EnhancedEnumBase.GetValue<CorrelationType>(model.CorrelationTypeId);
            }
        }

        #endregion

    }
}
