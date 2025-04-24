using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.GasLift
{
    /// <summary>
    /// A class representing the valves of the gas lift well.
    /// </summary>
    public class WellValve
    {

        /// <summary>
        /// Gets or sets the gas lift valve.
        /// </summary>
        public Lookup.Lookup GasLiftValve { get; set; }

        /// <summary>
        /// Gets or sets the gas lift valve manufacturer.
        /// </summary>
        public Lookup.Lookup GasLiftValvleManufacturer { get; set; }

        /// <summary>
        /// Gets or sets the vertical depth.
        /// </summary>
        public double VerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the test rack opening pressure.
        /// </summary>
        public double? TestRackOpeningPressure { get; set; }

        /// <summary>
        /// Gets or sets the closing pressure at depth.
        /// </summary>
        public double? ClosingPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the measured depth.
        /// </summary>
        public double? MeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the opening pressure at depth.
        /// </summary>
        public double? OpeningPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the opening pressure at surface.
        /// </summary>
        public double? OpeningPressureAtSurface { get; set; }

        /// <summary>
        /// Gets or sets the closing pressure at surface.
        /// </summary>
        public double? ClosingPressureAtSurface { get; set; }

        /// <summary>
        /// Gets or sets the true vertical depth.
        /// </summary>
        public double? TrueVerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the legacy primary keys.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

    }
}
