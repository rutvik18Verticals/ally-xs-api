using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for curve coordinate data output with list of coordinate data.
    /// </summary>
    public class CurveCoordinateValues
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the curve type id.
        /// </summary>
        public int CurveTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the list of coordinates.
        /// </summary>
        public IList<CoordinatesData<double>> Coordinates { get; set; }

    }
}
