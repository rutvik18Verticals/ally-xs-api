namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset
{
    /// <summary>
    /// A class representing a deviation of a well.
    /// </summary>
    public class Deviation
    {

        /// <summary>
        /// Gets or sets the measured depth.
        /// </summary>
        public int MeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the inclination.
        /// </summary>
        public double? Inclination { get; set; }

        /// <summary>
        /// Gets or sets the azimuth.
        /// </summary>
        public double? Azimuth { get; set; }

    }
}
