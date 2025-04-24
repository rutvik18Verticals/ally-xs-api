namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the model for downtime by wells influx.
    /// </summary>
    public class DowntimeByWellsInfluxModel : DowntimeByWellsModelBase
    {

        /// <summary>
        /// Gets or sets the parameter standard type.
        /// </summary>
        public string ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the value of the downtime.
        /// </summary>
        public float Value { get; set; }

    }
}
