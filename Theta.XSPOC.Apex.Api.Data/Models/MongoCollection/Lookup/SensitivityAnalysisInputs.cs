namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the sensitivity analysis inputs MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class SensitivityAnalysisInputs : LookupBase
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int SensitivityAnalysisInputsId { get; set; }

        /// <summary>
        /// Gets or sets the Sensitivity analysis Id.
        /// </summary>
        public int SensitivityAnalysisId { get; set; }

        /// <summary>
        /// Gets or sets the Input option Id.
        /// </summary>
        public int InputOptionId { get; set; }

        /// <summary>
        /// Gets or sets the Input value.
        /// </summary>
        public float? InputValue { get; set; }

    }
}
