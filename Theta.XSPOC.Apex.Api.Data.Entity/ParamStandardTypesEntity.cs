using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System ParamStandardType table.
    /// </summary>
    [Table("tblParamStandardTypes")]
    [Index("Description", Name = "UC_tblParamStandardTypes_Description", IsUnique = true)]
    public class ParamStandardTypesEntity
    {

        /// <summary>
        /// Get and set the Param Standard Type.
        /// </summary>
        [Key]
        public int ParamStandardType { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseId { get; set; }

        /// <summary>
        /// Get and set the Unit Type Id.
        /// </summary>
        [Column("UnitTypeID")]
        public int UnitTypeId { get; set; }

    }
}
