using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The ESP Cables table.
    /// </summary>
    [Table("tblESPCables")]
    public class ESPCablesEntity
    {

        /// <summary>
        /// Gets or sets the cable id.
        /// </summary>
        [Key]
        [Column("CableID")]
        public int CableId { get; set; }

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
        /// Get and set the cable type.
        /// </summary>
        [Column("CableType")]
        public string CableType { get; set; }

        /// <summary>
        /// Get and set the cable description.
        /// </summary>
        [Column("CableDescription")]
        public string CableDescription { get; set; }

    }
}
