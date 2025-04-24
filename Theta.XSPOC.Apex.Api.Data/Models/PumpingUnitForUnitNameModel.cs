namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents a pumping unit for unit name model.
    /// </summary>
    public class PumpingUnitForUnitNameModel
    {

        /// <summary>
        /// Gets or sets the unit id. This is maintained for backward compatibility, the Id should be considered the
        /// primary key.
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the API designation. 
        /// </summary>
        public string APIDesignation { get; set; }

    }
}
