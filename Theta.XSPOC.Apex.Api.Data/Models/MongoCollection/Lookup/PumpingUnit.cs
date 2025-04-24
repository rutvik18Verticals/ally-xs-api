namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the Pumping unit to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class PumpingUnit : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the pumping unit.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier for the pumping unit (e.g., Unit Id).
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer identifier for the pumping unit (nullable).
        /// </summary>
        public string ManufId { get; set; }

        /// <summary>
        /// Gets or sets the API  designation for the pumping unit (nullable).
        /// </summary>
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the name of the pumping unit (nullable).
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// Gets or sets other information about the pumping unit (nullable).
        /// </summary>
        public string OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the number of crank holes (nullable).
        /// </summary>
        public int? CrankHoles { get; set; }

        /// <summary>
        /// Gets or sets the stroke measurements (nullable).
        /// </summary>
        public float? Stroke1 { get; set; }

        /// <summary>
        /// Gets or sets the second stroke  measurements (nullable). 
        /// </summary>
        public float? Stroke2 { get; set; }

        /// Gets or sets the third stroke  measurements (nullable). 
        public float? Stroke3 { get; set; }

        /// Gets or sets the fourth stroke  measurements (nullable). 
        public float? Stroke4 { get; set; }

        /// Gets or sets the fifth stroke  measurements (nullable). 
        public float? Stroke5 { get; set; }

        /// <summary>
        /// Gets or sets the structural rating (nullable).
        /// </summary>
        public float? StructRating { get; set; }

        /// <summary>
        /// Gets or sets the gearbox rating (nullable).
        /// </summary>
        public float? GearboxRating { get; set; }

        /// <summary>
        /// Gets or sets the maximum stroke (nullable).
        /// </summary>
        public float? MaxStroke { get; set; }

        /// <summary>
        /// Gets or sets the type of walking beam (nullable).
        /// </summary>
        public string WV_Typ { get; set; }

        /// <summary>
        /// Gets or sets the make of the walking beam (nullable).
        /// </summary>
        public string WV_Make { get; set; }

        /// <summary>
        /// Gets or sets the model of the walking beam (nullable).
        /// </summary>
        public string WV_Model { get; set; }

        /// <summary>
        /// Gets or sets other information about the walking beam (nullable).
        /// </summary>
        public string WV_OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the dimensions of the pumping unit (nullable).
        /// </summary>
        public string Dimensions { get; set; }

    }
}
