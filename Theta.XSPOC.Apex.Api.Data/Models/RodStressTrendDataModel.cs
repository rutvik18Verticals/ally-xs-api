using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents rod stress trend data.
    /// </summary>
    public class RodStressTrendDataModel
    {

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the StressColumn.
        /// </summary>
        public float? StressColumn { get; set; }

    }
}
