using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represent the class for GLAnalysisCurveCoordinateResponseValues data values.
    /// </summary>
    public class GLAnalysisCurveCoordinateResponseValues
    {

        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The curve type id.
        /// </summary>
        public int CurveTypeId { get; set; }

        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The List of Coordinates.
        /// </summary>
        public IList<Coordinates> Coordinates { get; set; }

    }

}
