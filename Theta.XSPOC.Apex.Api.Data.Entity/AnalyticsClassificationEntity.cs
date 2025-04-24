using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The analytics classifications table.
    /// </summary>
    [Table("tblAnalyticsClassifications")]
    public class AnalyticsClassificationEntity
    {

        /// <summary>
        /// Gets or sets the id. 
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date.
        /// </summary>
        [Column("EndDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the classification type id.
        /// </summary>
        [Column("ClassificationTypeID")]
        public int ClassificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the second classification type id.
        /// </summary>
        [Column("SecondClassificationTypeID")]
        public int SecondClassificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the third classification type id.
        /// </summary>
        [Column("ThirdClassificationTypeId")]
        public int ThirdClassificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the value that depicts acknowledged or not.
        /// </summary>
        [Column("Acknowledged")]
        public bool Acknowledged { get; set; }

    }
}
