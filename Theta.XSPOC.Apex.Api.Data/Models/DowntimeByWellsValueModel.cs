namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a model for downtime by well value.
    /// </summary>
    public class DowntimeByWellsValueModel : DowntimeByWellsModelBase
    {

        /// <summary>
        /// Gets or sets the value of the downtime.
        /// </summary>
        public float Value { get; set; }

    }
}
