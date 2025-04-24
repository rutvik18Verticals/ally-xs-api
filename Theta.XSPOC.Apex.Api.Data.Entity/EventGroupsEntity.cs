using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The event groups table.
    /// </summary>
    [Table("tblEventGroups")]
    public class EventGroupsEntity
    {

        /// <summary>
        /// Gets or sets the event type id.
        /// </summary>
        [Column("EventTypeID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the allow user creation flag.
        /// </summary>
        [Column("AllowUserCreation")]
        public bool AllowUserCreation { get; set; }

    }
}
