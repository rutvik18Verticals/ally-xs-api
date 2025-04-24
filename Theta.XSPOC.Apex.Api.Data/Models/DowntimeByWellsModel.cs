using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a model for downtime information by well.
    /// </summary>
    public class DowntimeByWellsModel
    {

        /// <summary>
        /// Gets or sets the rod pump downtime information.
        /// </summary>
        public IList<DowntimeByWellsRodPumpModel> RodPump { get; set; }

        /// <summary>
        /// Gets or sets the ESP downtime information.
        /// </summary>
        public IList<DowntimeByWellsValueModel> ESP { get; set; }

        /// <summary>
        /// Gets or sets the GL downtime information.
        /// </summary>
        public IList<DowntimeByWellsValueModel> GL { get; set; }

    }
}
