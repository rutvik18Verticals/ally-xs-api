using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The states table.
    /// </summary>
    [Table("tblStates")]
    public class StatesEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("StateId")]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Column("Value")]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Column("Text")]
        [MaxLength(50)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the back color.
        /// </summary>
        [Column("BackColor")]
        public int? BackColor { get; set; }

        /// <summary>
        /// Gets or sets the alarm priority.
        /// </summary>
        [Column("AlarmPriority")]
        public int? AlarmPriority { get; set; }

        /// <summary>
        /// Gets or sets the locked flag.
        /// </summary>
        [Column("Locked")]
        public bool? Locked { get; set; }

        /// <summary>
        /// Gets or sets the fore color.
        /// </summary>
        [Column("ForeColor")]
        public int? ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}
