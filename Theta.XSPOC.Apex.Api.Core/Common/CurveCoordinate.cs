namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a single coordinate and its identifier.
    /// </summary>
    public class CurveCoordinate : IdentityBase
    {

        /// <summary>
        /// Represents a single coordinate with a unique identifier
        /// </summary>
        public CurveCoordinate()
        {
        }

        /// <summary>
        /// Initializes a new GLWellCurve with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public CurveCoordinate(object id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets or sets the specific coordinate
        /// </summary>
        public Coordinate<double, double> Coordinate { get; set; }

    }
}
