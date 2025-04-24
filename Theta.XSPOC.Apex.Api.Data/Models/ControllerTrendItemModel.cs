namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The controller trend item model.
    /// </summary>
    public class ControllerTrendItemModel
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public int UnitType { get; set; }

        /// <summary>
        /// Gets or sets the facility tag.
        /// </summary>
        public int FacilityTag { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

    }
}
