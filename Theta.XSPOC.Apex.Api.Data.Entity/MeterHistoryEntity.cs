using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The MeterHistory database table.
    /// </summary>
    [Table("tblMeterHistory")]
    public class MeterHistoryEntity
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
        /// Gets or sets the user id.
        /// </summary>
        [Column("UserID")]
        [MaxLength(255)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the approved.
        /// </summary>
        [Column("Approved")]
        public bool Approved { get; set; }

        /// <summary>
        /// Gets or sets the record type.
        /// </summary>
        [Column("RecordType")]
        public int RecordType { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        [Column("CasingPressure")]
        public float CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        [Column("TubingPressure")]
        public float TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        [Column("Volume")]
        public float Volume { get; set; }

        /// <summary>
        /// Gets or sets the accum volume.
        /// </summary>
        [Column("AccumVolume")]
        public float AccumVolume { get; set; }

        /// <summary>
        /// Gets or sets the pressure
        /// </summary>
        [Column("Pressure")]
        public float Pressure { get; set; }

        /// <summary>
        /// Gets or sets the target pressure.
        /// </summary>
        [Column("TargetPressure")]
        public float TargetPressure { get; set; }

        /// <summary>
        /// Gets or sets the valve position.
        /// </summary>
        [Column("ValvePosition")]
        public float ValvePosition { get; set; }

        /// <summary>
        /// Gets or sets the instant rate.
        /// </summary>
        [Column("InstantRate")]
        public float InstantRate { get; set; }

        /// <summary>
        /// Gets or sets the target rate.
        /// </summary>
        [Column("TargetRate")]
        public float TargetRate { get; set; }

    }
}
