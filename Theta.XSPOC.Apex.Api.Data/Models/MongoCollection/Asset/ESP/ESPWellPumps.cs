namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP
{
    /// <summary>
    /// A class representing the pumps of an ESP asset.
    /// </summary>
    public class ESPWellPumps
    {

        /// <summary>
        /// Gets or sets the order number of the pump.
        /// </summary>
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Gets or sets the order number of the pump.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the ESP pump.
        /// </summary>
        public Lookup.Lookup Pump { get; set; }

        /// <summary>
        /// Gets or sets the pump manufacturer.
        /// </summary>
        public Lookup.Lookup Manufacturer { get; set; }

    }
}
