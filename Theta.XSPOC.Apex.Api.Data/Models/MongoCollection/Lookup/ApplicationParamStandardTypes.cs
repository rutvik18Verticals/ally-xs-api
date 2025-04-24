namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the alarm event types MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class ApplicationParamStandardTypes : LookupBase
    {

        /// <summary>
        /// Gets or sets the Application Id.
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the Param standard type.
        /// </summary>
        public int ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets if the alaram is used for analysis.
        /// </summary>
        public bool IsUsedForAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets if the alaram is required for analysis.
        /// </summary>
        public bool IsRequiredForAnalysis { get; set; }

        /// <summary>
        /// Gets or sets if the alaram allows note.
        /// </summary>
        public bool? AllowsNote { get; set; }

    }
}
