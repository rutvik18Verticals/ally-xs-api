namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// Class for defining the layout coordinates of a widget in a dashboard.
    /// </summary>
    public class Lg
    {
        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        public float? X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the widget.
        /// </summary>
        public float W { get; set; }

        /// <summary>
        /// Gets or sets the height of the widget.
        /// </summary>
        public float H { get; set; }

        /// <summary>
        /// Gets or sets the width of the widget.
        /// </summary>
        public float MinH { get; set; }

        /// <summary>
        /// Gets or sets the height of the widget.
        /// </summary>
        public float MinW { get; set; }
    }
}