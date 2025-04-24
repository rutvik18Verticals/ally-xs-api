using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the user defaults table.
    /// </summary>
    [Table("tblDefaults")]
    public class UserDefaultEntity
    {

        /// <summary>
        /// Gets and sets the defaults group.
        /// </summary>
        [Column("DefaultsGroup", TypeName = "varchar")]
        [StringLength(50)]
        public string DefaultsGroup { get; set; }

        /// <summary>
        /// Gets and sets the Name.
        /// </summary>
        [Column("Property", TypeName = "varchar")]
        [StringLength(50)]
        public string Property { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        [Column("UID", TypeName = "varchar")]
        [StringLength(50)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets and sets the value.
        /// </summary>
        [Column("Value", TypeName = "varchar")]
        [StringLength(250)]
        public string Value { get; set; }

    }
}
