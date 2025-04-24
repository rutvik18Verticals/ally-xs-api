using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System Parameter table.
    /// </summary>
    [Table("tblConditionalFormats")]
    public partial class ConditionalFormatEntity
    {

        /// <summary>
        /// Get and set the Id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Get and set the ColumnId.
        /// </summary>
        [Column("ColumnID")]
        public int ColumnId { get; set; }

        /// <summary>
        /// Get and set the OperatorId.
        /// </summary>
        [Column("OperatorID")]
        public int OperatorId { get; set; }

        /// <summary>
        /// Get and set the Value.
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        /// Get and set the MinValue.
        /// </summary>
        public float? MinValue { get; set; }

        /// <summary>
        /// Get and set the MaxValue.
        /// </summary>
        public float? MaxValue { get; set; }

        /// <summary>
        /// Get and set the BackColor.
        /// </summary>
        public int BackColor { get; set; }

        /// <summary>
        /// Get and set the ForeColor.
        /// </summary>
        public int ForeColor { get; set; }

        /// <summary>
        /// Get and set the StringValue.
        /// </summary>
        public string StringValue { get; set; }

    }
}
