using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The poc type actions database table.
    /// </summary>
    [Table("tblPOCTypeActions")]
    public class POCTypeActionsEntity
    {

        /// <summary>
        /// Gets or sets the poctype.
        /// </summary>
        [Column("POCType")]
        public int POCType { get; set; }

        /// <summary>
        /// Gets or sets the control action id.
        /// </summary>
        [Column("ControlActionID")]
        public int ControlActionId { get; set; }

    }
}
