using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The MeterColumns database table
    /// </summary>
    [Table("tblMeterColumns")]
    public class MeterColumnEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the meter type id.
        /// </summary>
        [Column("MeterTypeID")]
        public int MeterTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alias name.
        /// </summary>
        [MaxLength(100)]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int? Width { get; set; }

    }
}
