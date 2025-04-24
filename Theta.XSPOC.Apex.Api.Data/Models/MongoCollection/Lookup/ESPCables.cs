namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the esp cables MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class ESPCables : LookupBase
    {

        /// <summary>
        /// Gets or sets the cable id.
        /// </summary>
        public int CableId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturerI id.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public float Diameter { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the cable type.
        /// </summary>
        public string CableType { get; set; }

        /// <summary>
        /// Gets or sets the cable description.
        /// </summary>
        public string CableDescription { get; set; }

    }
}
