namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset
{
    /// <summary>
    /// A class with properties for the tubing of an asset.
    /// </summary>
    public class Tubing
    {

        /// <summary>
        /// Gets or sets the order number of the tubing.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the length of the casing.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the size of the tubing. // todo Should this be a object id to the lookup collection?
        /// </summary>
        public Lookup.Lookup Size { get; set; }

    }
}
