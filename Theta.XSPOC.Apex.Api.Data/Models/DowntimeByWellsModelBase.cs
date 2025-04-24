using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the base model for downtime information related to a well.
    /// </summary>
    public abstract class DowntimeByWellsModelBase
    {

        /// <summary>
        /// Gets or sets the Id of the well.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date of the downtime.
        /// </summary>
        public DateTime Date { get; set; }

    }
}
