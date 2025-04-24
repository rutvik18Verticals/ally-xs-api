namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP
{
    /// <summary>
    /// A class representing the motor lead of an ESP asset.
    /// </summary>
    public class ESPWellMotorLead
    {

        /// <summary>
        /// Gets or sets the order number of the motor.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the motor lead.
        /// </summary>
        public Lookup.Lookup MotorLead { get; set; }

        /// <summary>
        /// Gets or sets the lead manufacturer.
        /// </summary>
        public Lookup.Lookup Manufacturer { get; set; }

    }
}
