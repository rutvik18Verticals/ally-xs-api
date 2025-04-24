namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the model for downtime information related to a well.
    /// </summary>
    public class DowntimeByWellsRodPumpModel : DowntimeByWellsModelBase
    {

        /// <summary>
        /// Gets or sets the runtime of the well.
        /// </summary>
        public float Runtime { get; set; }

        /// <summary>
        /// Gets or sets the idle time of the well.
        /// </summary>
        public float IdleTime { get; set; }

        /// <summary>
        /// Gets or sets the number of cycles of the well.
        /// </summary>
        public float Cycles { get; set; }

    }
}
