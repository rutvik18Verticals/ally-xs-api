namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a rod lift analysis value response that needs to be send out.
    /// </summary>
    public class RodLiftAnalysisResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public RodLiftAnalysisValues Values { get; set; }

    }
}
