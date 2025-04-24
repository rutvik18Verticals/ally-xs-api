namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the control actions MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class ControlActions : LookupBase
    {

        /// <summary>
        /// Gets or sets the Control action Id.
        /// </summary>
        public int ControlActionId { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

    }
}
