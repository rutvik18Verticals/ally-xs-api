using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GroupDataHistory table.
    /// </summary>
    [Table("tblGroupDataHistory")]
    public class GroupDataHistoryEntity
    {

        /// <summary>
        /// Get and set the ID.
        /// </summary>
        [Column("ID")]
        [MaxLength(8)]
        public int ID { get; set; }

        /// <summary>
        /// Get and set the GroupParameterID.
        /// </summary>
        [Column("GroupParameterID")]
        [MaxLength(4)]
        public int GroupParameterID { get; set; }

        /// <summary>
        /// Get and set the Name.
        /// </summary>
        [Column("Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Get and set the Date.
        /// </summary>
        [Column("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Get and set the value.
        /// </summary>
        [Column("value")]
        [MaxLength(8)]
        public decimal? Value { get; set; }

    }
}
