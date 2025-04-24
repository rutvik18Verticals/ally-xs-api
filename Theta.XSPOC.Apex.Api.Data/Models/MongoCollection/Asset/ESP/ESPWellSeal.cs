namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP
{
    /// <summary>
    /// A class representing the seal of an ESP asset.
    /// </summary>
    public class ESPWellSeal
    {

        /// <summary>
        /// Get or sets the order number of the seal.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the seal.
        /// </summary>
        public Lookup.Lookup Seal { get; set; }

        /// <summary>
        /// Gets or sets the seal manufacturer.
        /// </summary>
        public Lookup.Lookup SealManufacturer { get; set; }

    }
}
