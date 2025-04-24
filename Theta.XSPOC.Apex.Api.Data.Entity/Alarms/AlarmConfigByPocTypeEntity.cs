using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Alarms
{
    /// <summary>
    /// The alarm config by poc type table.
    /// </summary>
    [Table("tblAlarmConfigByPOCType")]
    public class AlarmConfigByPocTypeEntity
    {

        /// <summary>
        /// Gets or sets the alarm action.
        /// </summary>
        [Column("AlarmAction")]
        public int AlarmAction { get; set; }

        /// <summary>
        /// Gets or sets the bit.
        /// </summary>
        [Column("Bit")]
        public short Bit { get; set; }

        /// <summary>
        /// Gets or sets call out enabled flag.
        /// </summary>
        [Column("CalloutEnabled")]
        public bool CallOutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Column("Description", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the enabled flag.
        /// </summary>
        [Column("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the high limit.
        /// </summary>
        [Column("HiLimit")]
        public float HiLimit { get; set; }

        /// <summary>
        /// Gets or sets the locked flag.
        /// </summary>
        [Column("Locked")]
        public bool? Locked { get; set; }

        /// <summary>
        /// Gets or sets the low limit.
        /// </summary>
        [Column("LoLimit")]
        public float LoLimit { get; set; }

        /// <summary>
        /// Gets or sets the normal state flag.
        /// </summary>
        [Column("NormalState")]
        public bool NormalState { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        [Column("POCType")]
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [Column("Priority")]
        public short Priority { get; set; }

        /// <summary>
        /// Gets or sets register.
        /// </summary>
        [Column("Register")]
        public int Register { get; set; }

    }
}