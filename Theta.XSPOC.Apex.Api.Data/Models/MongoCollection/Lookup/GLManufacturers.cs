namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the GL valve configuration pptions MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class GLManufacturers : LookupBase
    {

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        public bool Locked { get; set; }

    }
}
