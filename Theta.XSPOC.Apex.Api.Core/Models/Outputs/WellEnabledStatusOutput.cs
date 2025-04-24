using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the enabled status data for a well.
    /// </summary>
    public class WellEnabledStatusOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the enabled status of the well.
        /// </summary>
        public bool Enabled { get; set; }

    }
}
