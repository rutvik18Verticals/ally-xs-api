using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Reprasents the tblSurveyDate.
    /// </summary>
    [Keyless]
    [Table("tblSurveyData")]
    public class SurveyDataEntity
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the NodeId.
        /// </summary>
        [Required]
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the SurveyDate.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime SurveyDate { get; set; }

    }
}
