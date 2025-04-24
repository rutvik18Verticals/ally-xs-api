namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the custom pumping unit model.
    /// </summary>
    public class CustomPumpingUnitModel
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the API designation.
        /// </summary>
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the other info.
        /// </summary>
        public string OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the number of crank holes.
        /// </summary>
        public short CrankHoles { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 1.
        /// </summary>
        public float? Stroke1 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 2.
        /// </summary>
        public float? Stroke2 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 3.
        /// </summary>
        public float? Stroke3 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 4.
        /// </summary>
        public float? Stroke4 { get; set; }

        /// <summary>
        /// Gets or sets the value for stroke 5.
        /// </summary>
        public float? Stroke5 { get; set; }

        /// <summary>
        /// Gets or sets the R1 dimension.
        /// </summary>
        public float R1 { get; set; }

        /// <summary>
        /// Gets or sets the R2 dimension.
        /// </summary>
        public float R2 { get; set; }

        /// <summary>
        /// Gets or sets the R3 dimension.
        /// </summary>
        public float R3 { get; set; }

        /// <summary>
        /// Gets or sets the R4 dimension.
        /// </summary>
        public float R4 { get; set; }

        /// <summary>
        /// Gets or sets the R5 dimension.
        /// </summary>
        public float R5 { get; set; }

        /// <summary>
        /// Gets or sets the A dimension.
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// Gets or sets the C dimension.
        /// </summary>
        public float C { get; set; }

        /// <summary>
        /// Gets or sets the I dimension.
        /// </summary>
        public float I { get; set; }

        /// <summary>
        /// Gets or sets the K dimension.
        /// </summary>
        public float K { get; set; }

        /// <summary>
        /// Gets or sets the P dimension.
        /// </summary>
        public float P { get; set; }

        /// <summary>
        /// Gets or sets the M dimension.
        /// </summary>
        public float M { get; set; }

        /// <summary>
        /// Gets or sets the S dimension.
        /// </summary>
        public float S { get; set; }

        /// <summary>
        /// Gets or sets the V0 dimension.
        /// </summary>
        public float V0 { get; set; }

        /// <summary>
        /// Gets or sets the drum diameter ratio.
        /// </summary>
        public float DrumDiameterRatio { get; set; }

        /// <summary>
        /// Gets or sets the sprocket diameter.
        /// </summary>
        public float SprocketDiameter { get; set; }

        /// <summary>
        /// Gets or sets the sprocket distance.
        /// </summary>
        public float SprocketDistance { get; set; }

        /// <summary>
        /// Gets or sets the unbalance.
        /// </summary>
        public float Unbalance { get; set; }

        /// <summary>
        /// Gets or sets the outer diameter.
        /// </summary>
        public float CrankOffset { get; set; }

        /// <summary>
        /// Gets or sets the structural rating.
        /// </summary>
        public float StructuralRating { get; set; }

        /// <summary>
        /// Gets or sets the gearbox rating.
        /// </summary>
        public float GearboxRating { get; set; }

        /// <summary>
        /// Gets or sets the maximum stroke.
        /// </summary>
        public float? MaximumStroke { get; set; }

        /// <summary>
        /// Gets or sets the articulating inertia.
        /// </summary>
        public float? ArticulatingInertia { get; set; }

        /// <summary>
        /// Gets or sets the Pitman arm length.
        /// </summary>
        public short PitmanArmLength { get; set; }

        /// <summary>
        /// Gets or sets the required rotation.
        /// </summary>
        public short RequiredRotation { get; set; }

    }
}
