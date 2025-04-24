namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the analysis correlations MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class AnalysisCorrelations : LookupBase
    {

        /// <summary>
        /// Gets or sets the Correlation Id.
        /// </summary>
        public int CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the Correlation type Id.
        /// </summary>
        public int CorrelationTypeId { get; set; }

    }
}
