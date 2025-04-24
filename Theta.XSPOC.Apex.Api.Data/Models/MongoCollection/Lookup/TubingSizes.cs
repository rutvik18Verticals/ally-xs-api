namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the tubing sizes MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class TubingSizes : LookupBase
    {

        /// <summary>
        /// Gets or sets the Tubing sizes Id.
        /// </summary>
        public int TubingSizesId { get; set; }

        /// <summary>
        /// Gets or sets the Tubing name.
        /// </summary>
        public string TubingName { get; set; }

        /// <summary>
        /// Gets or sets the Tubing OD.
        /// </summary>
        public float TubingOD { get; set; }

        /// <summary>
        /// Gets or sets the Tubing Id.
        /// </summary>
        public float TubingId { get; set; }

        /// <summary>
        /// Gets or sets the Tubing weight.
        /// </summary>
        public float? TubingWeight { get; set; }

    }
}
