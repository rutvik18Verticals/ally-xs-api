namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the Custom Pumping unit to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class PUCustom : LookupBase
    {

        /// <summary>
        /// Gets or sets the value for A.
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// Gets or sets the API designation for the pumping unit.
        /// </summary>
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the value for Artinertia (nullable).
        /// </summary>
        public float? Artinertia { get; set; }

        /// <summary>
        /// Gets or sets the value for C.
        /// </summary>
        public float C { get; set; }

        /// <summary>
        /// Gets or sets the number of crank holes (nullable).
        /// </summary>
        public short? Crankholes { get; set; }

        /// <summary>
        /// Gets or sets the value for Crankoffset.
        /// </summary>
        public float Crankoffset { get; set; }

        /// <summary>
        /// Gets or sets the value for Drumdiamratio.
        /// </summary>
        public float Drumdiamratio { get; set; }

        /// <summary>
        /// Gets or sets the value for Gearboxrating.
        /// </summary>
        public float Gearboxrating { get; set; }

        /// <summary>
        /// Gets or sets the value for I.
        /// </summary>
        public float I { get; set; }

        /// <summary>
        /// Gets or sets the value for K.
        /// </summary>
        public float K { get; set; }

        /// <summary>
        /// Gets or sets the value for M.
        /// </summary>
        public float M { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer identifier for the pumping unit.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the maximum stroke.
        /// </summary>
        public float Maxstroke { get; set; }

        /// <summary>
        /// Gets or sets other information about the pumping unit.
        /// </summary>
        public string OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the value for P.
        /// </summary>
        public float P { get; set; }

        /// <summary>
        /// Gets or sets the pitman arm length.
        /// </summary>
        public int Pitmanarmlength { get; set; }

        /// <summary>
        /// Gets or sets the value for R1.
        /// </summary>
        public float R1 { get; set; }

        /// <summary>
        /// Gets or sets the value for R2.
        /// </summary>
        public float R2 { get; set; }

        /// <summary>
        /// Gets or sets the value for R3.
        /// </summary>
        public float R3 { get; set; }

        /// <summary>
        /// Gets or sets the value for R4.
        /// </summary>
        public float R4 { get; set; }

        /// <summary>
        /// Gets or sets the value for R5.
        /// </summary>
        public float R5 { get; set; }

        /// <summary>
        /// Gets or sets the required rotation (nullable).
        /// </summary>
        public short? Reqrotation { get; set; }

        /// <summary>
        /// Gets or sets the value for S.
        /// </summary>
        public float S { get; set; }

        /// <summary>
        /// Gets or sets the sprocket diameter.
        /// </summary>
        public float Sprocketdiameter { get; set; }

        /// <summary>
        /// Gets or sets the sprocket distance.
        /// </summary>
        public float Sprocketdistance { get; set; }

        /// <summary>
        /// Gets or sets the first stroke measurement.
        /// </summary>
        public float Stroke1 { get; set; }

        /// <summary>
        /// Gets or sets the second stroke measurement (nullable).
        /// </summary>
        public float? Stroke2 { get; set; }

        /// <summary>
        /// Gets or sets the third stroke measurement (nullable).
        /// </summary>
        public float? Stroke3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth stroke measurement (nullable).
        /// </summary>
        public float? Stroke4 { get; set; }

        /// <summary>
        /// Gets or sets the fifth stroke measurement (nullable).
        /// </summary>
        public float? Stroke5 { get; set; }

        /// <summary>
        /// Gets or sets the structural rating.
        /// </summary>
        public float Structrating { get; set; }

        /// <summary>
        /// Gets or sets the unbalance value.
        /// </summary>
        public float Unbalance { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier for the pumping unit.
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the name of the pumping unit.
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public short UnitType { get; set; }

        /// <summary>
        /// Gets or sets the value for V0.
        /// </summary>
        public float V0 { get; set; }

    }
}
