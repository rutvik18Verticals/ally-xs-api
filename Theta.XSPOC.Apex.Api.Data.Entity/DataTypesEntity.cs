using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System DataTypes table.
    /// </summary>
    [Table("tblDataTypes")]
    public class DataTypesEntity
    {

        /// <summary>
        /// Get and set the DataType.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte DataType { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the Comment.
        /// </summary>
        [MaxLength(50)]
        public string Comment { get; set; }

    }
}
