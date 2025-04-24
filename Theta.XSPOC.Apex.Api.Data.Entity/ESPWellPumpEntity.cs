using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp pump table.
    /// </summary>
    [Table("tblESPWellPumps")]
    public partial class ESPWellPumpEntity
    {

        /// <summary>
        /// Gets or sets the esp well id.
        /// </summary>
        [Column("ESPWellID")]
        [MaxLength(50)]
        public string ESPWellId { get; set; }

        /// <summary>
        /// Gets or sets the esp pump id. 
        /// </summary>
        [Column("ESPPumpID")]
        public int ESPPumpId { get; set; }

        /// <summary>
        /// Gets or sets the number of stages in the well.
        /// </summary>
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Gets or sets the order number of the stages.
        /// </summary>
        public int OrderNumber { get; set; }

    }
}
