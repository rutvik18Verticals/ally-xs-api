namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the POC types MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class POCTypes : LookupBase
    {

        /// <summary>
        /// Gets or sets the POC type.
        /// </summary>
        public int POCType { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets if the alarm is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the Protocol Id.
        /// </summary>
        public int? ProtocolId { get; set; }

        /// <summary>
        /// Gets or sets if the alarm is master.
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// Gets or sets the Master POC type.
        /// </summary>
        public int? MasterPOCType { get; set; }

    }
}
