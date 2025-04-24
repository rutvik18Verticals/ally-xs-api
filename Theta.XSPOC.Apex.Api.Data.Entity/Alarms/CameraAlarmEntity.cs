using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Alarms
{
    /// <summary>
    /// The camera alarms table.
    /// </summary>
    [Table("tblCameraAlarms")]
    public class CameraAlarmEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the camera id.
        /// </summary>
        [Column("CameraID")]
        public int CameraId { get; set; }

        /// <summary>
        /// Gets or sets the alarm type.
        /// </summary>
        [Column("AlarmType")]
        public int AlarmType { get; set; }

        /// <summary>
        /// Gets or sets the is enabled flag.
        /// </summary>
        [Column("Enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the is email on alarm flag.
        /// </summary>
        [Column("EmailOnAlarm")]
        public bool IsEmailOnAlarm { get; set; }

        /// <summary>
        /// Gets or sets the contact list id.
        /// </summary>
        [Column("ContactListID")]
        public int? ContactListId { get; set; }

    }
}