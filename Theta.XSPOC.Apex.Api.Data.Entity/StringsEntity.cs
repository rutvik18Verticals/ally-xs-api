using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// THe WellStatistics table.
    /// </summary>
    [Table("tblStrings")]
    public partial class StringsEntity
    {

        /// <summary>
        /// Get and set the String Id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("StringID")]
        public int StringId { get; set; }

        /// <summary>
        /// Get and set the String Name.
        /// </summary>
        [MaxLength(15)]
        [Unicode(false)]
        public string StringName { get; set; }

        /// <summary>
        /// Get and set the ContactList Id.
        /// </summary>
        [Column("ContactListID")]
        public int? ContactListId { get; set; }

        /// <summary>
        /// Get and set the Responder ListId.
        /// </summary>
        [Column("ResponderListID")]
        public int? ResponderListId { get; set; }

    }
}
