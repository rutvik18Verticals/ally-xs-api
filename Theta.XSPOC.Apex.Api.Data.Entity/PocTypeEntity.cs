using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Table("tblPOCTypes")]
    public partial class POCTypeEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column("POCType")]
        public int PocType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("ProtocolID")]
        public int? ProtocolId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("MasterPOCType")]
        public int? MasterPocType { get; set; }
    }
}
