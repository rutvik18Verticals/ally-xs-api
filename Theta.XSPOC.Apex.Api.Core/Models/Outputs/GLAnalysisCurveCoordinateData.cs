using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for GLAnalysisCurveCoordinateData data
    /// </summary>
    public class GLAnalysisCurveCoordinateData
    {

        /// <summary>
        /// The Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Gets or sets curve type id.
        /// </summary>
        public int CurveTypeId { get; set; }

        /// <summary>
        /// The Gets or sets display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The Gets or sets list of coordinates.
        /// </summary>
        public IList<CoordinatesData<float>> CoordinatesOutput { get; set; }

    }

}
