using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the company table.
    /// </summary>
    [Table("tblCompany")]
    public class CompanyEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        [MaxLength(250)]
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Customer GUID.
        /// </summary>
        [Column("CustomerGUID")]
        public Guid CustomerGUID { get; set; }

    }
}
