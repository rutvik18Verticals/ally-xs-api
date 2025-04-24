using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Alarms
{
    /// <summary>
    /// The alarm events table.
    /// </summary>
    [Table("tblAlarmEvents")]
    public class AlarmEventEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the alarm type.
        /// </summary>
        [Column("AlarmType")]
        public int AlarmType { get; set; }

        /// <summary>
        /// Gets or sets the alarm id.
        /// </summary>
        [Column("AlarmID")]

        public int AlarmId { get; set; }

        /// <summary>
        /// Gets or sets the event date time.
        /// </summary>
        [Column("EventDateTime")]
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        [Column("EventType")]
        public int EventType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Column("Value")]
        public float? Value { get; set; }

        /// <summary>
        /// Gets or sets the picture id.
        /// </summary>
        [Column("PictureID")]
        public int? PictureId { get; set; }

        /// <summary>
        /// Gets or sets the acknowledged date time.
        /// </summary>
        [Column("AcknowledgedDateTime")]
        public DateTime? AcknowledgedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the acknowledged user name.
        /// </summary>
        [Column("AcknowledgedUserName", TypeName = "nvarchar")]
        [MaxLength(100)]
        public string AcknowledgedUserName { get; set; }

    }
}