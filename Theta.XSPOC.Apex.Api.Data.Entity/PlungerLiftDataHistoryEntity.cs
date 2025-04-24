using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The PlungerLiftDataHistory database table.
    /// </summary>
    [Table("tblPlungerLiftDataHistory")]
    public class PlungerLiftDataHistoryEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        [Column("CasingPressure")]
        public float? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        [Column("TubingPressure")]
        public float? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the differential pressure.
        /// </summary>
        [Column("DifferentialPressure")]
        public float? DifferentialPressure { get; set; }

        /// <summary>
        /// Gets or sets the line pressure.
        /// </summary>
        [Column("LinePressure")]
        public float? LinePressure { get; set; }

        /// <summary>
        /// Gets or sets the line temperature
        /// </summary>
        [Column("LineTemperature")]
        public float? LineTemperature { get; set; }

        /// <summary>
        /// Gets or sets the unit temperature.
        /// </summary>
        [Column("UnitTemperature")]
        public float? UnitTemperature { get; set; }

        /// <summary>
        /// Gets or sets the board temperature.
        /// </summary>
        [Column("BoardTemperature")]
        public float? BoardTemperature { get; set; }

        /// <summary>
        /// Gets or sets the battery.
        /// </summary>
        [Column("Battery")]
        public float? Battery { get; set; }

        /// <summary>
        /// Gets or sets the FCU differential pressure.
        /// </summary>
        [Column("FCU_DifferentialPressure")]
        public float? FCU_DifferentialPressure { get; set; }

        /// <summary>
        /// Gets or sets the FCU line pressure.
        /// </summary>
        [Column("FCU_LinePressure")]
        public float? FCU_LinePressure { get; set; }

        /// <summary>
        /// Gets or sets the FCU line temperature.
        /// </summary>
        [Column("FCU_LineTemperature")]
        public float? FCU_LineTemperature { get; set; }

        /// <summary>
        /// Gets or sets the FCU Rate.
        /// </summary>
        [Column("FCU_Rate")]
        public float? FCU_Rate { get; set; }

    }
}
