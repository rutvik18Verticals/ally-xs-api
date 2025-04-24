namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the status registers MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class StatusRegisters : LookupBase
    {

        /// <summary>
        /// Gets or sets the POC type.
        /// </summary>
        public int POCType { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the Bit.
        /// </summary>
        public int Bit { get; set; }

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets the Format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets if the alarm configuration is locked.
        /// </summary>
        public bool? IsLocked { get; set; }

    }
}
