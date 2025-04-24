namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the esp event types MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class ESPEventTypes : LookupBase
    {

        /// <summary>
        /// Gets or sets the cable id.
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        public bool Locked { get; set; }

    }
}
