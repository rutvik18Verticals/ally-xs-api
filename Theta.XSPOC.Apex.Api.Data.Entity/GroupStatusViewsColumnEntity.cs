using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System GroupStatusViewsColumn table.
    /// </summary>
    [Table("tblGroupStatusViewsColumns")]
    public partial class GroupStatusViewsColumnEntity
    {

        /// <summary>
        /// Get and set the ViewId.
        /// </summary>
        [Column("ViewID")]
        public int ViewId { get; set; }

        /// <summary>
        /// Get and set the ColumnId.
        /// </summary>
        [Column("ColumnID")]
        public int ColumnId { get; set; }

        /// <summary>
        /// Get and set the Width.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Get and set the Position.
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Get and set the Orientation.
        /// </summary>
        public int Orientation { get; set; }

        /// <summary>
        /// Get and set the FormatId.
        /// </summary>
        [Column("FormatID")]
        public int FormatId { get; set; }

    }
}
