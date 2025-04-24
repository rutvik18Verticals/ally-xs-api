using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.RodLift
{
    /// <summary>
    /// The motor settings table for rod lift.
    /// </summary>
    [Table("tblMotorSettings")]
    public class RodMotorSettingEntity
    {

        /// <summary>
        /// Gets or sets unique id.
        /// </summary>
        [Column("MotorSettingID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the RPM at full load.
        /// </summary>
        [Column("FullLoadRPM")]
        public float? FullLoadRPM { get; set; }

        /// <summary>
        /// Gets or sets the maximum load.
        /// </summary>
        [Column("MaxLoad")]
        public float? MaximumLoad { get; set; }

        /// <summary>
        /// Gets or sets the maximum SPV ( Specific Volume? ) .
        /// </summary>
        [Column("MaxSPV")]
        public float? MaximumSPV { get; set; }

        /// <summary>
        /// Gets or sets the rated horse power.
        /// </summary>
        [Column("RatedHP")]
        public float? RatedHP { get; set; }

        /// <summary>
        /// Gets or sets the motor size id.
        /// </summary>
        [Column("MotorSizeID")]
        public int? MotorSizeId { get; set; }

    }
}