namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset
{
    /// <summary>
    /// A class with properties for the perforation of an asset.
    /// </summary>
    public partial class Perforation
    {

        /// <summary>
        /// Gets and sets the depth of the perforation.
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// Gets and sets the interval of the perforation.
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// Gets and sets the diameter of the perforation.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets and sets the holes per foot of the perforation.
        /// </summary>
        public int? HolesPerFoot { get; set; }

    }
}
