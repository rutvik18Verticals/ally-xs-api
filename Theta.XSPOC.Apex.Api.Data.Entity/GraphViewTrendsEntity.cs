using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Graph View Trends database table.
    /// </summary>
    [Table("tblGraphViewTrends")]
    public class GraphViewTrendsEntity
    {

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        [Key]
        [Column("ViewID")]
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the source of the trend.
        /// </summary>
        [Column("Source")]
        public int Source { get; set; }

        /// <summary>
        /// Gets or sets the poctype.
        /// </summary>
        [Column("POCType")]
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
        /// Gets or sets the color of the axis.
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        /// Gets or sets the measure type.
        /// </summary>
        public int MeasureType { get; set; }

        /// <summary>
        /// Gets or sets the axis number.
        /// </summary>
        public int Axis { get; set; }

        /// <summary>
        /// Gets or sets the aggregation type.
        /// </summary>
        public short AggregateType { get; set; }

    }
}
