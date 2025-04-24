using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the tblUnitTypes.
    /// </summary>
    [Table("tblUnitTypes")]
    public class UnitTypeEntity
    {

        /// <summary>
        /// Get and set the UnitTypeID.
        /// </summary>
        [Key]
        [Column("UnitTypeID")]
        public int UnitTypeId { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}
