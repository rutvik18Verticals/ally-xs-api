using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Operational Score Entity table.
    /// </summary>
    [Table("tblOperationalScore")]
    public class OperationalScoreEntity
    {

        /// <summary>
        /// Gets or sets id. 
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Node ID.
        /// </summary>
        [Column("NodeID")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets Score Date Time.
        /// </summary>
        [Column("ScoreDateTime")]
        public DateTime ScoreDateTime { get; set; }

        /// <summary>
        /// Gets or sets Operational Score.
        /// </summary>
        [Column("OperationalScore")]
        public float? OperationalScore { get; set; }

    }
}
