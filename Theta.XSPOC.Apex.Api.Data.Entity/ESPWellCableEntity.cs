using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp pump table.
    /// </summary>
    [Table("tblESPWellCables")]
    public partial class ESPWellCableEntity
    {

        /// <summary>
        /// Gets or sets the esp well node id.
        /// </summary>
        [Column("NodeID")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the esp pump id. 
        /// </summary>
        [Column("ESPCableID")]
        public int ESPCableId { get; set; }

        /// <summary>
        /// Gets or sets the order number of the stages.
        /// </summary>
        public int OrderNumber { get; set; }

    }
}
