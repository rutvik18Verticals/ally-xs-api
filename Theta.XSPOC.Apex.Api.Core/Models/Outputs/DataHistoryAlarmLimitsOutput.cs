using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the class for data history alarm limit.
    /// </summary>
    public class DataHistoryAlarmLimitsOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public IList<DataHistoryAlarmLimitsValues> Values { get; set; }

    }
}
