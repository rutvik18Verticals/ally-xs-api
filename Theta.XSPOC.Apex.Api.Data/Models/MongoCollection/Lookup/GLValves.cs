namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the GL valves MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class GLValves : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public float Diameter { get; set; }

        /// <summary>
        /// Gets or sets the bellows area.
        /// </summary>
        public float BellowsArea { get; set; }

        /// <summary>
        /// Gets or sets the port size.
        /// </summary>
        public float PortSize { get; set; }

        /// <summary>
        /// Gets or sets the port area.
        /// </summary>
        public float PortArea { get; set; }

        /// <summary>
        /// Gets or sets the port to bellows area ratio.
        /// </summary>
        public float PortToBellowsAreaRatio { get; set; }

        /// <summary>
        /// Gets or sets the production pressure effect factor.
        /// </summary>
        public float ProductionPressureEffectFactor { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer identifier.
        /// </summary>
        public int? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the one minus R value.
        /// </summary>
        public float? OneMinusR { get; set; }

    }

}
