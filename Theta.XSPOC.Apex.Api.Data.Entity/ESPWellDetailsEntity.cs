using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The  ESP Well Details  table.
    /// </summary>
    [Table("tblESPWellDetails")]
    public class ESPWellDetailsEntity
    {

        /// <summary>
        /// Gets or sets the nodes id.
        /// </summary>
        [Column("NodeID")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the esp pump id.
        /// </summary>
        [Column("ESPPumpID")]
        public int? ESPPumpId { get; set; }

        /// <summary>
        /// Gets or sets the motor id.
        /// </summary>
        [Column("MotorID")]
        public int? MotorId { get; set; }

        /// <summary>
        /// Get and set the motor lead id.
        /// </summary>
        [Column("MotorLeadID")]
        public int? MotorLeadId { get; set; }

        /// <summary>
        /// Get and set the cable id.
        /// </summary>
        [Column("CableID")]
        public int? CableId { get; set; }

        /// <summary>
        /// Get and set the seal id.
        /// </summary>
        [Column("SealID")]
        public int? SealId { get; set; }

        /// <summary>
        /// Get and set the derating factor.
        /// </summary>
        [Column("DeratingFactor")]
        public int? DeratingFactor { get; set; }

        /// <summary>
        /// Get and set the number of stages.
        /// </summary>
        [Column("NumberOfStages")]
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Get and set the frequency.
        /// </summary>
        [Column("Frequency")]
        public int? Frequency { get; set; }

    }
}
