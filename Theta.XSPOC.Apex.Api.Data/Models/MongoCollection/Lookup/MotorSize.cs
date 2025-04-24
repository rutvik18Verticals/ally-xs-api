namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the motor size to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class MotorSize : LookupBase
    {

        /// <summary>
        /// Gets or sets the MotorSize Id.
        /// </summary>
        public int MotorSizeId { get; set; }

        /// <summary>
        /// Gets or sets the Name (nullable).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the MotorKind Id (nullable).
        /// </summary>
        public int? MotorKindId { get; set; }

    }
}
