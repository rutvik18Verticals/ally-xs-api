using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The table containing custom rod pumping units.
    /// </summary>
    [Table("tblPUCustom")]
    public class CustomPumpingUnitEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("UnitID", TypeName = "nvarchar")]
        [MaxLength(6)]
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        [Column("UnitType")]
        public short Type { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        [Column("Manufacturer", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the API designation.
        /// </summary>
        [Column("APIDesignation", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit name.
        /// </summary>
        [Column("UnitName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the other info.
        /// </summary>
        [Column("OtherInfo", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the number of crank holes.
        /// </summary>
        [Column("CrankHoles")]
        public short CrankHoles { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 1.
        /// </summary>
        [Column("Stroke1")]
        public float? Stroke1 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 2.
        /// </summary>
        [Column("Stroke2")]
        public float? Stroke2 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 3.
        /// </summary>
        [Column("Stroke3")]
        public float? Stroke3 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 4.
        /// </summary>
        [Column("Stroke4")]
        public float? Stroke4 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 5.
        /// </summary>
        [Column("Stroke5")]
        public float? Stroke5 { get; set; }

        /// <summary>
        /// Gets or sets the R1 dimension.
        /// </summary>
        [Column("R1")]
        public float R1 { get; set; }

        /// <summary>
        /// Gets or sets the R2 dimension.
        /// </summary>
        [Column("R2")]
        public float R2 { get; set; }

        /// <summary>
        /// Gets or sets the R3 dimension.
        /// </summary>
        [Column("R3")]
        public float R3 { get; set; }

        /// <summary>
        /// Gets or sets the R4 dimension.
        /// </summary>
        [Column("R4")]
        public float R4 { get; set; }

        /// <summary>
        /// Gets or sets the R5 dimension.
        /// </summary>
        [Column("R5")]
        public float R5 { get; set; }

        /// <summary>
        /// Gets or sets the A dimension.
        /// </summary>
        [Column("A")]
        public float A { get; set; }

        /// <summary>
        /// Gets or sets the C dimension.
        /// </summary>
        [Column("C")]
        public float C { get; set; }

        /// <summary>
        /// Gets or sets the I dimension.
        /// </summary>
        [Column("I")]
        public float I { get; set; }

        /// <summary>
        /// Gets or sets the K dimension.
        /// </summary>
        [Column("K")]
        public float K { get; set; }

        /// <summary>
        /// Gets or sets the P dimension.
        /// </summary>
        [Column("P")]
        public float P { get; set; }

        /// <summary>
        /// Gets or sets the M dimension.
        /// </summary>
        [Column("M")]
        public float M { get; set; }

        /// <summary>
        /// Gets or sets the S dimension.
        /// </summary>
        [Column("S")]
        public float S { get; set; }

        /// <summary>
        /// Gets or sets the V0 dimension.
        /// </summary>
        [Column("V0")]
        public float V0 { get; set; }

        /// <summary>
        /// Gets or sets the drum diameter ratio.
        /// </summary>
        [Column("DrumDiamRatio")]
        public float DrumDiameterRatio { get; set; }

        /// <summary>
        /// Gets or sets the sprocket diameter.
        /// </summary>
        [Column("SprocketDiameter")]
        public float SprocketDiameter { get; set; }

        /// <summary>
        /// Gets or sets the sprocket distance.
        /// </summary>
        [Column("SprocketDistance")]
        public float SprocketDistance { get; set; }

        /// <summary>
        /// Gets or sets the unbalance.
        /// </summary>
        [Column("Unbalance")]
        public float Unbalance { get; set; }

        /// <summary>
        /// Gets or sets the outer diameter.
        /// </summary>
        [Column("CrankOffset")]
        public float CrankOffset { get; set; }

        /// <summary>
        /// Gets or sets the structural rating.
        /// </summary>
        [Column("StructRating")]
        public float StructuralRating { get; set; }

        /// <summary>
        /// Gets or sets the gearbox rating.
        /// </summary>
        [Column("GearboxRating")]
        public float GearboxRating { get; set; }

        /// <summary>
        /// Gets or sets the maximum stroke.
        /// </summary>
        [Column("MaxStroke")]
        public float? MaximumStroke { get; set; }

        /// <summary>
        /// Gets or sets the articulating inertia.
        /// </summary>
        [Column("ArtInertia")]
        public float? ArticulatingInertia { get; set; }

        /// <summary>
        /// Gets or sets the Pitman arm length.
        /// </summary>
        [Column("PitmanArmLength")]
        public short PitmanArmLength { get; set; }

        /// <summary>
        /// Gets or sets the required rotation.
        /// </summary>
        [Column("ReqRotation")]
        public short RequiredRotation { get; set; }

    }
}
