using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for an available view.
    /// </summary>
    public class AvailableViewOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<AvailableViewData> Values { get; set; }

    }
}
