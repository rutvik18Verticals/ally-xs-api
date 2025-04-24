namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the Pumping unit manufacturer to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class PumpingUnitManufacturer : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the pumping unit manufacturer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation of the pumping unit manufacturer (nullable).
        /// </summary>
        public string Abbrev { get; set; }

        /// <summary>
        /// Gets or sets the full name of the pumping unit manufacturer (nullable).
        /// </summary>
        public string Manuf { get; set; }

        /// <summary>
        /// Gets or sets the unit type Id (nullable).
        /// </summary>
        public int? UnitTypeId { get; set; }

        /// <summary>
        /// Gets or sets the required rotation value (nullable).
        /// </summary>
        public int? RequiredRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to sort by name.
        /// </summary>
        public bool? SortByName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show names.
        /// </summary>
        public bool? ShowNames { get; set; }

    }
}
