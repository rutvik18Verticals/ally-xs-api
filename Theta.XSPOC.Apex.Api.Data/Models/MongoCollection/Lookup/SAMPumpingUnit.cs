namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the SAM pumping unit to mongo sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class SAMPumpingUnit : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the SAM pumping unit.
        /// </summary>
        public int SAMUnitId { get; set; }

        /// <summary>
        /// Gets or sets the Lufkin unit Id.
        /// </summary>
        public string LufkinUnitId { get; set; }

        /// <summary>
        /// Gets or sets the description of the Lufkin pumping unit.
        /// </summary>
        public string LufkinUnitDescription { get; set; }

        /// <summary>
        /// Gets or sets the Theta Id (nullable).
        /// </summary>
        public string ThetaId { get; set; }

    }
}
