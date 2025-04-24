using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System FacilityTags table.
    /// </summary>
    [Table("tblFacilityTagGroups")]
    public class FacilityTagGroupsEntity
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Get and set the Address.
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; }

    }
}
