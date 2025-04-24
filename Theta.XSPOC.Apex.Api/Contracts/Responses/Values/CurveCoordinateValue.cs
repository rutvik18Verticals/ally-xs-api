using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Describes a curve coordinate value that needs to be send out.
    /// </summary>
    public class CurveCoordinateValue
    {

        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The curve type id.
        /// </summary>
        public int CurveTypeId { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The list of coordinates
        /// </summary>
        public IList<CoordinatesValue> Coordinates { get; set; }

    }
}
