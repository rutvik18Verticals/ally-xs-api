using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The rod pumping unit table.
    /// </summary>
    [Table("tblPumpingUnits")]
    public class PumpingUnitsEntity
    {

        /// <summary>
        /// Gets or sets the unique numeric identifier.
        /// </summary>
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unit id. This is maintained for backward compatibility, the Id should be considered the
        /// primary key.
        /// </summary>
        [Column("UnitID", TypeName = "nvarchar")]
        [MaxLength(255)]
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id. 
        /// </summary>
        [Column("ManufID", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the API designation. 
        /// </summary>
        [Column("APIDesignation", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit name. 
        /// </summary>
        [Column("UnitName", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string UnitName { get; set; }

        /// <summary>
        /// Gets or sets the OtherInfo value. 
        /// </summary>
        [Column("OtherInfo", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the number of crank holes
        /// </summary>
        [Column("CrankHoles")]
        public int? CrankHoles { get; set; }

        /// <summary>
        /// Gets or sets the  stroke 1 value.
        /// </summary>
        [Column("Stroke1")]
        public float? Stroke1 { get; set; }

        /// <summary>
        /// Gets or sets the  stroke 2 value.
        /// </summary>
        [Column("Stroke2")]
        public float? Stroke2 { get; set; }

        /// <summary>
        /// Gets or sets the  stroke 3 value.
        /// </summary>
        [Column("Stroke3")]
        public float? Stroke3 { get; set; }

        /// <summary>
        /// Gets or sets the stroke 4 value. 
        /// </summary>
        [Column("Stroke4")]
        public float? Stroke4 { get; set; }

        /// <summary>
        /// Gets or sets the stroke 5 value.
        /// </summary>
        [Column("Stroke5")]
        public float? Stroke5 { get; set; }

        /// <summary>
        /// Gets or sets the structural rating.
        /// </summary>
        [Column("StructRating")]
        public float? StructuralRating { get; set; }

        /// <summary>
        /// Gets or sets the gearbox rating.
        /// </summary>
        [Column("GearboxRating")]
        public float? GearboxRating { get; set; }

        /// <summary>
        /// Gets or sets the maximum stroke.
        /// </summary>
        [Column("MaxStroke")]
        public float? MaxStroke { get; set; }

        /// <summary>
        /// Gets or sets the WV_Type.
        /// </summary>
        [Column("WV_Typ", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string WV_Type { get; set; }

        /// <summary>
        /// Gets or sets the WV_Make.
        /// </summary>
        [Column("WV_Make", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string WV_Make { get; set; }

        /// <summary>
        /// Gets or sets the WV_Model.
        /// </summary>
        [Column("WV_Model", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string WV_Model { get; set; }

        /// <summary>
        /// Gets or sets the WV_OtherInfo.
        /// </summary>
        [Column("WV_OtherInfo", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string WV_OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the Dimensions. 
        /// </summary>
        [Column("Dimensions", TypeName = "nvarchar")]
        [MaxLength(80)]
        public string Dimensions { get; set; }

    }
}
