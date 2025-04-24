using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for  list of CardResponsevalues.
    /// </summary>
    public class CardResponseValuesOutput
    {

        /// <summary>
        /// The parameter id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of coordinates.
        /// </summary>
        public IList<CoordinatesData<float>> CoordinatesOutput { get; set; }

    }
}
