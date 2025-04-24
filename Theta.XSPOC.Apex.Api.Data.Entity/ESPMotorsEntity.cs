using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The ESP Motor table.
    /// </summary>
    [Table("tblESPMotors")]
    public class ESPMotorsEntity
    {

        /// <summary>
        /// Gets or sets the motor id.
        /// </summary>
        [Key]
        [Column("MotorId")]
        public int MotorId { get; set; }

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
        /// Get and set the diameter.
        /// </summary>
        [Column("Diameter")]
        public float Diameter { get; set; }

        /// <summary>
        /// Get and set the hp.
        /// </summary>
        [Column("HP")]
        public float HP { get; set; }

        /// <summary>
        /// Get and set the volts.
        /// </summary>
        [Column("Volts")]
        public float Volts { get; set; }

        /// <summary>
        /// Get and set the amps.
        /// </summary>
        [Column("Amps")]
        public float Amps { get; set; }

        /// <summary>
        /// Get and set the flag locked.
        /// </summary>
        [Column("Locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// Get and set the rated temperature.
        /// </summary>
        [Column("RatedTemperature")]
        public float? RatedTemperature { get; set; }

        /// <summary>
        /// Get and set the series.
        /// </summary>
        [Column("Series")]
        public string Series { get; set; }

        /// <summary>
        /// Get and set the seal model.
        /// </summary>
        [Column("MotorModel")]
        public string MotorModel { get; set; }

    }
}