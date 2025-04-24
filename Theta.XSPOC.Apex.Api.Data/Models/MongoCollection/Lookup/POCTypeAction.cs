namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines thePOC Type action to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class POCTypeAction : LookupBase
    {

        /// <summary>
        /// Gets or sets the POCType.
        /// </summary>
        public int POCType { get; set; }

        /// <summary>
        /// Gets or sets the ControlAction Id.
        /// </summary>
        public int ControlActionId { get; set; }

    }
}
