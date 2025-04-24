using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The locale phrase table.
    /// </summary>
    [Table("tblLocalePhrases")]
    public class LocalePhraseEntity
    {

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the English version of the phrase.
        /// </summary>
        [Column("1033", TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string English { get; set; }

        /// <summary>
        /// Gets or sets the German version of the phrase.
        /// </summary>
        [Column("1031", TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string German { get; set; }

        /// <summary>
        /// Gets or sets the Spanish version of the phrase.
        /// </summary>
        [Column("1034", TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string Spanish { get; set; }

        /// <summary>
        /// Gets or sets the Russian version of the phrase.
        /// </summary>
        [Column("1049", TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string Russian { get; set; }

        /// <summary>
        /// Gets or sets the Chinese version of the phrase.
        /// </summary>
        [Column("2052", TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string Chinese { get; set; }

        /// <summary>
        /// Gets or sets the French version of the phrase.
        /// </summary>
        [Column("1036", TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string French { get; set; }

    }
}
