namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the pumping unit manufacturer model.
    /// </summary>
    public class PumpingUnitManufacturerModel
    {

        /// <summary>
        /// Gets or sets the primary key for the table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer abbreviation.
        /// </summary>
        public string ManufacturerAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the unit type id.
        /// </summary>
        public int UnitTypeId { get; set; }

        /// <summary>
        /// Gets or sets the required rotation.
        /// </summary>
        public int RequiredRotation { get; set; }

    }
}
