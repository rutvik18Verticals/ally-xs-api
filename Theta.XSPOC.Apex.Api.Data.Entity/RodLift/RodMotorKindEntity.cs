using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.RodLift
{
    /// <summary>
    /// The motor kinds table for rod lift.
    /// </summary>
    [Table("tblMotorKinds")]
    public class RodMotorKindEntity
    {

        /// <summary>
        /// Gets or sets unique id.
        /// </summary>
        [Column("MotorKindID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets if the motor kind is ultra high speed. A value of null is false.
        /// </summary>
        [Column("UHS")]
        public bool? UltraHighSpeed { get; set; }

    }
}