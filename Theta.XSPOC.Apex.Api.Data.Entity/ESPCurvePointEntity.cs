using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp curve points database table.
    /// </summary>
    [Table("tblESPCurvePoints")]
    public partial class ESPCurvePointEntity
    {

        /// <summary>
        /// The esp pump id.
        /// </summary>
        [Column("ESPPumpID")]
        public int ESPPumpID { get; set; }

        /// <summary>
        /// Gets or sets the flow rate.
        /// </summary>
        public double FlowRate { get; set; }

        /// <summary>
        /// Gets or sets the head feet per stage.
        /// </summary>
        public double HeadFeetPerStage { get; set; }

        /// <summary>
        /// Gets or sets the power in hp.
        /// </summary>
        [Column("PowerInHP")]
        public double PowerInHP { get; set; }

        /// <summary>
        /// Gets or sets the efficiency.
        /// </summary>
        public double? Efficiency { get; set; }

    }
}
