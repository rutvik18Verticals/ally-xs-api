using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The  ESP Well Details  table.
    /// </summary>
    [Table("tblESPMotorLeads")]
    public class ESPMotorLeadsEntity
    {

        /// <summary>
        /// Gets or sets the motor lead id.
        /// </summary>
        [Key]
        [Column("MotorLeadID")]
        public int MotorLeadId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        [Column("ManufacturerID")]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Column("Model")]
        public string Model { get; set; }

        /// <summary>
        /// Get and set the description.
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the flag locked.
        /// </summary>
        [Column("Locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// Get and set the series.
        /// </summary>
        [Column("Series")]
        public string Series { get; set; }

        /// <summary>
        /// Get and set the motor lead type.
        /// </summary>
        [Column("MotorLeadType")]
        public string MotorLeadType { get; set; }

        /// <summary>
        /// Get and set the motor lead description.
        /// </summary>
        [Column("MotorLeadDescription")]
        public string MotorLeadDescription { get; set; }

    }
}
