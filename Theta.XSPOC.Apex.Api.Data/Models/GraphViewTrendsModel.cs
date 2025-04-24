namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The Graph View Trends model.
    /// </summary>
    public class GraphViewTrendsModel
    {

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the source of the trend.
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// Gets or sets the poctype.
        /// </summary>
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets the address of the trend.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the trend name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the measure type.
        /// </summary>
        public int MeasureType { get; set; }

        /// <summary>
        /// Gets or sets the axis number.
        /// </summary>
        public int Axis { get; set; }

    }
}
