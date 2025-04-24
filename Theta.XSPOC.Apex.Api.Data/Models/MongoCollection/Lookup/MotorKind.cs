namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the motor kind to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class MotorKind : LookupBase
    {

        /// <summary>
        /// Gets or sets the MotorKind Id.
        /// </summary>
        public int MotorKindId { get; set; }

        /// <summary>
        /// Gets or sets the Name (nullable).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the UHS (nullable).
        /// </summary>
        public bool? UHS { get; set; }

    }
}
