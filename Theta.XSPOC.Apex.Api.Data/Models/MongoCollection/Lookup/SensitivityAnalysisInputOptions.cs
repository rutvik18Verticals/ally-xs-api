namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the sensitivity analysis input options MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class SensitivityAnalysisInputOptions : LookupBase
    {

        /// <summary>
        /// Gets or sets the Sensitivity analysis input options Id.
        /// </summary>
        public int SensitivityAnalysisInputOptionsId { get; set; }

        /// <summary>
        /// Gets or sets the Application Id.
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the Input Id.
        /// </summary>
        public int InputId { get; set; }

        /// <summary>
        /// Gets or sets the Input type Id.
        /// </summary>
        public int InputTypeId { get; set; }

        /// <summary>
        /// Gets or sets if the alarm configuration is locked.
        /// </summary>
        public bool? IsLocked { get; set; }

    }
}
