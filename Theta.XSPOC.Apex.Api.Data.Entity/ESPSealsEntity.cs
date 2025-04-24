using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The  ESP Well Details  table.
    /// </summary>
    [Table("tblESPSeals")]
    public class ESPSealsEntity
    {

        /// <summary>
        /// Gets or sets the seal id.
        /// </summary>
        [Key]
        [Column("SealID")]
        public int SealId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        [Column("ManufacturerID")]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Column("Model")]
        public string Model { get; set; }

        /// <summary>
        /// Get and set the description.
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the diameter.
        /// </summary>
        [Column("Diameter")]
        public float Diameter { get; set; }

        /// <summary>
        /// Get and set the flag locked.
        /// </summary>
        [Column("Locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// Get and set the series.
        /// </summary>
        [Column("Series")]
        public string Series { get; set; }

        /// <summary>
        /// Get and set the seal model.
        /// </summary>
        [Column("SealModel")]
        public string SealModel { get; set; }

    }
}
