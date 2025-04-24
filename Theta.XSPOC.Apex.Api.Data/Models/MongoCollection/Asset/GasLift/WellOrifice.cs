namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.GasLift
{
    /// <summary>
    /// A class representing the orifice of the gas lift well.
    /// </summary>
    public class WellOrifice
    {

        /// <summary>
        /// Gets or sets the gas lift manufacturer.
        /// </summary>
        public Lookup.Lookup GasLiftManufacturer { get; set; }

        /// <summary>
        /// Gets or sets the measured depth.
        /// </summary>
        public double? MeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the vertical depth.
        /// </summary>
        public double VerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the orifice port size in inches.
        /// </summary>
        public double PortSize { get; set; }

        /// <summary>
        /// Gets or sets the true vertical depth.
        /// </summary>
        public double? TrueVerticalDepth { get; set; }

    }
}
