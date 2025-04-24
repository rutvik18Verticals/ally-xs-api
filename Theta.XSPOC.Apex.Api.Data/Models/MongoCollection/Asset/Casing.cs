namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset
{
    /// <summary>
    /// A class with properties for the casing of an asset.
    /// </summary>
    public class Casing
    {

        /// <summary>
        /// Gets or sets the order number of the casing.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the casing.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the length of the casing.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the size of the casing. // todo Should this be a object id to the lookup collection?
        /// </summary>
        public Lookup.Lookup Size { get; set; }

    }
}
