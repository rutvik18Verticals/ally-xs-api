namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the analysis input default MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class AnalysisInputDefaults : LookupBase
    {

        /// <summary>
        /// Gets or sets the Param Standard Type.
        /// </summary>
        public int ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public float DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        public bool Locked { get; set; }

    }
}
