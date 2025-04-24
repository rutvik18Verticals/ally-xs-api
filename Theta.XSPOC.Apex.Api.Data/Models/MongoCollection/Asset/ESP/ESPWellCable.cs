namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP
{
    /// <summary>
    /// A class representing the cables of an ESP asset.
    /// </summary>
    public class ESPWellCable
    {

        /// <summary>
        /// Gets or sets the order number of the cable.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the ESP cable.
        /// </summary>
        public Lookup.Lookup ESPCable { get; set; }

        /// <summary>
        /// Gets or sets the ESP cable manufacturer.
        /// </summary>
        public Lookup.Lookup ESPManufacturer { get; set; }

    }
}
