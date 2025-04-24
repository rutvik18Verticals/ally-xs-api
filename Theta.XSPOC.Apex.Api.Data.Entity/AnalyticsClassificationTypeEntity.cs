using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The analytics classifications type table.
    /// </summary>
    [Table("tblAnalyticsClassificationTypes")]
    public class AnalyticsClassificationTypeEntity
    {

        /// <summary>
        /// Gets or sets the id. 
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseID { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        [Column("Color")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [Column("Priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets boolean value for the field state.
        /// </summary>
        [Column("IsState")]
        public bool IsState { get; set; }

        /// <summary>
        /// Gets or sets the value for enabled.
        /// </summary>
        [Column("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value that depicts Locked or not.
        /// </summary>
        [Column("Locked")]
        public bool Locked { get; set; }

    }
}
