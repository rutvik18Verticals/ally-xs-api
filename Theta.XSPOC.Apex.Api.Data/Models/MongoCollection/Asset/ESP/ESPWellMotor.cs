namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP
{
    /// <summary>
    /// A class representing the motor of an ESP asset.
    /// </summary>
    public class ESPWellMotor
    {

        /// <summary>
        /// Gets or sets the order number of the motor.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the motor.
        /// </summary>
        public Lookup.Lookup Motor { get; set; }

        /// <summary>
        /// Gets or sets the motor manufacturer.
        /// </summary>
        public Lookup.Lookup Manufacturer { get; set; }

    }
}
