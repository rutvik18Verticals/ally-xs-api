using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// The Card Coordinate response value details.
    /// </summary>
    public class CardResponseValues
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
        /// The parameter Coordinates.
        /// </summary>
        public IList<Coordinates> Coordinates { get; set; }

    }

    /// <summary>
    /// The coordinates.
    /// </summary>
    public class Coordinates
    {

        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        public float Y { get; set; }

    }

}
